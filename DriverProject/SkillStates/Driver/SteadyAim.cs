using UnityEngine;
using RoR2;
using EntityStates;
using static RoR2.CameraTargetParams;

namespace RobDriver.SkillStates.Driver
{
    public class SteadyAim : BaseSkillState
    {
        public float baseShotDuration = 0.4f;
        public float baseChargeDuration = 0.5f;

        public static float damageCoefficient = 5f;
        public static float recoil = 1f;

        private bool lastCharge;

        private CameraParamsOverrideHandle camParamsOverrideHandle;
        private float shotCooldown;
        private float chargeTimer;
        private float chargeDuration;
        private bool isCharged;
        private bool isCrit;
        private int cachedShots;
        private float cachedShotTimer;

        public override void OnEnter()
        {
            base.OnEnter();
            this.chargeDuration = this.baseChargeDuration / this.attackSpeedStat;
            this.camParamsOverrideHandle = Modules.CameraParams.OverrideCameraParams(base.cameraTargetParams, DriverCameraParams.AIM_PISTOL, 0.5f);

            base.PlayAnimation("Gesture, Override", "SteadyAim", "Action.playbackRate", 0.25f);
            base.PlayAnimation("AimPitch", "SteadyAimPitch");

            this.characterBody.AddBuff(RoR2Content.Buffs.Slow50);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.shotCooldown -= Time.fixedDeltaTime;
            this.chargeTimer += Time.fixedDeltaTime;
            this.cachedShotTimer -= Time.fixedDeltaTime;
            this.characterBody.outOfCombatStopwatch = 0f;
            this.characterBody.isSprinting = false;
            base.characterBody.SetAimTimer(0.2f);

            if (this.skillLocator.secondary.stock < 1)
            {
                this.isCharged = false;
                this.chargeTimer = 0f;
            }

            if (!this.isCharged)
            {
                if (this.chargeTimer >= this.chargeDuration)
                {
                    this.isCharged = true;
                    Util.PlaySound("sfx_driver_pistol_ready", this.gameObject);
                }
            }

            if (this.shotCooldown <= 0f && base.isAuthority)
            {
                if (this.inputBank.skill1.down)
                {
                    this.isCrit = this.RollCrit();
                    this.Fire();
                }
            }

            if (this.cachedShots > 0 && base.isAuthority)
            {
                if (this.cachedShotTimer <= 0f)
                {
                    this.Fire2(this.cachedShots);
                    this.cachedShots = 0;
                }
            }

            if (!this.inputBank.skill2.down && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public void Fire()
        {
            bool wasCharged = this.isCharged;

            this.shotCooldown = this.baseShotDuration / this.attackSpeedStat;
            this.chargeTimer = 0f;
            this.isCharged = false;

            base.characterBody.AddSpreadBloom(1f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, "PistolMuzzle", false);

            if (this.isCrit)
            {
                Util.PlaySound("sfx_driver_pistol_shoot_critical", base.gameObject);

                this.cachedShots++;
                this.cachedShotTimer = 0.05f;
            }
            else Util.PlaySound("sfx_driver_pistol_shoot", base.gameObject);

            string animString = "SteadyAimFire";
            if (wasCharged)
            {
                if (this.isCrit) animString = "SteadyAimFireChargedCritical";
                else animString = "SteadyAimFireCharged";
            }
            else
            {
                if (this.isCrit) animString = "SteadyAimFireCritical";
            }
            base.PlayAnimation("Gesture, Override", animString, "Action.playbackRate", this.shotCooldown * 1.5f);

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                base.AddRecoil(-1f * SteadyAim.recoil, -2f * SteadyAim.recoil, -0.5f * SteadyAim.recoil, 0.5f * SteadyAim.recoil);

                float dmg = Shoot.damageCoefficient;

                if (wasCharged)
                {
                    dmg = SteadyAim.damageCoefficient;

                    this.skillLocator.secondary.DeductStock(1);
                }

                this.lastCharge = wasCharged;

                GameObject tracerPrefab = Shoot.tracerEffectPrefab;
                if (isCrit) tracerPrefab = Shoot.critTracerEffectPrefab;

                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = dmg * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = Shoot.range,
                    force = Shoot.force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = isCrit,
                    owner = base.gameObject,
                    muzzleName = "PistolMuzzle",
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = Shoot.procCoefficient,
                    radius = 1f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = tracerPrefab,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                }.Fire();
            }
        }

        public void Fire2(int bulletCount)
        {
            // this is literally the worst POSSIBLE way to do this.
            // FUCK!!

            base.characterBody.AddSpreadBloom(1f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, "PistolMuzzle", false);

            Util.PlaySound("sfx_driver_pistol_shoot_critical", base.gameObject);

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                base.AddRecoil(-1f * Shoot.recoil, -2f * Shoot.recoil, -0.5f * Shoot.recoil, 0.5f * Shoot.recoil);

                float dmg = Shoot.damageCoefficient;

                if (this.lastCharge)
                {
                    dmg = SteadyAim.damageCoefficient;
                }

                GameObject tracerPrefab = Shoot.critTracerEffectPrefab;

                new BulletAttack
                {
                    bulletCount = (uint)bulletCount,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = dmg * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = Shoot.range,
                    force = Shoot.force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = isCrit,
                    owner = base.gameObject,
                    muzzleName = "PistolMuzzle",
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = Shoot.procCoefficient,
                    radius = 1f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = tracerPrefab,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                }.Fire();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            this.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
            base.PlayAnimation("Gesture, Override", "SteadyAimEnd", "Action.playbackRate", 0.2f);
            base.PlayAnimation("AimPitch", "AimPitch");
            this.cameraTargetParams.RemoveParamsOverride(this.camParamsOverrideHandle);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}