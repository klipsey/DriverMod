using UnityEngine;
using UnityEngine.UI;
using RoR2;
using RoR2.UI;
using TMPro;

namespace RobDriver.Modules.Components
{
    public class WeaponIcon : MonoBehaviour
    {
		public HUD targetHUD;

		public float weaponTimer = 0;
		public float maxWeaponTimer = 0;
		public bool isPistolOnly = false;

		public GameObject displayRoot;
		public RawImage iconImage;

		public GameObject flashPanelObject;
		public GameObject reminderFlashPanelObject;
		public GameObject isReadyPanelObject;
		public TooltipProvider tooltipProvider;
		public GameObject durationDisplay;
		public Image durationBar;
		public Image durationBarRed;

		private void Start()
		{
            var iDrive = this.targetHUD?.targetBodyObject?.GetComponent<DriverController>();
			if (iDrive)
            {
				isPistolOnly = iDrive.passive.isPistolOnly;
                iDrive.onWeaponUpdate += SetDisplay;
			}
        }

        private void OnDestroy()
        {
            var iDrive = this.targetHUD?.targetBodyObject?.GetComponent<DriverController>();
            if (iDrive) iDrive.onWeaponUpdate -= SetDisplay;
        }

        private void Update()
        {
			if (isPistolOnly) return;

            if (this.maxWeaponTimer > 0f)
            {
                this.durationDisplay.SetActive(true);

                float fill = Util.Remap(this.weaponTimer, 0f, this.maxWeaponTimer, 0f, 1f);

                if (this.durationBarRed)
                {
                    if (fill >= 1f) this.durationBarRed.fillAmount = 1f;
                    this.durationBarRed.fillAmount = Mathf.Lerp(this.durationBarRed.fillAmount, fill, Time.deltaTime * 2f);
                }

                this.durationBar.fillAmount = fill;
            }
            else
            {
                this.durationDisplay.SetActive(false);
            }
        }

        private void SetDisplay(DriverController iDrive)
		{
			if (!iDrive) return;
			
			maxWeaponTimer = iDrive.maxWeaponTimer;
			weaponTimer = iDrive.weaponTimer;

			this.DoStockFlash();

            if (this.displayRoot) this.displayRoot.SetActive(true); 
            if (this.isReadyPanelObject) this.isReadyPanelObject.SetActive(true);

            if (this.iconImage)
			{
				this.iconImage.texture = iDrive.weaponDef.icon;
				this.iconImage.color = Color.white;
				this.iconImage.enabled = true;
			}
            if (this.tooltipProvider)
			{
				this.tooltipProvider.titleToken = iDrive.weaponDef.nameToken;
				this.tooltipProvider.bodyToken = iDrive.weaponDef.descriptionToken;
				this.tooltipProvider.titleColor = Modules.Survivors.Driver.characterColor;
				this.tooltipProvider.bodyColor = Color.gray;
			}
        }

        private void DoReminderFlash()
		{
			if (this.reminderFlashPanelObject)
			{
				AnimateUIAlpha animateUI = this.reminderFlashPanelObject.GetComponent<AnimateUIAlpha>();
				if (animateUI) animateUI.time = 0f;
				this.reminderFlashPanelObject.SetActive(true);
			}
		}

		private void DoStockFlash()
		{
			this.DoReminderFlash();
			if (this.flashPanelObject)
			{
				AnimateUIAlpha animateUI = this.flashPanelObject.GetComponent<AnimateUIAlpha>();
				if (animateUI) animateUI.time = 0f;
				this.flashPanelObject.SetActive(true);
			}
		}
	}
}