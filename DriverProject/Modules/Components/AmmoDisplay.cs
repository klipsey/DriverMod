using UnityEngine;
using RoR2;
using RoR2.UI;
using UnityEngine.Networking;

namespace RobDriver.Modules.Components
{
    public class AmmoDisplay : MonoBehaviour
    {
        public HUD targetHUD;
        public LanguageTextMeshController targetText;
        
        private void Start()
        {
            var driver = this.targetHUD?.targetBodyObject?.GetComponent<DriverController>();
            if (driver) driver.onWeaponUpdate += SetDisplay;
            this.targetText.enabled = true;
        }

        private void OnDestroy()
        {
            var driver = this.targetHUD?.targetBodyObject?.GetComponent<DriverController>();
            if (driver) driver.onWeaponUpdate -= SetDisplay;
            this.targetText.enabled = false;
        }

        private void SetDisplay(DriverController iDrive)
        {
            if (this.targetText)
            {
                if (iDrive.maxWeaponTimer <= 0f || iDrive.passive.isDefault)
                {
                    this.targetText.token = "";
                    return;
                }

                if (iDrive.weaponTimer <= 0f)
                {
                    this.targetText.token = "<color=#C80000>0 / " + Mathf.CeilToInt(iDrive.maxWeaponTimer).ToString() + Helpers.colorSuffix;
                    return;
                }

                if (iDrive.passive.isPistolOnly)
                {
                    this.targetText.token = Mathf.CeilToInt(iDrive.weaponTimer).ToString() + " / " + Mathf.CeilToInt(iDrive.maxWeaponTimer).ToString();
                }
                else if (iDrive.HasSpecialBullets)
                {
                    this.targetText.token = $"<color=#{ColorUtility.ToHtmlStringRGBA(iDrive.currentBulletDef.trailColor)}>" +
                        Mathf.CeilToInt(iDrive.weaponTimer).ToString() + " / " + Mathf.CeilToInt(iDrive.maxWeaponTimer).ToString() +
                        " - " + iDrive.currentBulletDef.nameToken + Helpers.colorSuffix;
                }
                else
                {
                    this.targetText.token = "";
                }
            }
        }
    }
}