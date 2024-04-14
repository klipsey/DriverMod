﻿using R2API;
using RoR2.Orbs;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RobDriver.Modules;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace RobDriver.Modules
{
    public static class BulletTypes
    {
        internal static List<DriverBulletDef> bulletDefs { get; private set; } = new List<DriverBulletDef>();

        internal static DriverBulletDef Default { get; private set; }

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
            DamageType.FruitOnHit
        };

        internal static void Init()
        {
            InitializeBullets();
        }

        public static void InitializeBullets()
        {
            Default = DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo()
            {
                nameToken = "Default",
                tier = DriverWeaponTier.Unique,
                index = 0
            });
            bulletDefs.Add(Default);

            foreach (DamageType i in allowedDamageTypes)
            {
                //Renaming
                switch (i)
                {
                    // common
                    case DamageType.SlowOnHit:
                        CreateBulletType("Slowing Rounds", i, DriverWeaponTier.Common, Color.yellow);
                        break;

                    case DamageType.Stun1s:
                        CreateBulletType("Stunning Rounds", i, DriverWeaponTier.Common, Color.gray);
                        break;

                    case DamageType.IgniteOnHit:
                        CreateBulletType("Incendiary Rounds", i, DriverWeaponTier.Common, new Color(255f / 255f, 127f / 255f, 80 / 255f));
                        break;

                    case DamageType.ClayGoo:
                        CreateBulletType("Goo Rounds", i, DriverWeaponTier.Common, Color.black);
                        break;

                    case DamageType.BleedOnHit:
                        CreateBulletType("Serrated Rounds", i, DriverWeaponTier.Common, DamageColor.FindColor(DamageColorIndex.Bleed));
                        break;

                    case DamageType.PoisonOnHit:
                        CreateBulletType("Poison Rounds", i, DriverWeaponTier.Common, Color.green);
                        break;

                    case DamageType.WeakOnHit:
                        CreateBulletType("Weakening Rounds", i, DriverWeaponTier.Common, new Color(220f / 255f, 237f / 255f, 159f / 255f));
                        break;

                    case DamageType.BonusToLowHealth:
                        CreateBulletType("Executing Rounds", i, DriverWeaponTier.Common, DamageColor.FindColor(DamageColorIndex.Fragile));
                        break;

                    case DamageType.BlightOnHit:
                        CreateBulletType("Blighting Rounds", i, DriverWeaponTier.Common, new Color(222f / 255f, 85f / 255f, 230f / 255f));
                        break;

                    // uncommon
                    case DamageType.ResetCooldownsOnKill:
                        CreateBulletType("Resetting Rounds", i, DriverWeaponTier.Uncommon, Color.red);
                        break;

                    case DamageType.CrippleOnHit:
                        CreateBulletType("Crippling Rounds", i, DriverWeaponTier.Uncommon, new Color(48f / 255f, 205f / 255f, 217f / 255f));
                        break;

                    case DamageType.FruitOnHit:
                        CreateBulletType("Fruitful Rounds", i, DriverWeaponTier.Uncommon, new Color(255f / 255f, 191f / 255f, 225f / 255f));
                        break;

                    case DamageType.Freeze2s:
                        CreateBulletType("Frostbite Rounds", i, DriverWeaponTier.Uncommon, Color.cyan);
                        break;

                    // void
                    case DamageType.Nullify:
                        CreateBulletType("Nullifying Rounds", i, DriverWeaponTier.Uncommon, DamageColor.FindColor(DamageColorIndex.Void));
                        break;
                }
            }

            // common
            CreateBulletType("Coin Shot", DamageTypes.CoinShot, DriverWeaponTier.Common, new Color(255 / 255f, 212 / 255f, 94 / 255f));

            // uncommon
            CreateBulletType("Explosive Rounds", DamageTypes.ExplosiveRounds, DriverWeaponTier.Uncommon, Color.yellow);
            CreateBulletType("Missle Shot", DamageTypes.MissileShot, DriverWeaponTier.Uncommon, new Color(219 / 255f, 132 / 255f, 11 / 255f));
            CreateBulletType("Elemental Flame Rounds", DamageTypes.FlameTornadoShot, DriverWeaponTier.Uncommon, new Color(255f / 255f, 127f / 255f, 80 / 255f));
            CreateBulletType("Elemental Ice Rounds", DamageTypes.IceBlastShot, DriverWeaponTier.Uncommon, Color.cyan);
            CreateBulletType("Sticky Shot", DamageTypes.StickyShot, DriverWeaponTier.Uncommon, new Color(255 / 255f, 117 / 255f, 48 / 255f));
            CreateBulletType("Mystery Shot", DamageTypes.MysteryShot, DriverWeaponTier.Uncommon, new Color(30 / 255f, 51 / 255f, 45 / 255f));
            CreateBulletType("Hemorrhaging Rounds", DamageTypes.Hemorrhage, DriverWeaponTier.Uncommon, DamageColor.FindColor(DamageColorIndex.SuperBleed));

            // legendary
            CreateBulletType("Dagger Shot", DamageTypes.DaggerShot, DriverWeaponTier.Legendary, Color.black);
            CreateBulletType("Lightning Rounds", DamageTypes.LightningStrikeRounds, DriverWeaponTier.Legendary, Color.cyan);
            CreateBulletType("Fireball Rounds", DamageTypes.FireballRounds, DriverWeaponTier.Legendary, new Color(255f / 255f, 127f / 255f, 80 / 255f));
            CreateBulletType("Hook Shot", DamageTypes.HookShot, DriverWeaponTier.Legendary, Color.grey);

            //void
            CreateBulletType("Void Missile Rounds", DamageTypes.VoidMissileShot, DriverWeaponTier.Uncommon, new Color(122 / 255f, 69 / 255f, 173 / 255f));
            CreateBulletType("Void Lightning Rounds", DamageTypes.VoidLightning, DriverWeaponTier.Legendary, new Color(194 / 255f, 115 / 255f, 255 / 255f));

        }

        public static void CreateBulletType(string nameToken, DamageType damageType, DriverWeaponTier tier, DamageColorIndex color)
        {
            CreateBulletType(nameToken, damageType, DamageTypes.Generic, tier, DamageColor.FindColor(color));
        }

        public static void CreateBulletType(string nameToken, DamageType damageType, DriverWeaponTier tier, Color color)
        {
            CreateBulletType(nameToken, damageType, DamageTypes.Generic, tier, color);
        }

        public static void CreateBulletType(string nameToken, DamageAPI.ModdedDamageType moddedBulletType, DriverWeaponTier tier, DamageColorIndex color)
        {
            CreateBulletType(nameToken, DamageType.Generic, moddedBulletType, tier, DamageColor.FindColor(color));
        }

        public static void CreateBulletType(string nameToken, DamageAPI.ModdedDamageType moddedBulletType, DriverWeaponTier tier, Color color)
        {
            CreateBulletType(nameToken, DamageType.Generic, moddedBulletType, tier, color);
        }

        public static void CreateBulletType(string nameToken, DamageType damageType, DamageAPI.ModdedDamageType moddedBulletType, DriverWeaponTier tier, Color color)
        {
            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = nameToken,
                bulletType = damageType,
                moddedBulletType = moddedBulletType,
                tier = tier,
                trailColor = color,
                index = bulletDefs.Count
            }));
        }

        public static DriverBulletDef GetBulletDefFromIndex(int index)
        {
            return bulletDefs.ElementAtOrDefault(index) ?? Default;
        }

        public static DriverBulletDef GetWeightedRandomBullet(DriverWeaponTier maxTier)
        {
            int commonWeight = 5;
            int uncommonWeight = maxTier == DriverWeaponTier.Uncommon ? 3 : 0;
            int legendaryWeight = maxTier == DriverWeaponTier.Legendary ? 1 : 0;
            int rnd = UnityEngine.Random.Range(0, commonWeight + uncommonWeight + legendaryWeight);

            if (rnd < commonWeight) return GetRandomBulletFromTier(DriverWeaponTier.Common);
            if (rnd < commonWeight + uncommonWeight) return GetRandomBulletFromTier(DriverWeaponTier.Uncommon);
            return GetRandomBulletFromTier(DriverWeaponTier.Legendary);
        }

        public static DriverBulletDef GetRandomBulletFromTier(DriverWeaponTier tier)
        {
            var validBullets = bulletDefs.Where(bullet => bullet.tier == tier);
            int rnd = UnityEngine.Random.Range(0, validBullets.Count());

            return validBullets.ElementAtOrDefault(rnd) ?? Default;
        }
    }
}