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
        private ushort weapon;
        private bool cutAmmo;
        private short ammoIndex;
        private bool isNewAmmoType;

        public SyncWeaponPickup()
        {
        }

        public SyncWeaponPickup(NetworkInstanceId netId, ushort augh, bool ough, short ammoIndex, bool isNewAmmoType)
        {
            this.netId = netId;
            this.weapon = augh;
            this.cutAmmo = ough;
            this.ammoIndex = ammoIndex;
            this.isNewAmmoType = isNewAmmoType;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.weapon = reader.ReadUInt16();
            this.cutAmmo = reader.ReadBoolean();
            this.ammoIndex = reader.ReadInt16();
            this.isNewAmmoType = reader.ReadBoolean();
        }

        public void OnReceived()
        {
            GameObject pickupObject = Util.FindNetworkObject(this.netId);
            if (!pickupObject) return;

            WeaponPickup pickupComponent = pickupObject.GetComponentInChildren<WeaponPickup>();
            if (pickupComponent) pickupComponent.SetWeapon(DriverWeaponCatalog.GetWeaponFromIndex(this.weapon), this.cutAmmo, this.ammoIndex, isNewAmmoType);
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.weapon);
            writer.Write(this.cutAmmo);
            writer.Write(this.ammoIndex);
            writer.Write(this.isNewAmmoType);
        }
    }
}