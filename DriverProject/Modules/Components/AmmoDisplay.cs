using UnityEngine;
using UnityEngine.UI;
using RoR2;
using RoR2.UI;
using UnityEngine.Networking;

namespace RobDriver.Modules.Components
{
    public class AmmoDisplay : MonoBehaviour
    {
        public HUD targetHUD;

        public float weaponTimer = 0;
        public float maxWeaponTimer = 0;

        public LanguageTextMeshController targetText;
        public GameObject durationDisplay;
        public Image durationBar;
        public Image durationBarRed;

        private void Start()
        {
            var driver = this.targetHUD?.targetBodyObject?.GetComponent<DriverController>();
            if (driver) driver.onConsumeAmmo += SetDisplay;
            this.targetText.enabled = true;
        }

        private void OnDestroy()
        {
            var driver = this.targetHUD?.targetBodyObject?.GetComponent<DriverController>();
            if (driver) driver.onConsumeAmmo -= SetDisplay;
        }

        private void Update()
        {
            if (this.maxWeaponTimer > 0f)
            {
                float fill = Util.Remap(this.weaponTimer, 0f, this.maxWeaponTimer, 0f, 1f);

                if (this.durationBarRed)
                {
                    if (fill >= 1f) this.durationBarRed.fillAmount = 1f;
                    this.durationBarRed.fillAmount = Mathf.Lerp(this.durationBarRed.fillAmount, fill, Time.deltaTime * 2f);
                }

                this.durationBar.fillAmount = fill;
            }
        }

        private void SetDisplay(DriverController iDrive)
        {
            if (this.targetText)
            {
                if (iDrive.maxWeaponTimer <= 0f || iDrive.passive.isDefault)
                {
                    this.targetText.token = "";
                    this.durationDisplay.SetActive(false);
                    return;
                }

                if (iDrive.passive.isPistolOnly)
                {
                    if (iDrive.weaponTimer <= 0f)
                    {
                        this.targetText.token = "<color=#C80000>0 / " + Mathf.CeilToInt(iDrive.maxWeaponTimer).ToString() + Helpers.colorSuffix;
                        return;
                    }
                    this.targetText.token = Mathf.CeilToInt(iDrive.weaponTimer).ToString() + " / " + Mathf.CeilToInt(iDrive.maxWeaponTimer).ToString();
                }
                else if (iDrive.HasSpecialBullets)
                {
                    if (iDrive.weaponTimer <= 0f)
                    {
                        this.targetText.token = "";
                        durationBar.color = durationBarRed.color;
                        return;
                    }
                    this.durationDisplay.SetActive(true);

                    maxWeaponTimer = iDrive.maxWeaponTimer;
                    weaponTimer = iDrive.weaponTimer;

                    durationBar.color = iDrive.currentBulletDef.trailColor;
                    this.targetText.token = $"<color=#{ColorUtility.ToHtmlStringRGBA(iDrive.currentBulletDef.trailColor)}>"+ iDrive.currentBulletDef.nameToken + Helpers.colorSuffix;
                }
                else
                {
                    this.targetText.token = "";
                }
            }
        }
    }
}