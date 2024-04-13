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
        private ushort bullet;
        private bool cutAmmo;

        public SyncWeaponPickup()
        {
        }

        public SyncWeaponPickup(NetworkInstanceId netId, ushort weapon, ushort bullet, bool cutAmmo)
        {
            this.netId = netId;
            this.weapon = weapon;
            this.bullet = bullet;
            this.cutAmmo = cutAmmo;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.weapon = reader.ReadUInt16();
            this.bullet = reader.ReadUInt16();  
            this.cutAmmo = reader.ReadBoolean();
        }

        public void OnReceived()
        {
            GameObject pickupObject = Util.FindNetworkObject(this.netId);
            if (!pickupObject) return;

            WeaponPickup pickupComponent = pickupObject.GetComponentInChildren<WeaponPickup>();
            if (pickupComponent) pickupComponent.SetWeapon(DriverWeaponCatalog.GetWeaponFromIndex(this.weapon), BulletTypes.GetBulletFromIndex(this.bullet), this.cutAmmo);
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.weapon);
            writer.Write(this.bullet);
            writer.Write(this.cutAmmo);
        }
    }
}