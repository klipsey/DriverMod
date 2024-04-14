using R2API;
using RoR2.Orbs;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RobDriver.Modules;

namespace RobDriver.Modules
{
    public static class BulletTypes
    {
        public static List<DriverBulletDef> bulletDefs = new List<DriverBulletDef>();

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
            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Default",
                tier = DriverWeaponTier.Common,
                index = bulletDefs.Count
            }));

            foreach (DamageType i in allowedDamageTypes)
            {
                //Renaming
                switch (i)
                {
                    case DamageType.ResetCooldownsOnKill:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Resetting Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Uncommon,
                            trailColor = Color.red,
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.SlowOnHit:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Slowing Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Common,
                            trailColor = Color.yellow,
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.Stun1s:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Stunning Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Common,
                            trailColor = Color.gray,
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.IgniteOnHit:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Incendiary Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Common,
                            trailColor = new Color(255f / 255f, 127f / 255f, 80 / 255f),
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.Freeze2s:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Frostbite Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Uncommon,
                            trailColor = Color.cyan,
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.ClayGoo:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Goo Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Common,
                            trailColor = Color.black,
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.BleedOnHit:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Serrated Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Common,
                            trailColor = DamageColor.FindColor(DamageColorIndex.Bleed),
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.PoisonOnHit:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Poison Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Common,
                            trailColor = Color.green,
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.WeakOnHit:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Weakening Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Common,
                            trailColor = new Color(220f / 255f, 237f / 255f, 159f / 255f),
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.Nullify:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Nullifying Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Void,
                            trailColor = DamageColor.FindColor(DamageColorIndex.Void),
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.BonusToLowHealth:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Executing Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Common,
                            trailColor = DamageColor.FindColor(DamageColorIndex.Fragile),
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.BlightOnHit:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Blighting Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Common,
                            trailColor = new Color(222f / 255f, 85f / 255f, 230f / 255f),
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.CrippleOnHit:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Crippling Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Uncommon,
                            trailColor = new Color(48f / 255f, 205f / 255f, 217f / 255f),
                            index = bulletDefs.Count
                        }));
                        break;

                    case DamageType.FruitOnHit:
                        bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
                        {
                            nameToken = "Fruitful Rounds",
                            bulletType = i,
                            tier = DriverWeaponTier.Uncommon,
                            trailColor = new Color(255f / 255f, 191f / 255f, 225f / 255f),
                            index = bulletDefs.Count
                        }));
                        break;
                }
            }

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Hook Shot",
                moddedBulletType = DamageTypes.HookShot,
                tier = DriverWeaponTier.Legendary,
                trailColor = Color.grey,
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Missle Shot",
                moddedBulletType = DamageTypes.MissileShot,
                tier = DriverWeaponTier.Uncommon,
                trailColor = new Color(219 / 255f, 132 / 255f, 11 / 255f),
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Void Missile Rounds",
                moddedBulletType = DamageTypes.VoidMissileShot,
                tier = DriverWeaponTier.Void,
                trailColor = new Color(122 / 255f, 69 / 255f, 173 / 255f),
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Explosive Rounds",
                moddedBulletType = DamageTypes.ExplosiveRounds,
                tier = DriverWeaponTier.Uncommon,
                trailColor = Color.yellow,
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Elemental Flame Rounds",
                moddedBulletType = DamageTypes.FlameTornadoShot,
                tier = DriverWeaponTier.Uncommon,
                trailColor = new Color(255f / 255f, 127f / 255f, 80 / 255f),
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Elemental Ice Rounds",
                moddedBulletType = DamageTypes.IceBlastShot,
                tier = DriverWeaponTier.Uncommon,
                trailColor = Color.cyan,
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Dagger Shot",
                moddedBulletType = DamageTypes.DaggerShot,
                tier = DriverWeaponTier.Legendary,
                trailColor = Color.black,
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Lightning Rounds",
                moddedBulletType = DamageTypes.LightningStrikeRounds,
                tier = DriverWeaponTier.Legendary,
                trailColor = Color.cyan,
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Fireball Rounds",
                moddedBulletType = DamageTypes.FireballRounds,
                tier = DriverWeaponTier.Legendary,
                trailColor = new Color(255f / 255f, 127f / 255f, 80 / 255f),
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Sticky Shot",
                moddedBulletType = DamageTypes.StickyShot,
                tier = DriverWeaponTier.Uncommon,
                trailColor = new Color(255 / 255f, 117 / 255f, 48 / 255f),
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Void Lightning Rounds",
                moddedBulletType = DamageTypes.VoidLightning,
                tier = DriverWeaponTier.Void,
                trailColor = new Color(194 / 255f, 115 / 255f, 255 / 255f),
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Coin Shot",
                moddedBulletType = DamageTypes.CoinShot,
                tier = DriverWeaponTier.Common,
                trailColor = new Color(255 / 255f, 212 / 255f, 94 / 255f),
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Mystery Shot",
                moddedBulletType = DamageTypes.CoinShot,
                tier = DriverWeaponTier.Uncommon,
                trailColor = new Color(30 / 255f, 51 / 255f, 45 / 255f),
                index = bulletDefs.Count
            }));

            bulletDefs.Add(DriverBulletDef.CreateBulletDefFromInfo(new DriverBulletDefInfo
            {
                nameToken = "Hemorrhaging Rounds",
                moddedBulletType = DamageTypes.Hemorrhage,
                tier = DriverWeaponTier.Uncommon,
                trailColor = DamageColor.FindColor(DamageColorIndex.SuperBleed),
                index = bulletDefs.Count
            }));
        }

        public static DriverBulletDef GetBulletDefFromIndex(int index)
        {
            Log.Debug(index);
            Log.Debug(BulletTypes.bulletDefs.Count);
            if (index < 0 || index >= bulletDefs.Count) return bulletDefs[0];
            else return bulletDefs[index];
        }
        public static DriverWeaponTier GetWeightedBulletTier()
        {
            int commonWeight = 5;
            int uncommonWeight = 3;
            int legendaryWeight = 1;
            int rnd = UnityEngine.Random.Range(0, commonWeight + uncommonWeight + legendaryWeight);

            if (rnd < commonWeight) return DriverWeaponTier.Common;
            if (rnd < commonWeight + uncommonWeight) return DriverWeaponTier.Uncommon;
            return DriverWeaponTier.Legendary;
        }
        //Method to get a random bullet index. set to false to only get a random index within a tier
        public static int GetRandomIndexFromTier(DriverWeaponTier tier, bool canGetTiersBelow = true, bool weighted = true)
        {
            List<DriverBulletDef> validBullets = new List<DriverBulletDef>();
            if(weighted)
            {
                tier = GetWeightedBulletTier();
            }
            foreach (DriverBulletDef i in bulletDefs)
            {
                if (i && i != bulletDefs[0])
                {
                    if (canGetTiersBelow && (int)i.tier <= (int)tier)
                    {
                        validBullets.Add(i);
                    }
                    else if(i.tier == tier)
                    {
                        validBullets.Add(i);
                    }
                }
            }

            if (validBullets.Count <= 0) return 0; // Failsafe

            return validBullets[UnityEngine.Random.Range(0, validBullets.Count)].index;
        }
    }
}