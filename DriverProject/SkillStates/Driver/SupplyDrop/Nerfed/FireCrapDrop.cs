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
                int bulletDefIndex;

                if (Modules.Config.randomSupplyDrop.Value)
                {
                    _weaponDef = DriverWeaponCatalog.GetRandomWeapon();
                    bulletDefIndex = BulletTypes.GetWeightedRandomBullet(DriverWeaponTier.Legendary).index;

                }
                else
                {
                    _weaponDef = DriverWeaponCatalog.GetRandomWeaponFromTier(DriverWeaponTier.Uncommon);
                    bulletDefIndex = BulletTypes.GetWeightedRandomBullet(DriverWeaponTier.Uncommon).index;
                }

                GameObject weaponPickup = UnityEngine.Object.Instantiate<GameObject>(_weaponDef.pickupPrefab, this.dropPosition, UnityEngine.Random.rotation);

                var weaponComponent = weaponPickup.GetComponentInChildren<Modules.Components.WeaponPickup>();
                weaponComponent.cutAmmo = true;
                weaponComponent.bulletIndex = bulletDefIndex;
                weaponComponent.isNewAmmoType = false;

                TeamFilter teamFilter = weaponPickup.GetComponent<TeamFilter>();
                if (teamFilter) teamFilter.teamIndex = this.teamComponent.teamIndex;

                NetworkServer.Spawn(weaponPickup);
            }
        }
    }
}