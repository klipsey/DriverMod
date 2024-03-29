using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.Modules
{
    public static class Buffs
    {
        internal static List<BuffDef> buffDefs = new List<BuffDef>();
        internal static List<BuffDef> bulletDefs = new List<BuffDef>();

        internal static BuffDef dazedDebuff;
        internal static BuffDef woundDebuff;
        internal static BuffDef syringeDamageBuff;
        internal static BuffDef syringeAttackSpeedBuff;
        internal static BuffDef syringeCritBuff;
        internal static BuffDef syringeScepterBuff;
        internal static BuffDef bulletsOn;

        internal static List<DamageType> allowedDamageTypes = new List<DamageType>();
        //store this shit somewhere else... Needs to load before buff defs tho..
        internal static DamageType[] bannedDamageTypes = new DamageType[]
        {
            DamageType.Generic,
            DamageType.NonLethal,
            DamageType.BypassArmor,
            DamageType.WeakPointHit,
            DamageType.BypassBlock,
            DamageType.Silent,
            DamageType.BypassOneShotProtection,
            DamageType.FallDamage,
            DamageType.PercentIgniteOnHit,
            DamageType.ApplyMercExpose,
            DamageType.Shock5s,
            DamageType.LunarSecondaryRootOnHit,
            DamageType.OutOfBounds,
            DamageType.GiveSkullOnKill,
            DamageType.VoidDeath,
            DamageType.DoT,
            DamageType.AOE
        };

        internal static void RegisterBuffs()
        {
            dazedDebuff = AddNewBuff("RobDriverDazedDebuff", Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdCloak.asset").WaitForCompletion().iconSprite, Color.grey, false, false);
            woundDebuff = AddNewBuff("RobDriverWoundDebuff", Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Bandit2/bdBanditSkull.asset").WaitForCompletion().iconSprite, Color.red, false, true);
            syringeDamageBuff = AddNewBuff("RobDriverSyringeDamageBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffSyringe"), new Color(1f, 70f / 255f, 75f / 255f), false, false);
            syringeAttackSpeedBuff = AddNewBuff("RobDriverSyringeAttackSpeedBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffSyringe"), new Color(1f, 170f / 255f, 45f / 255f), false, false);
            syringeCritBuff = AddNewBuff("RobDriverSyringeCritBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffSyringe"), new Color(1f, 80f / 255f, 17f / 255f), false, false);
            syringeScepterBuff = AddNewBuff("RobDriverSyringeScepterBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffSyringe"), Modules.Survivors.Driver.characterColor, false, false);
            bulletsOn = AddNewBuff("RobDriverBulletsBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), Color.white, false, false);
            InitValidDamageTypes();
        }

        //I am sorry rob.
        internal static void InitValidDamageTypes()
        {
            bool remove = false;

            //To make this work for modded damagetypes, create a dict with DamageAPI.ModdedDamageType that devs can add their moddeddamagetype, the name and a color the loop through it
            /*
            * for(int i = 0; i < BulletsCatalog.moddedBullets.length; i++)
            * {
            * etc...
            * 
            * }
            */
            foreach (DamageType i in Enum.GetValues(typeof(DamageType)))
            {
                remove = false;
                foreach (DamageType j in bannedDamageTypes)
                {
                    if (i == j) remove = true;
                }
                if (remove != true)
                {
                    allowedDamageTypes.Add(i);
                }
            }


            //I am sorry. Ill make a list later.
            /*For modded damage types add afterward
            for(int j = 0; j < BulletsCatalog.moddedBullets.length; j++)
            {
                damageTypeToColor.Add(BulletsCatalog.moddedBullets.string, BulletsCatalog.moddedBullets.Color);
                count++;
            } 
             */
            int count = 0;
            bool found;
            foreach (DamageType type in Enum.GetValues(typeof(DamageType)))
            {
                found = false;
                if (count > allowedDamageTypes.Count - 1) break;
                if (allowedDamageTypes[count] == type)
                {
                    string i = Enum.GetName(typeof(DamageType), type);
                    if (i == "ResetCooldownsOnKill")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), UnityEngine.Color.red, false, false, true);
                        found = true;
                    }
                    if (i == "SlowOnHit")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), UnityEngine.Color.yellow, false, false, true);
                        found = true;
                    }
                    if (i == "Stun1s")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), UnityEngine.Color.gray, false, false, true);
                        found = true;
                    }
                    if (i == "IgniteOnHit")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), new UnityEngine.Color(255f / 255f, 127f / 255f, 80 / 255f), false, false, true);
                        found = true;
                    }
                    if (i == "Freeze2s")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), UnityEngine.Color.cyan, false, false, true);
                        found = true;
                    }
                    if (i == "ClayGoo")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), UnityEngine.Color.black, false, false, true);
                        found = true;
                    }
                    if (i == "BleedOnHit")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), DamageColor.FindColor(DamageColorIndex.Bleed), false, false, true);
                        found = true;
                    }
                    if (i == "PoisonOnHit")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), UnityEngine.Color.green, false, false, true);
                        found = true;
                    }
                    if (i == "WeakOnHit")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), new UnityEngine.Color(220f / 255f, 237f / 255f, 159f / 255f), false, false, true);
                        found = true;
                    }
                    if (i == "Nullify")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), DamageColor.FindColor(DamageColorIndex.Void), false, false, true);
                        found = true;
                    }
                    if (i == "BonusToLowHealth")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), DamageColor.FindColor(DamageColorIndex.Fragile), false, false, true);
                        found = true;
                    }
                    if (i == "BlightOnHit")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), new UnityEngine.Color(222f / 255f, 85f / 255f, 230f / 255f), false, false, true);
                        found = true;
                    }
                    if (i == "CrippleOnHit")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), new UnityEngine.Color(48f / 255f, 205f / 255f, 217f / 255f), false, false, true);
                        found = true;
                    }
                    if (i == "SuperBleedOnCrit")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), DamageColor.FindColor(DamageColorIndex.SuperBleed), false, false, true);
                        found = true;
                    }
                    if (i == "FruitOnHit")
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), new UnityEngine.Color(255f / 255f, 191f / 255f, 225f / 255f), false, false, true);
                        found = true;
                    }
                    if (found != true)
                    {
                        Buffs.AddNewBuff("RobDriverBulletsBuff" + i, Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSniperBulletIndicator"), UnityEngine.Color.white, false, false, true);
                    }
                    count++;
                }
            }
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff, bool bullet = false)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            if (bullet) bulletDefs.Add(buffDef);
            else buffDefs.Add(buffDef);

            return buffDef;
        }
    }
}