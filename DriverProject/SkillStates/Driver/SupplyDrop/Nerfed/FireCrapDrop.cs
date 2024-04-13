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
                DriverWeaponDef _weaponDef = DriverWeaponCatalog.GetRandomWeaponFromTier(DriverWeaponTier.Uncommon);
                DriverBulletDef _bulletDef = BulletTypes.GetRandomBulletFromTier(DriverWeaponTier.Uncommon);
                if (Modules.Config.randomSupplyDrop.Value) _weaponDef = DriverWeaponCatalog.GetRandomWeapon();
                _bulletDef = BulletTypes.GetRandomBullet();

                GameObject weaponPickup = UnityEngine.Object.Instantiate<GameObject>(_weaponDef.pickupPrefab, this.dropPosition, UnityEngine.Random.rotation);

                weaponPickup.GetComponentInChildren<Modules.Components.WeaponPickup>().cutAmmo = true;
                weaponPickup.GetComponentInChildren<Modules.Components.WeaponPickup>().isAmmoBox = iDrive.passive.isBullets;

                TeamFilter teamFilter = weaponPickup.GetComponent<TeamFilter>();
                if (teamFilter) teamFilter.teamIndex = this.teamComponent.teamIndex;

                NetworkServer.Spawn(weaponPickup);
            }
        }
    }
}