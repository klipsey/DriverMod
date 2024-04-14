using LostInTransit.DamageTypes;
using RobDriver.Modules;
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
                int _bulletDefIndex;

                if (Modules.Config.randomSupplyDrop.Value)
                {
                    _weaponDef = DriverWeaponCatalog.GetRandomWeapon();
                    _bulletDefIndex = BulletTypes.GetRandomIndexFromTier(DriverWeaponTier.Legendary);

                }
                else
                {
                    _weaponDef = DriverWeaponCatalog.GetRandomWeaponFromTier(DriverWeaponTier.Uncommon);
                    _bulletDefIndex = BulletTypes.GetRandomIndexFromTier(DriverWeaponTier.Uncommon);
                }

                GameObject weaponPickup = UnityEngine.Object.Instantiate<GameObject>(_weaponDef.pickupPrefab, this.dropPosition, UnityEngine.Random.rotation);

                weaponPickup.GetComponentInChildren<Modules.Components.WeaponPickup>().cutAmmo = true;
                weaponPickup.GetComponentInChildren<Modules.Components.WeaponPickup>().bulletIndex = _bulletDefIndex;
                weaponPickup.GetComponentInChildren<Modules.Components.WeaponPickup>().isNewAmmoType = false;

                TeamFilter teamFilter = weaponPickup.GetComponent<TeamFilter>();
                if (teamFilter) teamFilter.teamIndex = this.teamComponent.teamIndex;

                NetworkServer.Spawn(weaponPickup);
            }
        }
    }
}