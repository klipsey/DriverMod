using UnityEngine;
using RoR2;
using RoR2.UI;

namespace RobDriver.Modules.Components
{
    public class AmmoDisplay : MonoBehaviour
    {
        public HUD targetHUD;
        public LanguageTextMeshController targetText;

        private DriverController iDrive;

        private void FixedUpdate()
        {
            if (this.targetHUD)
            {
                if (this.targetHUD.targetBodyObject)
                {
                    if (!this.iDrive) this.iDrive = this.targetHUD.targetBodyObject.GetComponent<DriverController>();
                }
            }

            if (this.targetText)
            {
                if (this.iDrive)
                {
                    if (this.iDrive.maxWeaponTimer <= 0f)
                    {
                        this.targetText.token = "";
                        return;
                    }

                    if (this.iDrive.weaponTimer <= 0f) this.targetText.token = "<color=#C80000>0 / " + Mathf.CeilToInt(this.iDrive.maxWeaponTimer).ToString() + Helpers.colorSuffix;
                    else this.targetText.token = Mathf.CeilToInt(this.iDrive.weaponTimer).ToString() + " / " + Mathf.CeilToInt(this.iDrive.maxWeaponTimer).ToString();
                }
                else
                {
                    this.targetText.token = "";
                }
            }
        }
    }
}