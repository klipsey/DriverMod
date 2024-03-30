using R2API;
using RoR2.Orbs;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using RobDriver.Modules.Components;
using RobDriver.Modules.Survivors;
using System.Collections.Generic;
using System.Linq;
using System;
using HG;

namespace RobDriver.Modules
{
    public static class DamageTypes
    {
        public static List<DriverBulletInfo> bulletTypes = new List<DriverBulletInfo>();

        internal static DamageType[] allowedDamageTypes = new DamageType[]
        {
            DamageType.ResetCooldownsOnKill,
            DamageType.SlowOnHit,
            DamageType.Stun1s,
            DamageType.IgniteOnHit,
            DamageType.Freeze2s,
            DamageType.ClayGoo,
            DamageType.BleedOnHit,
            DamageType.PoisonOnHit,
            DamageType.WeakOnHit,
            DamageType.Nullify,
            DamageType.BonusToLowHealth,
            DamageType.BlightOnHit,
            DamageType.CrippleOnHit,
            DamageType.SuperBleedOnCrit,
            DamageType.FruitOnHit
        };

        public static DamageAPI.ModdedDamageType Generic;
        public static DamageAPI.ModdedDamageType HookShot;


        public struct DriverBulletInfo
        {
            public string nameToken;
            public DamageType bulletType;
            public DamageAPI.ModdedDamageType moddedBulletType;
            public Sprite icon;
            public Color trailColor;
        }

        internal static void Init()
        {
            Generic = DamageAPI.ReserveDamageType();
            HookShot = DamageAPI.ReserveDamageType();
            InitializeBullets();
            Hook();
        }

        private static void Hook()
        {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            On.RoR2.GlobalEventManager.OnHitEnemy += new On.RoR2.GlobalEventManager.hook_OnHitEnemy(GlobalEventManager_OnHitEnemy);
        }
        public static void InitializeBullets()
        {
            foreach (DamageType i in allowedDamageTypes)
            {
                string name = Enum.GetName(typeof(DamageType), i);
                //Renaming
                if (name == "ResetCooldownsOnKill")
                {
                    DamageTypes.AddNewBullet("Resetting Rounds", i, Color.red);
                }
                if (name == "SlowOnHit")
                {
                    DamageTypes.AddNewBullet("Slowing Rounds", i, UnityEngine.Color.yellow);
                }
                if (name == "Stun1s")
                {
                    DamageTypes.AddNewBullet("Stunning Rounds", i, UnityEngine.Color.gray);
                }
                if (name == "IgniteOnHit")
                {
                    DamageTypes.AddNewBullet("Incendiary Rounds", i, new UnityEngine.Color(255f / 255f, 127f / 255f, 80 / 255f));
                }
                if (name == "Freeze2s")
                {
                    DamageTypes.AddNewBullet("Frostbite Rounds", i, UnityEngine.Color.cyan);
                }
                if (name == "ClayGoo")
                {
                    DamageTypes.AddNewBullet("Goo Rounds", i, UnityEngine.Color.black);
                }
                if (name == "BleedOnHit")
                {
                    DamageTypes.AddNewBullet("Serrated Rounds", i, DamageColor.FindColor(DamageColorIndex.Bleed));
                }
                if (name == "PoisonOnHit")
                {
                    DamageTypes.AddNewBullet("Poison Rounds", i, UnityEngine.Color.green);
                }
                if (name == "WeakOnHit")
                {
                    DamageTypes.AddNewBullet("Weakening Rounds", i, new UnityEngine.Color(220f / 255f, 237f / 255f, 159f / 255f));
                }
                if (name == "Nullify")
                {
                    DamageTypes.AddNewBullet("Nullifying Rounds", i, DamageColor.FindColor(DamageColorIndex.Void));
                }
                if (name == "BonusToLowHealth")
                {
                    DamageTypes.AddNewBullet("Executing Rounds", i, DamageColor.FindColor(DamageColorIndex.Fragile));
                }
                if (name == "BlightOnHit")
                {
                    DamageTypes.AddNewBullet("Blighting Rounds", i, new UnityEngine.Color(222f / 255f, 85f / 255f, 230f / 255f));
                }
                if (name == "CrippleOnHit")
                {
                    DamageTypes.AddNewBullet("Crippling Rounds", i, new UnityEngine.Color(48f / 255f, 205f / 255f, 217f / 255f));
                }
                if (name == "SuperBleedOnCrit")
                {
                    DamageTypes.AddNewBullet("Hemorrhaging Rounds", i, DamageColor.FindColor(DamageColorIndex.SuperBleed));
                }
                if (name == "FruitOnHit")
                {
                    DamageTypes.AddNewBullet("Fruitful Rounds", i, new UnityEngine.Color(255f / 255f, 191f / 255f, 225f / 255f));
                }
            }

            DamageTypes.AddNewModdedBullet("Hook Shot", DamageTypes.HookShot, Color.grey);
        }
        public static void AddNewModdedBullet(string name, DamageAPI.ModdedDamageType bulletType, Color color, Sprite icon = null)
        {
            if (icon == null) icon = Assets.bulletSprite;
            DriverBulletInfo bulletDef = new DriverBulletInfo
            {
                nameToken = name,
                moddedBulletType = bulletType,
                bulletType = DamageType.Generic,
                icon = icon,
                trailColor = color  
            };
            bulletTypes.Add(bulletDef);
        }

        public static void AddNewBullet(string name, DamageType bulletType, Color color, Sprite icon = null)
        {
            if (icon == null) icon = Assets.bulletSprite;
            DriverBulletInfo bulletDef = new DriverBulletInfo
            {
                nameToken = name,
                bulletType = bulletType,
                moddedBulletType = Generic,
                icon = icon,
                trailColor = color
            };
            bulletTypes.Add(bulletDef);
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig.Invoke(self, damageInfo, victim);
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            TeamComponent attackerTeam = attackerBody.GetComponent<TeamComponent>();
            TeamIndex attackerTeamIndex = (attackerTeam ? attackerTeam.teamIndex : TeamIndex.Neutral);
            if (NetworkServer.active)
            {
                if (attackerBody)
                {
                    if(damageInfo.HasModdedDamageType(HookShot))
                    {
                        List<HurtBox> list = CollectionPool<HurtBox, List<HurtBox>>.RentCollection();
                        int maxTargets = 10;
                        BullseyeSearch search = new BullseyeSearch();
                        List<HealthComponent> list2 = CollectionPool<HealthComponent, List<HealthComponent>>.RentCollection();
                        if (attackerBody && attackerBody.healthComponent)
                        {
                            list2.Add(attackerBody.healthComponent);
                        }
                        if (victimBody && victimBody)
                        {
                            list2.Add(victimBody.healthComponent);
                        }
                        BounceOrb.SearchForTargets(search, attackerTeamIndex, damageInfo.position, 30f, maxTargets, list, list2);
                        CollectionPool<HealthComponent, List<HealthComponent>>.ReturnCollection(list2);
                        List<HealthComponent> bouncedObjects = new List<HealthComponent> { victim.GetComponent<HealthComponent>() };
                        float damageCoefficient6 = 1f;
                        float damageValue5 = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient6);
                        int j = 0;
                        for (int count = list.Count; j < count; j++)
                        {
                            HurtBox hurtBox3 = list[j];
                            if ((bool)hurtBox3)
                            {
                                BounceOrb bounceOrb = new BounceOrb();
                                bounceOrb.origin = damageInfo.position;
                                bounceOrb.damageValue = damageValue5;
                                bounceOrb.isCrit = damageInfo.crit;
                                bounceOrb.teamIndex = attackerTeamIndex;
                                bounceOrb.attacker = damageInfo.attacker;
                                bounceOrb.procChainMask = damageInfo.procChainMask;
                                bounceOrb.procCoefficient = 0.33f;
                                bounceOrb.damageColorIndex = DamageColorIndex.Default;
                                bounceOrb.bouncedObjects = bouncedObjects;
                                bounceOrb.target = hurtBox3;
                                OrbManager.instance.AddOrb(bounceOrb);
                            }
                        }
                        CollectionPool<HurtBox, List<HurtBox>>.ReturnCollection(list);
                    }
                }
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