using UnityEngine.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using UnityEngine;
using RoR2;

namespace RobDriver.Modules.Components
{
    internal class SyncWeapon : INetMessage
    {
        private NetworkInstanceId netId;
        private ushort weapon;
        private bool cutAmmo;
        private short ammoIndex;

        public SyncWeapon()
        {
        }

        public SyncWeapon(NetworkInstanceId netId, ushort augh, bool ough, short ammoIndex)
        {
            this.netId = netId;
            this.weapon = augh;
            this.cutAmmo = ough;
            this.ammoIndex = ammoIndex;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.weapon = reader.ReadUInt16();
            this.cutAmmo = reader.ReadBoolean();
            this.ammoIndex = reader.ReadInt16();
        }

        public void OnReceived()
        {
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (!bodyObject) return;

            DriverController iDrive = bodyObject.GetComponent<DriverController>();
            DriverWeaponDef weaponDef = DriverWeaponCatalog.GetWeaponFromIndex(this.weapon);

            float ammo = -1f;
            if (this.cutAmmo) ammo = weaponDef.shotCount * 0.5f;

            if (iDrive) iDrive.PickUpWeaponDrop(weaponDef, ammo, this.ammoIndex);
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.weapon);
            writer.Write(this.cutAmmo);
            writer.Write(this.ammoIndex);
        }
    }
}