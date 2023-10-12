using EntityStates;
using RobDriver.Modules.Components;

namespace RobDriver.SkillStates.Driver
{
    public class BaseDriverSkillState : BaseSkillState
    {
        protected DriverController iDrive;
        protected DriverWeaponDef cachedWeaponDef;

        public override void OnEnter()
        {
            this.iDrive = this.GetComponent<DriverController>();
            if (this.iDrive) this.cachedWeaponDef = this.iDrive.weaponDef;
            base.OnEnter();
        }
    }
}