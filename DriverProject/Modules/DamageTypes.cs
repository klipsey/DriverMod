using R2API;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace RobDriver.Modules
{
    public static class DamageTypes
    {
        //For later modded damagetypes
        public static DamageAPI.ModdedDamageType Empty;
        public static DamageAPI.ModdedDamageType HookShot;
        internal static void Init()
        {
            Empty = DamageAPI.ReserveDamageType();
            HookShot = DamageAPI.ReserveDamageType();
            Hook();
        }
        private static void Hook()
        {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            On.RoR2.GlobalEventManager.OnHitEnemy += new On.RoR2.GlobalEventManager.hook_OnHitEnemy(GlobalEventManager_OnHitEnemy);
        }
        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig.Invoke(self, damageInfo, victim);
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            if (NetworkServer.active)
            {
                if(attackerBody.HasBuff())
            }
        }

        private static void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            DamageInfo damageInfo = damageReport.damageInfo;
            if (!damageReport.attackerBody || !damageReport.victimBody)
            {
                return;
            }
            HealthComponent victim = damageReport.victim;
            GameObject inflictorObject = damageInfo.inflictor;
            CharacterBody victimBody = damageReport.victimBody;
            CharacterBody attackerBody = damageReport.attackerBody;
            GameObject attackerObject = damageReport.attacker.gameObject;
            if (NetworkServer.active)
            {
            }
        }
    }
}