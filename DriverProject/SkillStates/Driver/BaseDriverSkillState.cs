using EntityStates;
using RobDriver.Modules.Components;
using UnityEngine;

namespace RobDriver.SkillStates.Driver
{
    public class BaseDriverSkillState : BaseSkillState
    {
        public virtual void AddRecoil2(float x1, float x2, float y1, float y2)
        {
            if (!Modules.Config.enableRecoil.Value) return;
            this.AddRecoil(x1, x2, y1, y2);
        }

        protected virtual bool hideGun
        {
            get
            {
                return false;
            }
        }
        protected DriverController iDrive;
        protected DriverWeaponDef cachedWeaponDef;
        protected RedGuyController ravController;

        protected virtual string prop
        {
            get
            {
                return "";
            }
        }

        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();

            if (this.hideGun) this.iDrive.weaponRenderer.gameObject.SetActive(false);
            if (this.prop != "") this.GetModelChildLocator().FindChild(this.prop).gameObject.SetActive(true);
        }

        public void RefreshState()
        {
            if (!this.iDrive) this.iDrive = this.GetComponent<DriverController>();
            if (this.iDrive) this.cachedWeaponDef = this.iDrive.weaponDef;
            if (this.iDrive.weaponDef.nameToken == "ROB_DRIVER_WEAPON_RAV_SWORD_NAME" && !this.ravController) this.ravController = this.gameObject.GetComponent<RedGuyController>();
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.hideGun) this.GetModelChildLocator().FindChild("PistolModel").gameObject.SetActive(true);
            if (this.prop != "") this.GetModelChildLocator().FindChild(this.prop).gameObject.SetActive(false);
        }
    }
}