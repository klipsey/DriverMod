using UnityEngine;
using RoR2;
using EntityStates;

namespace RobDriver.SkillStates.Driver.Skateboard
{
    public class Stop : BaseDriverSkillState
    {
        public float baseDuration = 0.6f;
        protected override bool hideGun => true;
        protected override string prop => "SkateboardModel";

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;

            this.skillLocator.utility.UnsetSkillOverride(this.skillLocator.utility, Modules.Survivors.Driver.skateCancelSkillDef, GenericSkill.SkillOverridePriority.Replacement);

            Util.PlaySound("sfx_driver_foley_syringe", this.gameObject);
            this.PlayAnimation("FullBody, Override", "StopSkate", "Slide.playbackRate", 0.6f / attackSpeedStat);
            this.SmallHop(this.characterMotor, 10f);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (this.iDrive.weaponEffectInstance) Destroy(this.iDrive.weaponEffectInstance);
            this.GetModelChildLocator().FindChild("SkateboardBackModel").gameObject.SetActive(true);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.iDrive.weaponEffectInstance && base.fixedAge >= this.duration * 0.8f)
            {
                this.GetModelChildLocator().FindChild("PistolModel").gameObject.SetActive(true);
                this.GetModelChildLocator().FindChild("SkateboardModel").gameObject.SetActive(false);
                this.GetModelChildLocator().FindChild("SkateboardBackModel").gameObject.SetActive(true);
                Destroy(this.iDrive.weaponEffectInstance);
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextState(new WaitForReload());
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (base.fixedAge >= this.duration / 2f) return InterruptPriority.Any;
            return InterruptPriority.PrioritySkill;
        }
    }
}