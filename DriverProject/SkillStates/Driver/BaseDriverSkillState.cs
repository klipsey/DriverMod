using EntityStates;
using RobDriver.Modules.Components;

namespace RobDriver.SkillStates.Driver
{
    public class BaseDriverSkillState : BaseSkillState
    {
        protected DriverController iDrive;

        public override void OnEnter()
        {
            this.iDrive = this.GetComponent<DriverController>();
            base.OnEnter();
        }
    }
}