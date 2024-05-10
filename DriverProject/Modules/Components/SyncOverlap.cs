using UnityEngine.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using UnityEngine;
using RoR2;

namespace RobDriver.Modules.Components
{
    internal class SyncOverlap : INetMessage
    {
        private NetworkInstanceId netId;
        private DamageInfo damageInfo;
        public SyncOverlap()
        {
        }

        public SyncOverlap(NetworkInstanceId netId, DamageInfo damageInfo)
        {
            this.netId = netId;
            this.damageInfo = damageInfo;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.damageInfo = reader.ReadDamageInfo();
        }

        public void OnReceived()
        {
            if(!NetworkServer.active)
            {
                Log.Debug("WHY ARE YOU RUNNING ON CLIENTS???");
                return;
            }
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (bodyObject != null) return;
            CoinController coin = bodyObject.GetComponent<CoinController>();
            coin.RicochetBullet(this.damageInfo);
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.damageInfo);
        }
    }
}