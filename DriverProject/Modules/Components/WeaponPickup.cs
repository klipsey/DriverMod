using UnityEngine;
using RoR2;
using UnityEngine.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;

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

			// uh will this work?
			/*if (Run.instance)
			{
				float rng = Run.instance.stageRng.nextNormalizedFloat;

				if (rng > 0.5f) this.SetWeapon(DriverWeapon.MachineGun);
				else this.SetWeapon(DriverWeapon.Shotgun);
			}*/
			// no it doesn't, clients don't have the rng
		}

		private void Start()
        {
			this.SetWeapon(this.weapon);

			/*if (NetworkServer.active)
			{
				// this is horrible
				// i feel like there's probably a much simpler way to do this?
				// but really all that matters is that it works right
				//
				// god please work.
				ushort fuckMyAssIFuckingHateNetworking = 1;
				if (Random.value > 0.5f) fuckMyAssIFuckingHateNetworking = 2;

				NetworkIdentity identity = this.GetComponentInParent<NetworkIdentity>();
				if (!identity) return;

				new SyncWeaponPickup(identity.netId, fuckMyAssIFuckingHateNetworking).Send(NetworkDestination.Clients);
			}*/
			// no fuck my life bro no way this is real
		}

		public void SetWeapon(DriverWeapon newWeapon)
        {
			this.weapon = newWeapon;

			// wow this is awful!
			RoR2.UI.LanguageTextMeshController textComponent = this.transform.parent.GetComponentInChildren<RoR2.UI.LanguageTextMeshController>();
			if (textComponent)
			{
				if (!NetworkServer.active)
				{
					// band-aid i don't have the time to keep fighting with this code rn
					textComponent.token = "????";
					return;
				}

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
					case DriverWeapon.RocketLauncher:
						textComponent.token = "ROB_DRIVER_ROCKETLAUNCHER_NAME";
						break;
				}
			}
		}

		private void OnTriggerStay(Collider collider)
		{
			// can this run on every client? i don't know but let's find out
			if (NetworkServer.active && this.alive && TeamComponent.GetObjectTeam(collider.gameObject) == this.teamFilter.teamIndex)
			{
				// well it can but it's not a fix.
				DriverController iDrive = collider.GetComponent<DriverController>();
				if (iDrive)
				{
					this.alive = false;
					iDrive.ServerPickUpWeapon(this.weapon, iDrive);
					EffectManager.SimpleEffect(this.pickupEffect, this.transform.position, Quaternion.identity, true);
					UnityEngine.Object.Destroy(this.baseObject);
				}
			}
		}
	}
}