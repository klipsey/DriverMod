using UnityEngine.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using UnityEngine;
using RoR2;

namespace RobDriver.Modules.Components
{
    internal class SyncWeaponPickup : INetMessage
    {
        private NetworkInstanceId netId;
        private ushort weaponIndex;
        private ushort bulletIndex;
        private bool cutAmmo;
        private bool isNewAmmoType;

        public SyncWeaponPickup()
        {
        }

        public SyncWeaponPickup(NetworkInstanceId netId, ushort augh, ushort ough, bool cutAmmo, bool isNewAmmoType)
        {
            this.netId = netId;
            this.weaponIndex = augh;
            this.bulletIndex = ough;
            this.cutAmmo = cutAmmo;
            this.isNewAmmoType = isNewAmmoType;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.weaponIndex = reader.ReadUInt16();
            this.bulletIndex = reader.ReadUInt16();
            this.cutAmmo = reader.ReadBoolean();
            this.isNewAmmoType = reader.ReadBoolean();
        }

        public void OnReceived()
        {
            GameObject pickupObject = Util.FindNetworkObject(this.netId);
            if (!pickupObject) return;

            WeaponPickup pickupComponent = pickupObject.GetComponentInChildren<WeaponPickup>();
            DriverWeaponDef weaponDef = DriverWeaponCatalog.GetWeaponFromIndex(this.weaponIndex);
            DriverBulletDef bulletDef = DriverBulletCatalog.GetBulletDefFromIndex(this.bulletIndex);

            if (pickupComponent) pickupComponent.SetWeapon(weaponDef, bulletDef, this.cutAmmo, isNewAmmoType);
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.weaponIndex);
            writer.Write(this.bulletIndex);
            writer.Write(this.cutAmmo);
            writer.Write(this.isNewAmmoType);
        }
    }
}