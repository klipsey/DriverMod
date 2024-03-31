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
        public static DamageAPI.ModdedDamageType MissileShot;
        public static DamageAPI.ModdedDamageType VoidMissileShot;
        public static DamageAPI.ModdedDamageType ExplosiveRounds;
        public static DamageAPI.ModdedDamageType FlameTornadoShot;
        public static DamageAPI.ModdedDamageType IceBlastShot;
        public static DamageAPI.ModdedDamageType DaggerShot;
        public static DamageAPI.ModdedDamageType LightningStrikeRounds;
        public static DamageAPI.ModdedDamageType FireballRounds;
        public static DamageAPI.ModdedDamageType StickyShot;
        public static DamageAPI.ModdedDamageType VoidLightning;
        public static DamageAPI.ModdedDamageType CoinShot;
        public static DamageAPI.ModdedDamageType MysteryShot;

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
            MissileShot = DamageAPI.ReserveDamageType();
            VoidMissileShot = DamageAPI.ReserveDamageType();
            ExplosiveRounds = DamageAPI.ReserveDamageType();
            FlameTornadoShot = DamageAPI.ReserveDamageType();
            IceBlastShot = DamageAPI.ReserveDamageType();
            DaggerShot = DamageAPI.ReserveDamageType();
            LightningStrikeRounds = DamageAPI.ReserveDamageType();
            FireballRounds = DamageAPI.ReserveDamageType();
            StickyShot = DamageAPI.ReserveDamageType();
            VoidLightning = DamageAPI.ReserveDamageType();
            CoinShot = DamageAPI.ReserveDamageType();
            MysteryShot = DamageAPI.ReserveDamageType();

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
            int common = 5;
            int uncommon = 3;
            int legendary = 1;
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
                    DamageTypes.AddNewBullet("Frostbite Rounds", i, UnityEngine.Color.cyan, null, uncommon);
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
                    DamageTypes.AddNewBullet("Hemorrhaging Rounds", i, DamageColor.FindColor(DamageColorIndex.SuperBleed), null, uncommon);
                }
                if (name == "FruitOnHit")
                {
                    DamageTypes.AddNewBullet("Fruitful Rounds", i, new UnityEngine.Color(255f / 255f, 191f / 255f, 225f / 255f));
                }
            }

            DamageTypes.AddNewModdedBullet("Hook Shot", DamageTypes.HookShot, Color.grey, null, legendary);

            DamageTypes.AddNewModdedBullet("Missle Shot", DamageTypes.MissileShot, new UnityEngine.Color(219 / 255f, 132 / 255f, 11 / 255f), null, legendary);

            DamageTypes.AddNewModdedBullet("Void Rounds", DamageTypes.VoidMissileShot, new UnityEngine.Color(122 / 255f, 69 / 255f, 173 / 255f), null, uncommon);

            DamageTypes.AddNewModdedBullet("Explosive Rounds", DamageTypes.ExplosiveRounds, Color.yellow, null, uncommon);

            DamageTypes.AddNewModdedBullet("Elemental Flame Rounds", DamageTypes.FlameTornadoShot, new UnityEngine.Color(255f / 255f, 127f / 255f, 80 / 255f), null, uncommon);

            DamageTypes.AddNewModdedBullet("Elemental Ice Rounds", DamageTypes.IceBlastShot, UnityEngine.Color.cyan, null, uncommon);

            DamageTypes.AddNewModdedBullet("Dagger Shot", DamageTypes.DaggerShot, Color.black, null, legendary);

            DamageTypes.AddNewModdedBullet("Lightning Rounds", DamageTypes.LightningStrikeRounds, Color.cyan, null, legendary);

            DamageTypes.AddNewModdedBullet("Fireball Rounds", DamageTypes.FireballRounds, new UnityEngine.Color(255f / 255f, 127f / 255f, 80 / 255f), null, legendary);

            DamageTypes.AddNewModdedBullet("Sticky Shot", DamageTypes.StickyShot, new UnityEngine.Color(255 / 255f, 117 / 255f, 48 / 255f));

            DamageTypes.AddNewModdedBullet("Void Strike Rounds", DamageTypes.VoidLightning, new UnityEngine.Color(194 / 255f, 115 / 255f, 255 / 255f), null, uncommon);

            DamageTypes.AddNewModdedBullet("Coin Shot", DamageTypes.CoinShot, new UnityEngine.Color(255 / 255f, 212 / 255f, 94 / 255f));

            DamageTypes.AddNewModdedBullet("Mystery Shot", DamageTypes.MysteryShot, new UnityEngine.Color(30 / 255f, 51 / 255f, 45 / 255f));

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

        public static void AddNewBullet(string name, DamageType bulletType, Color color, Sprite icon = null, int chance = 5)
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
            if (damageInfo.HasModdedDamageType(MysteryShot))
            {
                System.Random rnd = new System.Random();
                int num = rnd.Next(Buffs.bulletDefs.Count);

                damageInfo.damageType = DamageTypes.bulletTypes[num].bulletType;
                damageInfo.RemoveModdedDamageType(MysteryShot);
                damageInfo.AddModdedDamageType(DamageTypes.bulletTypes[num].moddedBulletType);
            }
            orig.Invoke(self, damageInfo, victim);
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            TeamComponent attackerTeam = attackerBody.GetComponent<TeamComponent>();
            TeamIndex attackerTeamIndex = (attackerTeam ? attackerTeam.teamIndex : TeamIndex.Neutral);
            if (NetworkServer.active)
            {
                if (attackerBody)
                {
                    if (damageInfo.HasModdedDamageType(HookShot))
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
                    if(damageInfo.HasModdedDamageType(MissileShot))
                    {
                        if (Util.CheckRoll(10f * damageInfo.procCoefficient, attackerBody.master))
                        {
                            float damageCoefficient = 1f + attackerBody.inventory.GetItemCount(RoR2Content.Items.Missile);
                            float missileDamage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient);
                            MissileUtils.FireMissile(attackerBody.corePosition, attackerBody, damageInfo.procChainMask, victim, missileDamage, damageInfo.crit, Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/MissileProjectile.prefab").WaitForCompletion(), DamageColorIndex.Item, addMissileProc: true);
                        }
                    }
                    if(damageInfo.HasModdedDamageType(VoidMissileShot))
                    {
                        int num3 = attackerBody?.inventory?.GetItemCount(DLC1Content.Items.MoreMissile) ?? 0;
                        float num4 = Mathf.Max(1f, 1f + 0.5f * (num3 - 1));
                        float damageCoefficient2 = 0.4f + 0.4f * attackerBody.inventory.GetItemCount(RoR2Content.Items.Missile);
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
                    if(damageInfo.HasModdedDamageType(FlameTornadoShot))
                    {
                        ProcChainMask procChainMask4 = damageInfo.procChainMask;
                        procChainMask4.AddProc(ProcType.Rings);
                        Vector3 position2 = damageInfo.position;
                        GameObject gameObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/FireTornado");
                        float resetInterval = gameObject.GetComponent<ProjectileOverlapAttack>().resetInterval;
                        float lifetime = gameObject.GetComponent<ProjectileSimple>().lifetime;
                        float damageCoefficient9 = 1.5f + 1.5f * attackerBody.inventory.GetItemCount(RoR2Content.Items.FireRing);
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
                        float damageCoefficient8 = 1.25f + 1.25f * attackerBody.inventory.GetItemCount(RoR2Content.Items.IceRing);
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
                        Transform transform2 = attackerBody.gameObject.transform;
                        if (transform && transform2)
                        {
                            vector = Vector3.Lerp(transform.position, transform2.position, 0.75f);
                            quaternion = transform.rotation;
                        }
                        float damageCoefficient2 = 1.5f + 1.5f * attackerBody.inventory.GetItemCount(RoR2Content.Items.Dagger);
                        Vector3 vector3 = vector + Vector3.up * 1.8f;
                        ProjectileManager.instance.FireProjectile(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Dagger/DaggerProjectile.prefab").WaitForCompletion(), vector3 + UnityEngine.Random.insideUnitSphere * 0.5f, Util.QuaternionSafeLookRotation(Vector3.up + UnityEngine.Random.insideUnitSphere * 0.1f), attackerBody.gameObject, Util.OnKillProcDamage(attackerBody.damage, damageCoefficient2), 200f, Util.CheckRoll(attackerBody.crit, attackerBody.master), DamageColorIndex.Item);
                    }
                    if(damageInfo.HasModdedDamageType(LightningStrikeRounds))
                    {
                        float damageValue6 = Util.OnHitProcDamage(damageInfo.damage,attackerBody.damage, 2.5f + 2.5f * attackerBody.inventory.GetItemCount(RoR2Content.Items.LightningStrikeOnHit));
                        ProcChainMask procChainMask8 = damageInfo.procChainMask;
                        procChainMask8.AddProc(ProcType.LightningStrikeOnHit);
                        HurtBox target = victimBody.mainHurtBox;
                        if (victimBody.hurtBoxGroup)
                        {
                            target = victimBody.hurtBoxGroup.hurtBoxes[UnityEngine.Random.Range(0, victimBody.hurtBoxGroup.hurtBoxes.Length)];
                        }
                        OrbManager.instance.AddOrb(new SimpleLightningStrikeOrb
                        {
                            attacker = attackerBody.gameObject,
                            damageColorIndex = DamageColorIndex.Item,
                            damageValue = damageValue6,
                            isCrit = Util.CheckRoll(attackerBody.crit, attackerBody.master),
                            procChainMask = procChainMask8,
                            procCoefficient = 1f,
                            target = target
                        });
                    }
                    if(damageInfo.HasModdedDamageType(FireballRounds))
                    {
                        InputBankTest component5 = attackerBody.GetComponent<InputBankTest>();
                        Vector3 vector2 = (attackerBody.characterMotor ? (victim.transform.position + Vector3.up * (attackerBody.characterMotor.capsuleHeight * 0.5f + 2f)) : (victim.transform.position + Vector3.up * 2f));
                        Vector3 vector3 = (component5 ? component5.aimDirection : victim.transform.forward);
                        vector3 = Vector3.up;
                        float num12 = 20f;
                        if (Util.CheckRoll(10f * damageInfo.procCoefficient, attackerBody.master))
                        {
                            EffectData effectData = new EffectData
                            {
                                scale = 1f,
                                origin = vector2
                            };
                            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashFireMeatBall"), effectData, transmit: true);
                            int num13 = 3;
                            float damageCoefficient11 = 1.5f + 1.5f * attackerBody.inventory.GetItemCount(RoR2Content.Items.FireballsOnHit);
                            float damage5 = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient11);
                            float max = 30f;
                            ProcChainMask procChainMask7 = damageInfo.procChainMask;
                            procChainMask7.AddProc(ProcType.Meatball);
                            float speedOverride2 = UnityEngine.Random.Range(15f, max);
                            float num14 = 360 / num13;
                            _ = num14 / 360f;
                            float num15 = 1f;
                            float num16 = num14;
                            for (int n = 0; n < num13; n++)
                            {
                                float num17 = (float)n * (float)Math.PI * 2f / (float)num13;
                                FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                                fireProjectileInfo.projectilePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/FireMeatBall");
                                fireProjectileInfo.position = vector2 + new Vector3(num15 * Mathf.Sin(num17), 0f, num15 * Mathf.Cos(num17));
                                fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(vector3);
                                fireProjectileInfo.procChainMask = procChainMask7;
                                fireProjectileInfo.target = victim;
                                fireProjectileInfo.owner = attackerBody.gameObject;
                                fireProjectileInfo.damage = damage5;
                                fireProjectileInfo.crit = damageInfo.crit;
                                fireProjectileInfo.force = 200f;
                                fireProjectileInfo.damageColorIndex = DamageColorIndex.Item;
                                fireProjectileInfo.speedOverride = speedOverride2;
                                fireProjectileInfo.useSpeedOverride = true;
                                FireProjectileInfo fireProjectileInfo2 = fireProjectileInfo;
                                num16 += num14;
                                ProjectileManager.instance.FireProjectile(fireProjectileInfo2);
                                vector3.x += Mathf.Sin(num17 + UnityEngine.Random.Range(0f - num12, num12));
                                vector3.z += Mathf.Cos(num17 + UnityEngine.Random.Range(0f - num12, num12));
                            }
                        }
                        if(damageInfo.HasModdedDamageType(StickyShot))
                        {
                            bool alive = attackerBody.healthComponent.alive;
                            float num9 = 5f;
                            Vector3 position = damageInfo.position;
                            Vector3 forward = victimBody.corePosition - position;
                            float magnitude = forward.magnitude;
                            Quaternion rotation = ((magnitude != 0f) ? Util.QuaternionSafeLookRotation(forward) : UnityEngine.Random.rotationUniform);
                            float damageCoefficient7 = 1.8f;
                            float damage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient7);
                            ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/StickyBomb"), position, rotation, damageInfo.attacker, damage, 100f, damageInfo.crit, DamageColorIndex.Item, null, alive ? (magnitude * num9) : (-1f));
                        }
                        if (damageInfo.HasModdedDamageType(VoidLightning))
                        {
                            float damageCoefficient5 = 0.6f;
                            float damageValue4 = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient5);
                            VoidLightningOrb voidLightningOrb = new VoidLightningOrb();
                            voidLightningOrb.origin = damageInfo.position;
                            voidLightningOrb.damageValue = damageValue4;
                            voidLightningOrb.isCrit = damageInfo.crit;
                            voidLightningOrb.totalStrikes = 1 + 3 * attackerBody.inventory.GetItemCount(DLC1Content.Items.ChainLightningVoid);
                            voidLightningOrb.teamIndex = attackerTeamIndex;
                            voidLightningOrb.attacker = damageInfo.attacker;
                            voidLightningOrb.procChainMask = damageInfo.procChainMask;
                            voidLightningOrb.procChainMask.AddProc(ProcType.ChainLightning);
                            voidLightningOrb.procCoefficient = 0.2f;
                            voidLightningOrb.damageColorIndex = DamageColorIndex.Void;
                            voidLightningOrb.secondsPerStrike = 0.1f;
                            HurtBox mainHurtBox2 = attackerBody.mainHurtBox;
                            if ((bool)mainHurtBox2)
                            {
                                voidLightningOrb.target = mainHurtBox2;
                                OrbManager.instance.AddOrb(voidLightningOrb);
                            }
                        }
                        if(damageInfo.HasModdedDamageType(CoinShot))
                        {
                            GoldOrb goldOrb = new GoldOrb();
                            goldOrb.origin = damageInfo.position;
                            goldOrb.target = attackerBody.mainHurtBox;
                            goldOrb.goldAmount = (uint)(2f * Run.instance.difficultyCoefficient);
                            OrbManager.instance.AddOrb(goldOrb);
                            EffectManager.SimpleImpactEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/CoinImpact"), damageInfo.position, Vector3.up, transmit: true);
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