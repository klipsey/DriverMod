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
        private bool isAmmoBox;

        public SyncWeapon()
        {
        }

        public SyncWeapon(NetworkInstanceId netId, ushort augh, bool ough, bool isAmmoBox)
        {
            this.netId = netId;
            this.weapon = augh;
            this.cutAmmo = ough;
            this.isAmmoBox = isAmmoBox;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.weapon = reader.ReadUInt16();
            this.cutAmmo = reader.ReadBoolean();
            this.isAmmoBox = reader.ReadBoolean();
        }

        public void OnReceived()
        {
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (!bodyObject) return;

            DriverController iDrive = bodyObject.GetComponent<DriverController>();

            if (iDrive)
            {
                DriverWeaponDef weaponDef = DriverWeaponCatalog.GetWeaponFromIndex(this.weapon);

                float ammo = -1f;
                if (this.cutAmmo) ammo = weaponDef.shotCount * 0.5f;

                // fuck thissssss
                bool isAmmoBox = iDrive.passive.isBullets || this.isAmmoBox;
                iDrive.PickUpWeapon(weaponDef, ammo, isAmmoBox);
            }
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.weapon);
            writer.Write(this.cutAmmo);
            writer.Write(this.isAmmoBox);
        }
    }
}