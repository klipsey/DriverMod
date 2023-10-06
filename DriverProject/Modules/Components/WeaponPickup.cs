using UnityEngine;
using RoR2;
using UnityEngine.Networking;

namespace RobDriver.Modules.Components
{
    public class WeaponPickup : MonoBehaviour
    {
		[Tooltip("The base object to destroy when this pickup is consumed.")]
		public GameObject baseObject;
		[Tooltip("The team filter object which determines who can pick up this pack.")]
		public TeamFilter teamFilter;
		public DriverWeapon weapon = DriverWeapon.Shotgun;
		public GameObject pickupEffect;

		private bool alive = true;

		private void Awake()
        {
			if (Random.value > 0.5f) this.weapon = DriverWeapon.Shotgun;
			else this.weapon = DriverWeapon.MachineGun;

			// wow this is awful!
			RoR2.UI.LanguageTextMeshController textComponent = this.transform.parent.GetComponentInChildren<RoR2.UI.LanguageTextMeshController>();
			if (textComponent)
            {
				switch (this.weapon)
                {
					case DriverWeapon.Default:
						textComponent.token = "ROB_DRIVER_PISTOL_NAME";
						break;
					case DriverWeapon.Shotgun:
						textComponent.token = "ROB_DRIVER_SHOTGUN_NAME";
						break;
					case DriverWeapon.MachineGun:
						textComponent.token = "ROB_DRIVER_MACHINEGUN_NAME";
						break;
				}
            }

			// disable visuals for non driver
			if (!Modules.Config.sharedPickupVisuals.Value)
            {
				BeginRapidlyActivatingAndDeactivating blinker = this.transform.parent.GetComponentInChildren<BeginRapidlyActivatingAndDeactivating>();
				if (blinker)
				{
					bool isDriver = false;

					var localPlayers = LocalUserManager.readOnlyLocalUsersList;
					foreach (LocalUser i in localPlayers)
					{
						if (i.cachedBody.baseNameToken == Modules.Survivors.Driver.bodyNameToken) isDriver = true;
					}

					if (!isDriver)
					{
						blinker.blinkingRootObject.SetActive(false);
						Destroy(blinker);
					}
				}
			}
		}

		private void OnTriggerStay(Collider collider)
		{
			// can this run on every client? i don't know but let's find out
			if (/*NetworkServer.active && */this.alive && TeamComponent.GetObjectTeam(collider.gameObject) == this.teamFilter.teamIndex)
			{
				DriverController iDrive = collider.GetComponent<DriverController>();
				if (iDrive)
				{
					this.alive = false;
					iDrive.PickUpWeapon(this.weapon);
					EffectManager.SimpleEffect(this.pickupEffect, this.transform.position, Quaternion.identity, true);
					UnityEngine.Object.Destroy(this.baseObject);
				}
			}
		}
	}
}