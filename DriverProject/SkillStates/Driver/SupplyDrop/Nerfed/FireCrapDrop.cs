using LostInTransit.DamageTypes;
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
                DriverWeaponDef _weaponDef;
                int bulletIndex;

                if (Modules.Config.randomSupplyDrop.Value)
                {
                    _weaponDef = DriverWeaponCatalog.GetRandomWeapon();
                    bulletIndex = Modules.DamageTypes.GetRandomBulletIndexFromTier(DriverWeaponTier.Uncommon);
                }
                else
                {
                    _weaponDef = DriverWeaponCatalog.GetRandomWeaponFromTier(DriverWeaponTier.Uncommon);
                    bulletIndex = Modules.DamageTypes.GetRandomBulletIndexFromTier(Modules.DamageTypes.GetWeightedBulletTier());
                }

                GameObject weaponPickup = UnityEngine.Object.Instantiate<GameObject>(_weaponDef.pickupPrefab, this.dropPosition, UnityEngine.Random.rotation);

                weaponPickup.GetComponentInChildren<Modules.Components.WeaponPickup>().cutAmmo = true;
                weaponPickup.GetComponentInChildren<Modules.Components.WeaponPickup>().ammoIndex = bulletIndex;
                weaponPickup.GetComponentInChildren<Modules.Components.WeaponPickup>().isNewAmmoType = false;

                TeamFilter teamFilter = weaponPickup.GetComponent<TeamFilter>();
                if (teamFilter) teamFilter.teamIndex = this.teamComponent.teamIndex;

                NetworkServer.Spawn(weaponPickup);
            }
        }
    }
}