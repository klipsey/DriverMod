using EntityStates;
using RobDriver.Modules.Components;

namespace RobDriver.SkillStates.Driver
{
    public class BaseDriverSkillState : BaseSkillState
    {
        protected virtual bool hideGun
        {
            get
            {
                return false;
            }
        }
        protected DriverController iDrive;
        protected DriverWeaponDef cachedWeaponDef;
        protected virtual string prop
        {
            get
            {
                return "";
            }
        }

        public override void OnEnter()
        {
            this.iDrive = this.GetComponent<DriverController>();
            if (this.iDrive) this.cachedWeaponDef = this.iDrive.weaponDef;
            base.OnEnter();

            if (this.hideGun) this.GetModelChildLocator().FindChild("PistolModel").gameObject.SetActive(false);
            if (this.prop != "") this.GetModelChildLocator().FindChild(this.prop).gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.hideGun) this.GetModelChildLocator().FindChild("PistolModel").gameObject.SetActive(true);
            if (this.prop != "") this.GetModelChildLocator().FindChild(this.prop).gameObject.SetActive(false);
        }
    }
}