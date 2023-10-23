using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.Driver
{
    public class Shoot : BaseDriverSkillState
    {
        public static float damageCoefficient = 2.2f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.7f;
        public static float baseCritDuration = 0.9f;
        public static float force = 200f;
        public static float recoil = 2f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        public static GameObject critTracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCaptainShotgun");

        private float duration;
        private float fireTime;
        private float fireTime2;
        private bool hasFired;
        private bool hasFired2;
        private string muzzleString;
        private bool isCrit;
        private GameObject effectInstance;
        private uint spinPlayID;

        public virtual float _damageCoefficient
        {
            get
            {
                return Shoot.damageCoefficient;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Shoot.baseDuration / this.attackSpeedStat;
            this.characterBody.isSprinting = false;

            this.fireTime = 0.1f * this.duration;
            this.fireTime2 = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "PistolMuzzle";

            this.isCrit = base.RollCrit();

            if (this.isCrit)
            {
                this.duration = Shoot.baseCritDuration / this.attackSpeedStat;
                this.fireTime = 0.5f * this.duration;
                this.fireTime2 = 0.55f * this.duration;

                this.effectInstance = GameObject.Instantiate(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoReloadFX.prefab").WaitForCompletion());
                this.effectInstance.transform.parent = this.FindModelChild("Pistol");
                this.effectInstance.transform.localRotation = Quaternion.Euler(new Vector3(0f, 80f, 0f));
                this.effectInstance.transform.localPosition = Vector3.zero;

                this.spinPlayID = Util.PlaySound("sfx_driver_pistol_spin", this.gameObject);

                this.PlayAnimation("Gesture, Override", "ShootCritical", "Shoot.playbackRate", this.duration);
            }
            else
            {
                if (base.isAuthority)
                {
                    this.hasFired = true;
                    this.Fire();
                }

                this.PlayAnimation("Gesture, Override", "Shoot", "Shoot.playbackRate", this.duration * 1.5f);
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.spinPlayID != 0u) AkSoundEngine.StopPlayingID(this.spinPlayID);
            if (this.effectInstance) EntityState.Destroy(this.effectInstance);
        }

        public virtual string shootSoundString
        {
            get
            {
                if (this.isCrit) return "sfx_driver_pistol_shoot_critical";
                return "sfx_driver_pistol_shoot";
            }
        }

        public virtual BulletAttack.FalloffModel falloff
        {
            get
            {
                return BulletAttack.FalloffModel.DefaultBullet;
            }
        }

        private void Fire()
        {
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, this.gameObject, this.muzzleString, false);

            Util.PlaySound(this.shootSoundString, this.gameObject);

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                base.AddRecoil(-1f * Shoot.recoil, -2f * Shoot.recoil, -0.5f * Shoot.recoil, 0.5f * Shoot.recoil);

                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = this._damageCoefficient * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = this.falloff,
                    maxDistance = Shoot.range,
                    force = Shoot.force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = this.characterBody.spreadBloomAngle * 2f,
                    isCrit = this.isCrit,
                    owner = base.gameObject,
                    muzzleName = muzzleString,
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = procCoefficient,
                    radius = 0.75f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = this.tracerPrefab,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                }.Fire();
            }

            base.characterBody.AddSpreadBloom(1.25f);
        }

        private GameObject tracerPrefab
        {
            get
            {
                if (this.isCrit) return Shoot.critTracerEffectPrefab;
                else return Shoot.tracerEffectPrefab;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (this.iDrive && this.iDrive.weaponDef != this.cachedWeaponDef)
            {
                base.PlayAnimation("Gesture, Override", "BufferEmpty");
                this.outer.SetNextStateToMain();
                return;
            }

            if (base.fixedAge >= this.fireTime && base.isAuthority)
            {
                if (!this.hasFired)
                {
                    this.hasFired = true;
                    this.Fire();
                }
            }

            if (this.isCrit && base.fixedAge >= this.fireTime2 && base.isAuthority)
            {
                if (!this.hasFired2)
                {
                    this.hasFired2 = true;
                    this.Fire();
                }
            }

            if (this.effectInstance && base.fixedAge >= (0.4f * this.duration))
            {
                EntityState.Destroy(this.effectInstance);

                AkSoundEngine.StopPlayingID(this.spinPlayID);
                this.spinPlayID = 0u;
                Util.PlaySound("sfx_driver_pistol_ready", this.gameObject);
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            float kek = 0.5f;
            if (this.isCrit) kek = 0.75f;

            if (base.fixedAge >= kek * this.duration)
            {
                return InterruptPriority.Any;
            }

            if (this.isCrit && !this.hasFired2) return InterruptPriority.PrioritySkill;

            return InterruptPriority.Skill;
        }
    }
}