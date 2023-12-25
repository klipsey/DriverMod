using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace RobDriver.SkillStates.Driver.SupplyDrop.Nerfed
{
    public class FireCrapDrop : FireSupplyDrop
    {
        protected override void SpawnWeapon()
        {
            if (NetworkServer.active)
            {
                DriverWeaponDef _weaponDef = DriverWeaponCatalog.GetRandomWeaponFromTier(DriverWeaponTier.Uncommon);

                if (Modules.Config.randomSupplyDrop.Value) _weaponDef = DriverWeaponCatalog.GetRandomWeapon();

                GameObject weaponPickup = UnityEngine.Object.Instantiate<GameObject>(_weaponDef.pickupPrefab, this.dropPosition, UnityEngine.Random.rotation);

                TeamFilter teamFilter = weaponPickup.GetComponent<TeamFilter>();
                if (teamFilter) teamFilter.teamIndex = this.teamComponent.teamIndex;

                NetworkServer.Spawn(weaponPickup);
            }
        }
    }
}