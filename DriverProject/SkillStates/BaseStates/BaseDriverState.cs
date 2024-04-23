using EntityStates;
using RobDriver.Modules.Components;

namespace RobDriver.SkillStates.BaseStates
{
    public class BaseDriverState : BaseState
    {
        protected DriverController iDrive;
        protected DriverWeaponDef cachedWeaponDef;
        protected RedGuyController penis;

        public override void OnEnter()
        {
            RefreshState();

            base.OnEnter();
        }
        public void RefreshState()
        {
            if (!this.penis) this.penis = this.gameObject.GetComponent<RedGuyController>();
            if (!this.iDrive) this.iDrive = this.GetComponent<DriverController>();
            if (this.iDrive) this.cachedWeaponDef = this.iDrive.weaponDef;
        }
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}