using UnityEngine;
using EntityStates;

namespace RobDriver.SkillStates.Driver.Shotgun
{
    public class Reload : BaseDriverSkillState
    {
        public float duration = 1.75f;

        public override void OnEnter()
        {
            base.OnEnter();

            base.PlayAnimation("Gesture, Override", "ReloadShotgun", "Shoot.playbackRate", this.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (this.iDrive && this.iDrive.weapon != DriverWeapon.Shotgun)
            {
                base.PlayAnimation("Gesture, Override", "BufferEmpty");
                this.outer.SetNextStateToMain();
                return;
            }

            if (base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }
    }
}