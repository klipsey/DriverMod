using RoR2;
using UnityEngine;
using EntityStates;
using RobDriver.Modules.Components;
using RoR2.Projectile;

namespace RobDriver.SkillStates.Driver.RocketLauncher
{
    public class Shoot : BaseDriverSkillState
    {
        public static float damageCoefficient = 6f;
        public static float procCoefficient = 1f;
        public float baseDuration = 2.2f; // the base skill duration. i.e. attack speed
        public static float recoil = 8f;

        private float earlyExitTime;
        protected float duration;
        protected float fireDuration;
        protected bool hasFired;
        private Animator animator;
        private bool isCrit;
        protected string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.characterBody.SetAimTimer(5f);
            this.animator = GetModelAnimator();
            this.muzzleString = "ShotgunMuzzle";
            this.hasFired = false;
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.isCrit = base.RollCrit();
            this.earlyExitTime = 0.4f * this.duration;

            if (this.isCrit) Util.PlaySound("sfx_driver_shotgun_shoot_critical", base.gameObject);
            else Util.PlaySound("sfx_driver_shotgun_shoot", base.gameObject);

            //this.PlayCrossfade("Gesture, Override", "FireShotgun", "Shoot.playbackRate", Mathf.Max(0.05f, 1.75f * duration), 0.06f);
            base.PlayAnimation("Gesture, Override", "FireShotgun", "Shoot.playbackRate", this.duration);

            this.fireDuration = 0;

            if (this.iDrive) this.iDrive.StartTimer();
        }

        public virtual void FireBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                if (this.iDrive)
                {
                    this.iDrive.DropShell(-this.GetModelBaseTransform().transform.right * -Random.Range(4, 12));
                    this.iDrive.DropShell(-this.GetModelBaseTransform().transform.right * -Random.Range(4, 12));
                }

                float recoilAmplitude = Shoot.recoil / this.attackSpeedStat;

                base.AddRecoil(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
                characterBody.AddSpreadBloom(4f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzleString, false);

                if (base.isAuthority)
                {
                    Ray aimRay = this.GetAimRay();
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.stunGrenadeProjectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageStat * Shoot.damageCoefficient, 1200f, this.RollCrit());
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireDuration)
            {
                this.FireBullet();
            }

            if (this.iDrive && this.iDrive.weapon != DriverWeapon.Shotgun)
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
            if (base.fixedAge >= this.earlyExitTime) return InterruptPriority.Any;
            return InterruptPriority.Skill;
        }
    }
}