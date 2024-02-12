using UnityEngine;
using RoR2;
using EntityStates;
using static RoR2.CameraTargetParams;
using UnityEngine.Networking;

namespace RobDriver.SkillStates.Driver
{
    public class SteadyAim : BaseDriverSkillState
    {
        public float baseShotDuration = 0.3f;
        public float baseChargeDuration = 0.15f;

        public static float damageCoefficient = 5f;
        public static float recoil = 0.5f;

        protected bool lastCharge;

        protected virtual bool isPiercing
        {
            get
            {
                return false;
            }
        }

        protected virtual float _damageCoefficient
        {
            get
            {
                return SteadyAim.damageCoefficient;
            }
        }

        protected virtual GameObject tracerPrefab
        {
            get
            {
                if (this.isCrit) return Shoot.critTracerEffectPrefab;
                return Shoot.tracerEffectPrefab;
            }
        }

        private CameraParamsOverrideHandle camParamsOverrideHandle;
        private float shotCooldown;
        private float chargeTimer;
        private float chargeDuration;
        private bool isCharged;
        private bool isCrit;
        private int cachedShots;
        private float cachedShotTimer;
        private PrimarySkillShurikenBehavior shurikenComponent;
        private bool autoFocus;
        private bool cancelling;

        private bool jamFlag; // fired shortly after entering state

        public override void OnEnter()
        {
            base.OnEnter();
            this.chargeDuration = this.baseChargeDuration / this.attackSpeedStat;
            this.camParamsOverrideHandle = Modules.CameraParams.OverrideCameraParams(base.cameraTargetParams, DriverCameraParams.AIM_PISTOL, 0.5f);

            this.PlayAnim();
            base.PlayAnimation("AimPitch", "SteadyAimPitch");

            if (NetworkServer.active) this.characterBody.AddBuff(RoR2Content.Buffs.Slow50);

            this.shurikenComponent = this.GetComponent<PrimarySkillShurikenBehavior>();

            if (this.characterBody.master && this.characterBody.master.inventory)
            {
                this.characterBody.master.inventory.onInventoryChanged += Inventory_onInventoryChanged;
            }

            this.characterBody._defaultCrosshairPrefab = Modules.Assets.pistolAimCrosshairPrefab;
            this.autoFocus = Modules.Config.autoFocus.Value;

            if (Modules.Config.adaptiveFocus.Value)
            {
                if (this.chargeDuration <= 0.1f) this.autoFocus = true;
            }

            this.FindModelChild("PistolSight").gameObject.SetActive(true);
        }

        protected virtual void PlayAnim()
        {
            base.PlayAnimation("Gesture, Override", "SteadyAim", "Action.playbackRate", 0.25f);
        }

        protected virtual void PlayExitAnim()
        {
            base.PlayAnimation("Gesture, Override", "SteadyAimEnd", "Action.playbackRate", 0.2f);
        }

        private void Inventory_onInventoryChanged()
        {
            this.shurikenComponent = this.GetComponent<PrimarySkillShurikenBehavior>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.shotCooldown -= Time.fixedDeltaTime;
            this.cachedShotTimer -= Time.fixedDeltaTime;
            this.characterBody.outOfCombatStopwatch = 0f;
            this.characterBody.isSprinting = false;
            base.characterBody.SetAimTimer(0.2f);
            this.attackSpeedStat = this.characterBody.attackSpeed;
            this.damageStat = this.characterBody.damage;
            this.critStat = this.characterBody.crit;

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

        protected virtual void PlayShootAnim(bool wasCharged, bool wasCrit, float speed)
        {
            string animString = "SteadyAimFire";

            if (wasCharged)
            {
                if (wasCrit) animString = "SteadyAimFireChargedCritical";
                else animString = "SteadyAimFireCharged";
            }
            else
            {
                if (wasCrit) animString = "SteadyAimFireCritical";
            }

            base.PlayAnimation("Gesture, Override", animString, "Action.playbackRate", speed);
        }

        protected virtual string GetSoundString(bool crit, bool charged)
        {
            string soundString = "sfx_driver_pistol_shoot";
            if (crit) soundString = "sfx_driver_pistol_shoot_critical";
            if (charged) soundString = "sfx_driver_pistol_shoot_charged";
            return soundString;
        }

        public virtual void Fire()
        {
            if (this.shurikenComponent) shurikenComponent.OnSkillActivated(base.skillLocator.primary);

            if (base.fixedAge <= 0.25f) this.jamFlag = true;

            bool wasCharged = this.isCharged;

            this.shotCooldown = this.baseShotDuration / this.attackSpeedStat;
            this.chargeTimer = 0f;
            this.isCharged = false;

            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, "PistolMuzzle", false);

            
            if (this.isCrit)
            {
                this.cachedShots++;
                this.cachedShotTimer = 0.05f;
            }

            if (wasCharged)
            {
                base.characterBody.AddSpreadBloom(1.5f);
            }
            else
            {
                base.characterBody.AddSpreadBloom(0.35f);
            }

            Util.PlaySound(this.GetSoundString(this.isCrit, wasCharged), this.gameObject);

            this.PlayShootAnim(wasCharged, this.isCrit, this.shotCooldown * 1.5f);

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                base.AddRecoil(-1f * SteadyAim.recoil, -2f * SteadyAim.recoil, -0.5f * SteadyAim.recoil, 0.5f * SteadyAim.recoil);

                float dmg = Shoot.damageCoefficient;

                if (wasCharged)
                {
                    dmg = this._damageCoefficient;

                    this.skillLocator.secondary.DeductStock(1);
                }

                this.lastCharge = wasCharged;

                BulletAttack.FalloffModel falloffModel = BulletAttack.FalloffModel.DefaultBullet;
                if (this.lastCharge) falloffModel = BulletAttack.FalloffModel.None;

                if (this.isPiercing)
                {
                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = dmg * this.damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = falloffModel,
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
                        stopperMask = LayerIndex.world.mask,
                        weapon = null,
                        tracerEffectPrefab = this.tracerPrefab,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                    }.Fire();
                }
                else
                {
                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = dmg * this.damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = falloffModel,
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
                        tracerEffectPrefab = this.tracerPrefab,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                    }.Fire();
                }
            }
        }

        public void Fire2(int bulletCount)
        {
            // this is literally the worst POSSIBLE way to do this.
            // FUCK!!

            base.characterBody.AddSpreadBloom(1.15f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, "PistolMuzzle", false);

            Util.PlaySound("sfx_driver_pistol_shoot_charged", base.gameObject);

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                base.AddRecoil(-1f * Shoot.recoil, -2f * Shoot.recoil, -0.5f * Shoot.recoil, 0.5f * Shoot.recoil);

                float dmg = Shoot.damageCoefficient;

                BulletAttack.FalloffModel falloffModel = BulletAttack.FalloffModel.DefaultBullet;

                if (this.lastCharge)
                {
                    dmg = SteadyAim.damageCoefficient;
                    falloffModel = BulletAttack.FalloffModel.None;
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
                    falloffModel = falloffModel,
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

            if (this.iDrive) this.iDrive.chargeValue = 0f;

            if (NetworkServer.active) this.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);

            this.PlayExitAnim();
            base.PlayAnimation("AimPitch", "AimPitch");
            this.cameraTargetParams.RemoveParamsOverride(this.camParamsOverrideHandle);

            if (this.characterBody.master && this.characterBody.master.inventory)
            {
                this.characterBody.master.inventory.onInventoryChanged -= Inventory_onInventoryChanged;
            }

            if (!this.cancelling) this.characterBody._defaultCrosshairPrefab = this.iDrive.crosshairPrefab;

            this.FindModelChild("PistolSight").gameObject.SetActive(false);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}