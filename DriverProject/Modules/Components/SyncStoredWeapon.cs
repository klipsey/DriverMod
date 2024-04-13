using UnityEngine.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using UnityEngine;
using RoR2;

namespace RobDriver.Modules.Components
{
    internal class SyncStoredWeapon : INetMessage
    {
        private NetworkInstanceId netId;
        private ushort weapon;
        private ushort bullet;
        private long ammo;

        public SyncStoredWeapon()
        {
        }

        public SyncStoredWeapon(NetworkInstanceId netId, ushort weapon, ushort bullet, float ammo)
        {
            this.netId = netId;
            this.weapon = weapon;
            this.bullet = bullet;
            this.ammo = Mathf.CeilToInt(ammo * 100f);
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.weapon = reader.ReadUInt16();
            this.bullet = reader.ReadUInt16();
            this.ammo = reader.ReadInt64();
        }

        public void OnReceived()
        {
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (!bodyObject) return;

            DriverController iDrive = bodyObject.GetComponent<DriverController>();
            if (iDrive)
            {
                iDrive.PickUpWeapon(DriverWeaponCatalog.GetWeaponFromIndex(this.weapon), BulletTypes.GetBulletFromIndex(this.bullet), this.ammo * 0.01f);
            }
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.weapon);
            writer.Write(this.bullet);
            writer.Write(this.ammo);
        }
    }
}