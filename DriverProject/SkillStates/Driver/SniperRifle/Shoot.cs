using RoR2;
using UnityEngine;
using EntityStates;

namespace RobDriver.SkillStates.Driver.SniperRifle
{
    public class Shoot : BaseDriverSkillState
    {
        public static float damageCoefficient = 8f;
        public static float procCoefficient = 1f;
        public float baseDuration = 1.6f;
        public static int bulletCount = 1;
        public static float bulletRecoil = 16f;
        public static float bulletRange = 250f;
        public static float bulletThiccness = 1f;
        public float selfForce = -500f;

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
            this.earlyExitTime = 0.75f * this.duration;

            Util.PlaySound("sfx_driver_sniper_shoot", base.gameObject);

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
                    this.iDrive.machineGunVFX.Play();
                }

                float recoilAmplitude = Shoot.bulletRecoil / this.attackSpeedStat;

                base.AddRecoil(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
                this.characterBody.AddSpreadBloom(4f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzleString, false);

                GameObject tracer = Modules.Assets.shotgunTracer;
                if (this.isCrit) tracer = Modules.Assets.shotgunTracerCrit;

                if (base.isAuthority)
                {
                    float damage = Shoot.damageCoefficient * this.damageStat;

                    Ray aimRay = GetAimRay();

                    float thiccness = Shoot.bulletThiccness;
                    float force = 2500;

                    new BulletAttack
                    {
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = bulletRange,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        isCrit = this.isCrit,
                        owner = gameObject,
                        muzzleName = muzzleString,
                        smartCollision = true,
                        procChainMask = default,
                        procCoefficient = procCoefficient,
                        radius = thiccness,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = tracer,
                        spreadPitchScale = 1f,
                        spreadYawScale = 1f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.hitEffectPrefab,
                        HitEffectNormal = false,
                        maxSpread = 5f,
                        minSpread = 2f,
                        bulletCount = 1
                    }.Fire();

                    this.characterMotor.ApplyForce(aimRay.direction * -this.selfForce);
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
            if (base.fixedAge >= this.earlyExitTime) return InterruptPriority.Any;
            return InterruptPriority.Skill;
        }
    }
}