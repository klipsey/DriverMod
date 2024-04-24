using EntityStates;
using RobDriver.Modules.Components;

namespace RobDriver.SkillStates.BaseStates
{
    public class BaseDriverState : BaseState
    {
        protected DriverController iDrive;
        protected DriverWeaponDef cachedWeaponDef;
        protected RedGuyController ravController;

        public override void OnEnter()
        {
            RefreshState();

            base.OnEnter();
        }
        public void RefreshState()
        {
            if (!this.iDrive) this.iDrive = this.GetComponent<DriverController>();
            if (this.iDrive) this.cachedWeaponDef = this.iDrive.weaponDef;
            if (!this.ravController) this.ravController = this.gameObject.GetComponent<RedGuyController>();
        }
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}