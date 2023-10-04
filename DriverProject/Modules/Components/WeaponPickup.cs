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
        }

		private void OnTriggerStay(Collider collider)
		{
			if (NetworkServer.active && this.alive && TeamComponent.GetObjectTeam(collider.gameObject) == this.teamFilter.teamIndex)
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