using RoR2;
using UnityEngine;
using EntityStates;
using RobDriver.Modules.Components;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.Driver.HeavyMachineGun
{
    public class ShootGrenade : BaseDriverSkillState
    {
        public static float damageCoefficient = 8f;
        public static float procCoefficient = 1f;
        public float baseDuration = 0.6f; // the base skill duration. i.e. attack speed
        public static float recoil = 16f;

        private float earlyExitTime;
        protected float duration;
        protected float fireDuration;
        protected bool hasFired;
        private bool isCrit;
        protected string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.characterBody.SetAimTimer(5f);
            this.muzzleString = "ShotgunMuzzle";
            this.hasFired = false;
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.isCrit = base.RollCrit();
            this.earlyExitTime = 0.4f * this.duration;

            if (this.isCrit) Util.PlaySound("sfx_driver_rocket_launcher_shoot", base.gameObject);
            else Util.PlaySound("sfx_driver_rocket_launcher_shoot", base.gameObject);

            //this.PlayCrossfade("Gesture, Override", "FireShotgun", "Shoot.playbackRate", Mathf.Max(0.05f, 1.75f * duration), 0.06f);
            base.PlayAnimation("Gesture, Override", "FireShotgun", "Shoot.playbackRate", this.duration);

            this.fireDuration = 0;

            if (this.iDrive) this.iDrive.StartTimer();
        }

        public virtual void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                float recoilAmplitude = ShootGrenade.recoil / this.attackSpeedStat;

                base.AddRecoil(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
                this.characterBody.AddSpreadBloom(4f);
                EffectManager.SimpleMuzzleFlash(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/MuzzleflashSmokeRing.prefab").WaitForCompletion(), this.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    Ray aimRay = this.GetAimRay();

                    // copied from moff's rocket
                    // the fact that this item literally has to be hardcoded into character skillstates makes me so fucking angry you have no idea
                    if (this.characterBody.inventory && this.characterBody.inventory.GetItemCount(DLC1Content.Items.MoreMissile) > 0)
                    {
                        float damageMult = DriverPlugin.GetICBMDamageMult(this.characterBody);

                        Vector3 rhs = Vector3.Cross(Vector3.up, aimRay.direction);
                        Vector3 axis = Vector3.Cross(aimRay.direction, rhs);

                        float currentSpread = 0f;
                        float angle = 0f;
                        float num2 = 0f;
                        num2 = UnityEngine.Random.Range(1f + currentSpread, 1f + currentSpread) * 3f;   //Bandit is x2
                        angle = num2 / 2f;  //3 - 1 rockets

                        Vector3 direction = Quaternion.AngleAxis(-num2 * 0.5f, axis) * aimRay.direction;
                        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
                        Ray aimRay2 = new Ray(aimRay.origin, direction);
                        for (int i = 0; i < 3; i++)
                        {
                            ProjectileManager.instance.FireProjectile(Modules.Projectiles.hmgGrenadeProjectilePrefab, aimRay2.origin, Util.QuaternionSafeLookRotation(aimRay2.direction), this.gameObject, damageMult * this.damageStat * ShootGrenade.damageCoefficient, 1200f, this.RollCrit(), DamageColorIndex.Default, null, 80f);
                            aimRay2.direction = rotation * aimRay2.direction;
                        }
                    }
                    else
                    {
                        ProjectileManager.instance.FireProjectile(Modules.Projectiles.hmgGrenadeProjectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), this.gameObject, this.damageStat * ShootGrenade.damageCoefficient, 1200f, this.RollCrit(), DamageColorIndex.Default, null, 80f);
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireDuration)
            {
                this.Fire();
            }

            if (this.iDrive && this.iDrive.weaponDef != this.cachedWeaponDef)
            {
                base.PlayAnimation("Gesture, Override", "BufferEmpty");
                this.outer.SetNextStateToMain();
                return;
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (base.fixedAge >= this.earlyExitTime && this.hasFired) return InterruptPriority.Any;
            return InterruptPriority.PrioritySkill;
        }
    }
}