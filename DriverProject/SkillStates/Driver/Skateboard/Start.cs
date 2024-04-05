using UnityEngine;
using RoR2;
using EntityStates;

namespace RobDriver.SkillStates.Driver.Skateboard
{
    public class Start : BaseDriverSkillState
    {
        public float baseDuration = 0.5f;

        protected override bool hideGun => true;
        protected override string prop => "SkateboardModel";

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;

            this.skillLocator.utility.SetSkillOverride(this.skillLocator.utility, Modules.Survivors.Driver.skateCancelSkillDef, GenericSkill.SkillOverridePriority.Replacement);
            this.GetModelChildLocator().FindChild("SkateboardBackModel").gameObject.SetActive(false);

            // pistol has good animation blending, others dont
            if (iDrive.weaponDef != DriverWeaponCatalog.Pistol && 
                iDrive.weaponDef != DriverWeaponCatalog.VoidPistol && 
                iDrive.weaponDef != DriverWeaponCatalog.LunarPistol)
            {
                base.PlayAnimation("Gesture, Override", "BufferEmpty");
            }

            Util.PlaySound("sfx_driver_foley_syringe", this.gameObject);
            base.PlayAnimation("FullBody, Override", "StartSkate", "Slide.playbackRate", this.duration);

            this.SmallHop(this.characterMotor, 10f);
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
            return InterruptPriority.Death;
        }
    }
}