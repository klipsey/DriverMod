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

        public SyncWeaponPickup()
        {
        }

        public SyncWeaponPickup(NetworkInstanceId netId, ushort augh)
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
            GameObject pickupObject = Util.FindNetworkObject(this.netId);
            if (!pickupObject) return;

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
            }

            WeaponPickup pickupComponent = pickupObject.GetComponentInChildren<WeaponPickup>();
            if (pickupComponent)
            {
                // w h y
                //pickupComponent.SetWeapon(newWeapon);
            }
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.weapon);
        }
    }
}