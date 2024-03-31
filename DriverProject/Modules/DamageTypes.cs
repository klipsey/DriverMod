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
using UnityEngine.AddressableAssets;
using RoR2.Projectile;
using UnityEngine.UIElements;
using System.ComponentModel;

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
        public static DamageAPI.ModdedDamageType MissleShot;
        public static DamageAPI.ModdedDamageType VoidMissileShot;
        public static DamageAPI.ModdedDamageType ExplosiveRounds;
        public static DamageAPI.ModdedDamageType FlameTornadoShot;
        public static DamageAPI.ModdedDamageType IceBlastShot;
        public static DamageAPI.ModdedDamageType DaggerShot;

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
            MissleShot = DamageAPI.ReserveDamageType();
            VoidMissileShot = DamageAPI.ReserveDamageType();
            ExplosiveRounds = DamageAPI.ReserveDamageType();
            FlameTornadoShot = DamageAPI.ReserveDamageType();
            IceBlastShot = DamageAPI.ReserveDamageType();
            DaggerShot = DamageAPI.ReserveDamageType();
            InitializeBullets();
            Hook();
        }

        private static void Hook()
        {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            On.RoR2.GlobalEventManager.OnHitEnemy += new On.RoR2.GlobalEventManager.hook_OnHitEnemy(GlobalEventManager_OnHitEnemy);
            On.RoR2.GlobalEventManager.OnHitAll += new On.RoR2.GlobalEventManager.hook_OnHitAll(GlobalEventManager_OnHitAll);
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

            DamageTypes.AddNewModdedBullet("Missle Shot", DamageTypes.MissleShot, new UnityEngine.Color(219 / 255f, 132 / 255f, 11 / 255f));

            DamageTypes.AddNewModdedBullet("Void Rounds", DamageTypes.VoidMissileShot, new UnityEngine.Color(122 / 255f, 69 / 255f, 173 / 255f));

            DamageTypes.AddNewModdedBullet("Explosive Rounds", DamageTypes.ExplosiveRounds, Color.yellow);

            DamageTypes.AddNewModdedBullet("Elemental Flame Rounds", DamageTypes.FlameTornadoShot, new UnityEngine.Color(255f / 255f, 127f / 255f, 80 / 255f));

            DamageTypes.AddNewModdedBullet("Elemental Ice Rounds", DamageTypes.IceBlastShot, UnityEngine.Color.cyan);

            DamageTypes.AddNewModdedBullet("Dagger Shot", DamageTypes.DaggerShot, Color.black);
        }
        public static void AddNewModdedBullet(string name, DamageAPI.ModdedDamageType bulletType, Color color, Sprite icon = null, int chance = 1)
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
            for (int i = 0; i < chance; i++) bulletTypes.Add(bulletDef);
        }

        public static void AddNewBullet(string name, DamageType bulletType, Color color, Sprite icon = null, int chance = 1)
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
            for (int i = 0; i < chance; i++) bulletTypes.Add(bulletDef);
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
                    if(damageInfo.HasModdedDamageType(MissleShot))
                    {
                        if (Util.CheckRoll(10f * damageInfo.procCoefficient, attackerBody.master))
                        {
                            float damageCoefficient = 3f * attackerBody.inventory.GetItemCount(RoR2Content.Items.Missile);
                            float missileDamage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient);
                            MissileUtils.FireMissile(attackerBody.corePosition, attackerBody, damageInfo.procChainMask, victim, missileDamage, damageInfo.crit, Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/MissileProjectile.prefab").WaitForCompletion(), DamageColorIndex.Item, addMissileProc: true);
                        }
                    }
                    if(damageInfo.HasModdedDamageType(VoidMissileShot))
                    {
                        int num3 = attackerBody?.inventory?.GetItemCount(DLC1Content.Items.MoreMissile) ?? 0;
                        float num4 = Mathf.Max(1f, 1f + 0.5f * (float)(num3 - 1));
                        float damageCoefficient2 = 0.4f * attackerBody.inventory.GetItemCount(RoR2Content.Items.Missile);
                        float damageValue = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient2) * num4;
                        int num5 = ((num3 <= 0) ? 1 : 3);
                        for (int i = 0; i < num5; i++)
                        {
                            MissileVoidOrb missileVoidOrb = new MissileVoidOrb();
                            missileVoidOrb.origin = attackerBody.aimOrigin;
                            missileVoidOrb.damageValue = damageValue;
                            missileVoidOrb.isCrit = damageInfo.crit;
                            missileVoidOrb.teamIndex = attackerTeamIndex;
                            missileVoidOrb.attacker = damageInfo.attacker;
                            missileVoidOrb.procChainMask = damageInfo.procChainMask;
                            missileVoidOrb.procChainMask.AddProc(ProcType.Missile);
                            missileVoidOrb.procCoefficient = 0.2f;
                            missileVoidOrb.damageColorIndex = DamageColorIndex.Void;
                            HurtBox mainHurtBox = victimBody.mainHurtBox;
                            if ((bool)mainHurtBox)
                            {
                                missileVoidOrb.target = mainHurtBox;
                                OrbManager.instance.AddOrb(missileVoidOrb);
                            }
                        }
                    }
                    if(damageInfo.HasModdedDamageType(ExplosiveRounds))
                    {

                    }
                    if(damageInfo.HasModdedDamageType(FlameTornadoShot))
                    {
                        ProcChainMask procChainMask4 = damageInfo.procChainMask;
                        procChainMask4.AddProc(ProcType.Rings);
                        Vector3 position2 = damageInfo.position;
                        GameObject gameObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/FireTornado");
                        float resetInterval = gameObject.GetComponent<ProjectileOverlapAttack>().resetInterval;
                        float lifetime = gameObject.GetComponent<ProjectileSimple>().lifetime;
                        float damageCoefficient9 = 1.5f * attackerBody.inventory.GetItemCount(RoR2Content.Items.FireRing);
                        float damage3 = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient9) / lifetime * resetInterval;
                        float speedOverride = 0f;
                        Quaternion rotation2 = Quaternion.identity;
                        Vector3 vector = position2 - attackerBody.aimOrigin;
                        vector.y = 0f;
                        if (vector != Vector3.zero)
                        {
                            speedOverride = -1f;
                            rotation2 = Util.QuaternionSafeLookRotation(vector, Vector3.up);
                        }
                        ProjectileManager.instance.FireProjectile(new FireProjectileInfo
                        {
                            damage = damage3,
                            crit = damageInfo.crit,
                            damageColorIndex = DamageColorIndex.Item,
                            position = position2,
                            procChainMask = procChainMask4,
                            force = 0f,
                            owner = damageInfo.attacker,
                            projectilePrefab = gameObject,
                            rotation = rotation2,
                            speedOverride = speedOverride,
                            target = null
                        });
                    }
                    if(damageInfo.HasModdedDamageType(IceBlastShot))
                    {
                        ProcChainMask procChainMask4 = damageInfo.procChainMask;
                        procChainMask4.AddProc(ProcType.Rings);
                        Vector3 position2 = damageInfo.position;
                        float damageCoefficient8 = 1.25f * attackerBody.inventory.GetItemCount(RoR2Content.Items.IceRing);
                        float damage2 = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient8);
                        DamageInfo damageInfo2 = new DamageInfo
                        {
                            damage = damage2,
                            damageColorIndex = DamageColorIndex.Item,
                            damageType = DamageType.Generic,
                            attacker = damageInfo.attacker,
                            crit = damageInfo.crit,
                            force = Vector3.zero,
                            inflictor = null,
                            position = position2,
                            procChainMask = procChainMask4,
                            procCoefficient = 1f
                        };
                        EffectManager.SimpleImpactEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/IceRingExplosion"), position2, Vector3.up, transmit: true);
                        victimBody.AddTimedBuff(RoR2Content.Buffs.Slow80, 3f * attackerBody.inventory.GetItemCount(RoR2Content.Items.IceRing));
                        victimBody.healthComponent.TakeDamage(damageInfo2);
                    }
                    if(damageInfo.HasModdedDamageType(DaggerShot))
                    {
                        Vector3 vector = Vector3.zero;
                        Quaternion quaternion = Quaternion.identity;
                        Vector3 vector2 = Vector3.zero;
                        Transform transform = victimBody.gameObject.transform;
                        if ((bool)transform)
                        {
                            vector = transform.position;
                            quaternion = transform.rotation;
                        }
                        float damageCoefficient2 = 1.5f * attackerBody.inventory.GetItemCount(RoR2Content.Items.Dagger);
                        Vector3 vector3 = vector + Vector3.up * 1.8f;
                        for (int j = 0; j < 3; j++)
                        {
                            ProjectileManager.instance.FireProjectile(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Dagger/DaggerProjectile.prefab").WaitForCompletion(), vector3 + UnityEngine.Random.insideUnitSphere * 0.5f, Util.QuaternionSafeLookRotation(Vector3.up + UnityEngine.Random.insideUnitSphere * 0.1f), attackerBody.gameObject, Util.OnKillProcDamage(attackerBody.damage, damageCoefficient2), 200f, Util.CheckRoll(attackerBody.crit, attackerBody.master), DamageColorIndex.Item);
                        }
                    }
                }
            }
        }

        private static void GlobalEventManager_OnHitAll(On.RoR2.GlobalEventManager.orig_OnHitAll orig, GlobalEventManager self, DamageInfo damageInfo, GameObject hitObject)
        {
            orig.Invoke(self, damageInfo, hitObject);
            if (damageInfo.procCoefficient == 0f || damageInfo.rejected)
            {
                return;
            }
            _ = NetworkServer.active;
            if (!damageInfo.attacker)
            {
                return;
            }
            CharacterBody component = damageInfo.attacker.GetComponent<CharacterBody>();
            if (!component)
            {
                return;
            }
            CharacterMaster master = component.master;
            if (!master)
            {
                return;
            }
            Inventory inventory = master.inventory;
            if (!master.inventory)
            {
                return;
            }
            int itemCount = inventory.GetItemCount(RoR2Content.Items.Behemoth);
            if (damageInfo.HasModdedDamageType(ExplosiveRounds))
            {
                float num = (1.5f + 2.5f * itemCount) * damageInfo.procCoefficient;
                float damageCoefficient = 0.6f;
                float baseDamage = Util.OnHitProcDamage(damageInfo.damage, component.damage, damageCoefficient);
                EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick"), new EffectData
                {
                    origin = damageInfo.position,
                    scale = num,
                    rotation = Util.QuaternionSafeLookRotation(damageInfo.force)
                }, transmit: true);
                BlastAttack obj = new BlastAttack
                {
                    position = damageInfo.position,
                    baseDamage = baseDamage,
                    baseForce = 0f,
                    radius = num,
                    attacker = damageInfo.attacker,
                    inflictor = null
                };
                obj.teamIndex = TeamComponent.GetObjectTeam(obj.attacker);
                obj.crit = damageInfo.crit;
                obj.procChainMask = damageInfo.procChainMask;
                obj.procCoefficient = 0f;
                obj.damageColorIndex = DamageColorIndex.Item;
                obj.falloffModel = BlastAttack.FalloffModel.None;
                obj.damageType = damageInfo.damageType;
                obj.Fire();
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