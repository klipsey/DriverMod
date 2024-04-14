using RoR2;
using UnityEngine;
using EntityStates;
using RobDriver.Modules.Components;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using R2API;

namespace RobDriver.SkillStates.Driver.RocketLauncher
{
    public class Shoot : BaseDriverSkillState
    {
        public static float damageCoefficient = 5f;
        public static float procCoefficient = 1f;
        public float baseDuration = 1.3f; // the base skill duration. i.e. attack speed
        public static float recoil = 16f;

        private float earlyExitTime;
        protected float duration;
        protected float fireDuration;
        protected bool hasFired;
        private bool isCrit;
        protected string muzzleString;

        protected virtual string soundString
        {
            get
            {
                if (this.isCrit) return "sfx_driver_rocket_launcher_shoot";
                return "sfx_driver_rocket_launcher_shoot";
            }
        }

        protected virtual GameObject projectilePrefab
        {
            get
            {
                return Modules.Projectiles.rocketProjectilePrefab;
            }
        }

        protected virtual float ammoMod
        {
            get
            {
                return 1.5f;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            this.characterBody.SetAimTimer(5f);
            this.muzzleString = "ShotgunMuzzle";
            this.hasFired = false;
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.isCrit = base.RollCrit();
            this.earlyExitTime = 0.4f * this.duration;

            Util.PlaySound(this.soundString, base.gameObject);

            //this.PlayCrossfade("Gesture, Override", "FireShotgun", "Shoot.playbackRate", Mathf.Max(0.05f, 1.75f * duration), 0.06f);
            base.PlayAnimation("Gesture, Override", "FireTwohand", "Shoot.playbackRate", this.duration);
            base.PlayAnimation("AimPitch", "Shoot");

            this.fireDuration = 0;

            if (this.iDrive) this.iDrive.ConsumeAmmo(this.ammoMod);
        }

        protected virtual float _damageCoefficient
        {
            get
            {
                return Shoot.damageCoefficient;
            }
        }

        public virtual void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                float recoilAmplitude = Shoot.recoil / this.attackSpeedStat;

                base.AddRecoil2(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
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
                            GameObject modify = this.projectilePrefab;
                            modify.GetComponent<ProjectileDamage>().damageType = iDrive.DamageType;
                            if (!modify.GetComponent<DamageAPI.ModdedDamageTypeHolderComponent>()) modify.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
                            modify.GetComponent<DamageAPI.ModdedDamageTypeHolderComponent>().Add(iDrive.ModdedDamageType);
                            ProjectileManager.instance.FireProjectile(modify, aimRay2.origin, Util.QuaternionSafeLookRotation(aimRay2.direction), this.gameObject, damageMult * this.damageStat * this._damageCoefficient, 1200f, this.isCrit, DamageColorIndex.Default, null, 120f);
                            modify.GetComponent<DamageAPI.ModdedDamageTypeHolderComponent>().Remove(iDrive.ModdedDamageType);
                            aimRay2.direction = rotation * aimRay2.direction;
                        }
                    }
                    else
                    {
                        GameObject modify = this.projectilePrefab;
                        modify.GetComponent<ProjectileDamage>().damageType = iDrive.DamageType;
                        if (!modify.GetComponent<DamageAPI.ModdedDamageTypeHolderComponent>()) modify.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
                        modify.GetComponent<DamageAPI.ModdedDamageTypeHolderComponent>().Add(iDrive.ModdedDamageType);
                        ProjectileManager.instance.FireProjectile(modify, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), this.gameObject, this.damageStat * this._damageCoefficient, 1200f, this.isCrit, DamageColorIndex.Default, null, 120f);
                        modify.GetComponent<DamageAPI.ModdedDamageTypeHolderComponent>().Remove(iDrive.ModdedDamageType);
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
                base.PlayAnimation("Gesture, Override", this.iDrive.weaponDef.equipAnimationString);
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

            this.GetModelAnimator().SetTrigger("endAim");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (base.fixedAge >= this.earlyExitTime) return InterruptPriority.Any;
            return InterruptPriority.Skill;
        }
    }
}