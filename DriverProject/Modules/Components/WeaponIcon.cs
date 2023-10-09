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
		public DriverController iDrive;

		public GameObject displayRoot;
		public PlayerCharacterMasterController playerCharacterMasterController;
		public RawImage iconImage;

		public GameObject flashPanelObject;
		public GameObject reminderFlashPanelObject;
		public GameObject isReadyPanelObject;
		public TooltipProvider tooltipProvider;
		public GameObject durationDisplay;
		public Image durationBar;

		private void Update()
        {
			// REWRITE THIS ASAP
			if (!this.iDrive)
            {
				if (!this.playerCharacterMasterController)
                {
					this.playerCharacterMasterController = (this.targetHUD.targetMaster ? this.targetHUD.targetMaster.GetComponent<PlayerCharacterMasterController>() : null);
				}

				if (this.playerCharacterMasterController && this.playerCharacterMasterController.master.hasBody)
                {
					DriverController fuckYou = this.playerCharacterMasterController.master.GetBody().GetComponent<DriverController>();
					if (fuckYou) this.SetTarget(fuckYou);
                }
            }
			else
            {
				this.UpdateDisplay();
            }
        }

		public void SetTarget(DriverController driver)
        {
			this.iDrive = driver;
			this.iDrive.onWeaponUpdate += SetDisplay;
			this.SetDisplay(this.iDrive);
        }

		private void UpdateDisplay()
        {
			if (this.iDrive.weaponTimer > 0f)
            {
				this.durationDisplay.SetActive(true);

				float fill = Util.Remap(this.iDrive.weaponTimer, 0f, this.iDrive.maxWeaponTimer, 0f, 1f);
				this.durationBar.fillAmount = fill;
            }
			else
            {
				this.durationDisplay.SetActive(false);
            }
        }

		private void SetDisplay(DriverController z)
		{
			if (!this.iDrive) return;

			this.DoStockFlash();

			if (this.displayRoot) this.displayRoot.SetActive(true);
			if (this.isReadyPanelObject) this.isReadyPanelObject.SetActive(true);

			if (this.iconImage)
			{
				Texture texture = null;

				switch (this.iDrive.weapon)
                {
					case DriverWeapon.Default:
						texture = Modules.Assets.pistolWeaponIcon;
						break;
					case DriverWeapon.Shotgun:
						texture = Modules.Assets.shotgunWeaponIcon;
						break;
					case DriverWeapon.MachineGun:
						texture = Modules.Assets.machineGunWeaponIcon;
						break;
					case DriverWeapon.RocketLauncher:
						texture = Modules.Assets.machineGunWeaponIcon;
						break;
				}

				this.iconImage.texture = texture;
				this.iconImage.color = Color.white;
				this.iconImage.enabled = true;
			}

			string titleToken = null;
			string bodyToken = null;
			Color titleColor = Color.white;

			switch (this.iDrive.weapon)
            {
				case DriverWeapon.Default:
					titleToken = "ROB_DRIVER_PISTOL_NAME";
					bodyToken = "ROB_DRIVER_PISTOL_DESC";
					titleColor = Modules.Survivors.Driver.characterColor;
					break;
				case DriverWeapon.Shotgun:
					titleToken = "ROB_DRIVER_SHOTGUN_NAME";
					bodyToken = "ROB_DRIVER_SHOTGUN_DESC";
					titleColor = Modules.Survivors.Driver.characterColor;
					break;
				case DriverWeapon.MachineGun:
					titleToken = "ROB_DRIVER_MACHINEGUN_NAME";
					bodyToken = "ROB_DRIVER_MACHINEGUN_DESC";
					titleColor = Modules.Survivors.Driver.characterColor;
					break;
				case DriverWeapon.RocketLauncher:
					titleToken = "ROB_DRIVER_ROCKETLAUNCHER_NAME";
					bodyToken = "ROB_DRIVER_ROCKETLAUNCHER_DESC";
					titleColor = Modules.Survivors.Driver.characterColor;
					break;
			}

			if (this.tooltipProvider)
			{
				this.tooltipProvider.titleToken = titleToken;
				this.tooltipProvider.titleColor = titleColor;
				this.tooltipProvider.bodyToken = bodyToken;
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