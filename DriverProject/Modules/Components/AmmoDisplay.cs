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
        public DriverController iDrive;

        public LanguageTextMeshController targetText;
        public GameObject durationDisplay;
        public Image durationBar;
        public Image durationBarRed;

        private void Start()
        {
            this.iDrive = this.targetHUD?.targetBodyObject?.GetComponent<DriverController>();
            if (this.iDrive && !this.iDrive.passive.isDefault)
            {
                this.iDrive.onConsumeAmmo += SetDisplay;
                this.targetText.enabled = true;
                this.durationDisplay.SetActive(true);
            }
            else
            {
                this.targetText.enabled = false;
                this.durationDisplay.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (this.iDrive) this.iDrive.onConsumeAmmo -= SetDisplay;
        }

        private void Update()
        {
            if (this.iDrive && this.iDrive.maxWeaponTimer > 0f)
            {
                float fill = Util.Remap(this.iDrive.weaponTimer, 0f, this.iDrive.maxWeaponTimer, 0f, 1f);

                if (this.durationBarRed)
                {
                    if (fill >= 1f) this.durationBarRed.fillAmount = 1f;
                    this.durationBarRed.fillAmount = Mathf.Lerp(this.durationBarRed.fillAmount, fill, Time.deltaTime * 2f);
                }

                this.durationBar.fillAmount = fill;
            }
        }

        private void SetDisplay()
        {
            if (this.targetText && this.iDrive)
            {
                if (this.iDrive.maxWeaponTimer <= 0f)
                {
                    this.targetText.token = "";
                    this.durationDisplay.SetActive(false);
                    return;
                }

                if (this.iDrive.passive.isPistolOnly)
                {
                    if (this.iDrive.weaponTimer <= 0f)
                    {
                        this.targetText.token = "<color=#C80000>0 / " + Mathf.CeilToInt(this.iDrive.maxWeaponTimer).ToString() + Helpers.colorSuffix;
                        return;
                    }
                    this.targetText.token = Mathf.CeilToInt(this.iDrive.weaponTimer).ToString() + " / " + Mathf.CeilToInt(this.iDrive.maxWeaponTimer).ToString();
                }
                else if (this.iDrive.HasSpecialBullets)
                {
                    if (this.iDrive.weaponTimer <= 0f)
                    {
                        this.targetText.token = "";
                        durationBar.color = durationBarRed.color;
                        return;
                    }
                    this.durationDisplay.SetActive(true);

                    durationBar.color = this.iDrive.currentBulletDef.trailColor;
                    this.targetText.token = $"<color=#{ColorUtility.ToHtmlStringRGBA(this.iDrive.currentBulletDef.trailColor)}>" + this.iDrive.currentBulletDef.nameToken + Helpers.colorSuffix;
                }
                else
                {
                    this.targetText.token = "";
                }
            }
        }
    }
}