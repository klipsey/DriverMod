using RoR2;
using EntityStates;
using RobDriver.SkillStates.BaseStates;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.Driver
{
    public class SwingKnife : BaseMeleeAttack
    {
        protected override string prop => "KnifeModel";
        protected override bool hideGun => iDrive.weaponDef.isMelee && Modules.Config.enableRevengence.Value;

        private GameObject swingEffectInstance;
        private bool wasActive;
        public override void OnEnter()
        {
            if (this.GetModelChildLocator().FindChild("BackWeaponModel").gameObject.activeSelf) 
            {
                this.GetModelChildLocator().FindChild("BackWeaponModel").gameObject.SetActive(false);
                this.wasActive = true;
            }
            this.hitboxName = "Knife";

            this.damageCoefficient = 4.7f;
            this.pushForce = 200f;
            this.baseDuration = 1.2f;
            this.baseEarlyExitTime = 0.5f;
            this.attackRecoil = 5f / this.attackSpeedStat;

            this.attackStartTime = 0.13f;
            this.attackEndTime = 0.5f;

            this.hitStopDuration = 0.18f;
            this.smoothHitstop = true;

            this.swingSoundString = "sfx_driver_swing_knife";
            this.swingEffectPrefab = RobDriver.Modules.Config.enabledRedVfxForKnife.Value ? Modules.Assets.redSmallSlashEffect : Modules.Assets.knifeSwingEffect;
            this.hitSoundString = "";
            this.hitEffectPrefab = RobDriver.Modules.Config.enabledRedVfxForKnife.Value ? Modules.Assets.redSlashImpactEffect : Modules.Assets.knifeImpactEffect;
            this.impactSound = Modules.Assets.knifeImpactSoundDef.index;

            this.damageType = DamageType.ApplyMercExpose;

            this.muzzleString = "KnifeSwingMuzzle";

            base.OnEnter();

            Util.PlaySound("sfx_driver_foley_knife", this.gameObject);
        }

        protected override void FireAttack()
        {
            if (base.isAuthority)
            {
                Vector3 direction = this.GetAimRay().direction;
                direction.y = Mathf.Max(direction.y, direction.y * 0.5f);
                this.FindModelChild("MeleePivot").rotation = Util.QuaternionSafeLookRotation(direction);
            }

            base.FireAttack();
        }

        protected override void PlaySwingEffect()
        {
            Util.PlaySound(this.swingSoundString, this.gameObject);
            if (this.swingEffectPrefab)
            {
                Transform muzzleTransform = this.FindModelChild(this.muzzleString);
                if (muzzleTransform)
                {
                    this.swingEffectInstance = UnityEngine.Object.Instantiate<GameObject>(this.swingEffectPrefab, muzzleTransform);
                    ScaleParticleSystemDuration fuck = this.swingEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                    if (fuck) fuck.newDuration = fuck.initialDuration;
                }
            }
        }

        protected override void TriggerHitStop()
        {
            base.TriggerHitStop();

            if (this.swingEffectInstance)
            {
                ScaleParticleSystemDuration fuck = this.swingEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                if (fuck) fuck.newDuration = 20f;
            }
        }

        protected override void ClearHitStop()
        {
            base.ClearHitStop();

            if (this.swingEffectInstance)
            {
                ScaleParticleSystemDuration fuck = this.swingEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                if (fuck) fuck.newDuration = fuck.initialDuration;
            }
        }

        protected override void PlayAttackAnimation()
        {
            base.PlayCrossfade("Gesture, Override", "SwingKnife", "Slash.playbackRate", this.duration, 0.1f);
        }

        protected override void SetNextState()
        {
        }

        public override void OnExit()
        {
            if (this.wasActive == true)
            {
                this.GetModelChildLocator().FindChild("BackWeaponModel").gameObject.SetActive(true);
            }
            base.OnExit();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (this.stopwatch >= (0.5f * this.duration)) return InterruptPriority.Any;
            else return InterruptPriority.Pain;
        }
    }
}