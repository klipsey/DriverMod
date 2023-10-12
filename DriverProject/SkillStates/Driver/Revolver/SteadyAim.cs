using UnityEngine;
using RoR2;
using EntityStates;
using static RoR2.CameraTargetParams;
using RobDriver.Modules.Components;
using RoR2.UI;

namespace RobDriver.SkillStates.Driver.Revolver
{
    public class SteadyAim : BaseDriverSkillState
    {
        public float baseShotDuration = 0.4f;
        public float baseChargeDuration = 0.2f;

        public static float damageCoefficient = 5f;
        public static float recoil = 1f;

        private CameraParamsOverrideHandle camParamsOverrideHandle;
        private float shotCooldown;
        private float chargeTimer;
        private float chargeDuration;
        private bool isCharged;
        private bool isCrit;
        private PrimarySkillShurikenBehavior shurikenComponent;
        private bool autoFocus;
        private bool cancelling;

        private bool jamFlag; // fired shortly after entering state

        public override void OnEnter()
        {
            base.OnEnter();
            this.iDrive = this.GetComponent<DriverController>();
            this.chargeDuration = this.baseChargeDuration / this.attackSpeedStat;
            this.camParamsOverrideHandle = Modules.CameraParams.OverrideCameraParams(base.cameraTargetParams, DriverCameraParams.AIM_PISTOL, 0.5f);

            base.PlayAnimation("Gesture, Override", "SteadyAim", "Action.playbackRate", 0.25f);
            base.PlayAnimation("AimPitch", "SteadyAimPitch");

            this.characterBody.AddBuff(RoR2Content.Buffs.Slow50);

            this.shurikenComponent = this.GetComponent<PrimarySkillShurikenBehavior>();

            if (this.characterBody.master && this.characterBody.master.inventory)
            {
                this.characterBody.master.inventory.onInventoryChanged += Inventory_onInventoryChanged;
            }

            this.characterBody._defaultCrosshairPrefab = Modules.Assets.pistolAimCrosshairPrefab;
            this.autoFocus = Modules.Config.autoFocus.Value;
        }

        private void Inventory_onInventoryChanged()
        {
            this.shurikenComponent = this.GetComponent<PrimarySkillShurikenBehavior>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.shotCooldown -= Time.fixedDeltaTime;
            this.characterBody.outOfCombatStopwatch = 0f;
            this.characterBody.isSprinting = false;
            base.characterBody.SetAimTimer(0.2f);

            if (this.iDrive && this.iDrive.weaponDef != this.cachedWeaponDef)
            {
                this.cancelling = true;
                this.outer.SetNextStateToMain();
                return;
            }

            if (this.skillLocator.secondary.stock < 1)
            {
                this.isCharged = false;
                this.chargeTimer = 0f;
            }
            else
            {
                if (this.shotCooldown <= 0f) this.chargeTimer += Time.fixedDeltaTime;
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
                if (this.autoFocus)
                {
                    if (this.inputBank.skill1.down)
                    {
                        if (this.skillLocator.secondary.stock > 0)
                        {
                            if (this.isCharged)
                            {
                                this.isCrit = this.RollCrit();
                                this.Fire();
                            }
                        }
                        else
                        {
                            this.isCrit = this.RollCrit();
                            this.Fire();
                        }
                    }
                }
                else
                {
                    if (this.inputBank.skill1.down)
                    {
                        this.isCrit = this.RollCrit();
                        this.Fire();
                    }
                }
            }

            if (!this.inputBank.skill2.down && base.isAuthority)
            {
                if (this.jamFlag && this.shotCooldown > 0f)
                {
                    // add jam buildup
                    if (this.iDrive)
                    {
                        if (iDrive.AddJamBuildup())
                        {
                            this.outer.SetNextState(new JammedGun());
                            return;
                        }
                    }
                }

                this.outer.SetNextStateToMain();
            }

            if (this.iDrive)
            {
                if (this.skillLocator.secondary.stock < 1)
                {
                    this.iDrive.chargeValue = 0f;
                    return;
                }

                float value = Util.Remap(this.chargeTimer, 0f, this.chargeDuration, 0f, 1f);
                this.iDrive.chargeValue = value;
            }
        }

        public void Fire()
        {
            if (this.shurikenComponent) shurikenComponent.OnSkillActivated(base.skillLocator.primary);

            if (base.fixedAge <= 0.25f) this.jamFlag = true;

            bool wasCharged = this.isCharged;

            this.shotCooldown = this.baseShotDuration / this.attackSpeedStat;
            this.chargeTimer = 0f;
            this.isCharged = false;

            base.characterBody.AddSpreadBloom(1f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, "PistolMuzzle", false);

            string soundString = "sfx_driver_pistol_shoot";
            if (this.isCrit)
            {
                soundString = "sfx_driver_pistol_shoot_critical";
            }

            string animString = "SteadyAimFire";
            if (wasCharged)
            {
                soundString = "sfx_driver_pistol_shoot_charged";
                if (this.isCrit) animString = "SteadyAimFireChargedCritical";
                else animString = "SteadyAimFireCharged";
            }
            else
            {
                if (this.isCrit) animString = "SteadyAimFireCritical";
            }

            Util.PlaySound(soundString, this.gameObject);
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

                // ricochet here
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            this.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
            base.PlayAnimation("Gesture, Override", "SteadyAimEnd", "Action.playbackRate", 0.2f);
            base.PlayAnimation("AimPitch", "AimPitch");
            this.cameraTargetParams.RemoveParamsOverride(this.camParamsOverrideHandle);

            if (this.characterBody.master && this.characterBody.master.inventory)
            {
                this.characterBody.master.inventory.onInventoryChanged -= Inventory_onInventoryChanged;
            }

            if (!this.cancelling) this.characterBody._defaultCrosshairPrefab = this.iDrive.crosshairPrefab;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}