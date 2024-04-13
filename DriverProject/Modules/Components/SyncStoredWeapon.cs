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
        private long ammo;
        private short ammoIndex;

        public SyncStoredWeapon()
        {
        }

        public SyncStoredWeapon(NetworkInstanceId netId, ushort augh, float ammo, short ammoIndex)
        {
            this.netId = netId;
            this.weapon = augh;
            this.ammo = Mathf.CeilToInt(ammo * 100f);
            this.ammoIndex = ammoIndex;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.weapon = reader.ReadUInt16();
            this.ammo = reader.ReadInt64();
            this.ammoIndex = reader.ReadInt16();
        }

        public void OnReceived()
        {
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (!bodyObject) return;

            DriverController iDrive = bodyObject.GetComponent<DriverController>();
            if (iDrive)
            {
                // yes, this is a dumb way to do it
                iDrive.PickUpWeaponDrop(DriverWeaponCatalog.GetWeaponFromIndex(this.weapon), this.ammo * 0.01f, ammoIndex, false);
                iDrive.PickUpWeaponDrop(DriverWeaponCatalog.GetWeaponFromIndex(this.weapon), this.ammo * 0.01f, ammoIndex, true);
            }
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.weapon);
            writer.Write(this.ammo);
            writer.Write(this.ammoIndex);
        }
    }
}