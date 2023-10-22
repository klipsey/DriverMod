using UnityEngine;
using RoR2;
using EntityStates;

namespace RobDriver.SkillStates.Driver.Skateboard
{
    public class Stop : BaseDriverSkillState
    {
        public float baseDuration = 0.6f;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;

            if (this.iDrive) this.iDrive.ToggleSkateboard(SkateboardState.Transitioning);

            this.PlayAnimation("FullBody, Override", "StopSkate", "Slide.playbackRate", this.duration);

            this.SmallHop(this.characterMotor, 10f);

            this.skillLocator.utility.UnsetSkillOverride(this.skillLocator.utility, Modules.Survivors.Driver.skateCancelSkillDef, GenericSkill.SkillOverridePriority.Replacement);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (this.iDrive) this.iDrive.ToggleSkateboard(SkateboardState.Inactive);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}