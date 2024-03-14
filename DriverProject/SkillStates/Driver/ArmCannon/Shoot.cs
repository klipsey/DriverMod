using RoR2;
using UnityEngine;
using EntityStates;
using RobDriver.Modules.Components;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.Driver.ArmCannon
{
    public class Shoot : BaseDriverSkillState
    {
        public static float damageCoefficient = 10f;
        public static float procCoefficient = 1f;
        public float baseDuration = 1.2f; // the base skill duration. i.e. attack speed
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

            base.PlayAnimation("Gesture, Override", "Shoot", "Shoot.playbackRate", this.duration);

            this.fireDuration = 0;

            if (this.iDrive) this.iDrive.StartTimer();
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
                    ProjectileManager.instance.FireProjectile(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Vagrant/VagrantCannon.prefab").WaitForCompletion(), aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), this.gameObject, this.damageStat * this._damageCoefficient, 1200f, this.isCrit, DamageColorIndex.Default, null, 120f);
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
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (base.fixedAge >= this.earlyExitTime) return InterruptPriority.Any;
            return InterruptPriority.Skill;
        }
    }
}