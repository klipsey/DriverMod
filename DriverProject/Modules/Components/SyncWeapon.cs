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

        public SyncWeapon()
        {
        }

        public SyncWeapon(NetworkInstanceId netId, ushort augh, bool ough)
        {
            this.netId = netId;
            this.weapon = augh;
            this.cutAmmo = ough;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.weapon = reader.ReadUInt16();
            this.cutAmmo = reader.ReadBoolean();
        }

        public void OnReceived()
        {
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (!bodyObject) return;

            DriverController iDrive = bodyObject.GetComponent<DriverController>();

            DriverWeaponDef weaponDef = DriverWeaponCatalog.GetWeaponFromIndex(this.weapon);

            float ammo = -1f;
            if (this.cutAmmo) ammo = weaponDef.shotCount * 0.5f;

            if (iDrive) iDrive.PickUpWeapon(weaponDef, ammo);
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.weapon);
        }
    }
}