using UnityEngine;
using RoR2;
using EntityStates;

namespace RobDriver.SkillStates.Driver.Skateboard
{
    public class Start : BaseDriverSkillState
    {
        public float baseDuration = 0.5f;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;

            if (this.iDrive) this.iDrive.ToggleSkateboard(SkateboardState.Transitioning);

            this.PlayAnimation("FullBody, Override", "StartSkate", "Slide.playbackRate", this.duration);

            this.SmallHop(this.characterMotor, 10f);

            this.skillLocator.utility.SetSkillOverride(this.skillLocator.utility, Modules.Survivors.Driver.skateCancelSkillDef, GenericSkill.SkillOverridePriority.Replacement);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (this.outer.destroying)
            {
                if (this.iDrive) this.iDrive.ToggleSkateboard(SkateboardState.Inactive);
            }
            else
            {
                if (this.iDrive) this.iDrive.ToggleSkateboard(SkateboardState.Active);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextState(new Idle());
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}