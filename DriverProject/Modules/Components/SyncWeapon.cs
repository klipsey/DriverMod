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

        public SyncWeapon()
        {
        }

        public SyncWeapon(NetworkInstanceId netId, ushort augh)
        {
            this.netId = netId;
            this.weapon = augh;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.weapon = reader.ReadUInt16();
        }

        public void OnReceived()
        {
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (!bodyObject) return;

            DriverWeapon newWeapon = DriverWeapon.Default;
            switch (this.weapon)
            {
                case 0:
                    newWeapon = DriverWeapon.Default;
                    break;
                case 1:
                    newWeapon = DriverWeapon.Shotgun;
                    break;
                case 2:
                    newWeapon = DriverWeapon.MachineGun;
                    break;
                case 3:
                    newWeapon = DriverWeapon.RocketLauncher;
                    break;
            }

            DriverController iDrive = bodyObject.GetComponent<DriverController>();
            if (iDrive) iDrive.PickUpWeapon(newWeapon);
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.weapon);
        }
    }
}