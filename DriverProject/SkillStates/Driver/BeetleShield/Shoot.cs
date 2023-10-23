using UnityEngine;

namespace RobDriver.SkillStates.Driver.BeetleShield
{
    public class Shoot : Driver.Shoot
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (this.iDrive) this.iDrive.StartTimer();
        }
    }
}