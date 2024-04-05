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
        internal static BuffDef syringeNewBuff;

        internal static void RegisterBuffs()
        {
            dazedDebuff = AddNewBuff("RobDriverDazedDebuff", Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdCloak.asset").WaitForCompletion().iconSprite, Color.grey, false, false);
            woundDebuff = AddNewBuff("RobDriverWoundDebuff", Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Bandit2/bdBanditSkull.asset").WaitForCompletion().iconSprite, Color.red, false, true);
            syringeDamageBuff = AddNewBuff("RobDriverSyringeDamageBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffSyringe"), new Color(1f, 70f / 255f, 75f / 255f), false, false);
            syringeAttackSpeedBuff = AddNewBuff("RobDriverSyringeAttackSpeedBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffSyringe"), new Color(1f, 170f / 255f, 45f / 255f), false, false);
            syringeCritBuff = AddNewBuff("RobDriverSyringeCritBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffSyringe"), new Color(1f, 80f / 255f, 17f / 255f), false, false);
            syringeNewBuff = AddNewBuff("RobDriverSyringeNewBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffSyringe"), new Color(1f, 70f / 255f, 75f / 255f), false, false);
            syringeScepterBuff = AddNewBuff("RobDriverSyringeScepterBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffSyringe"), Modules.Survivors.Driver.characterColor, false, false);
            InitBulletsWithBuffs();
        }

        internal static void InitBulletsWithBuffs()
        {
            foreach (DamageTypes.DriverBulletInfo i in DamageTypes.bulletTypes)
            {
                if(i.bulletType != DamageType.Generic) Buffs.AddNewBuff(i.nameToken, i.icon, i.trailColor, false, false, true);
                else Buffs.AddNewBuff(i.nameToken, i.icon, i.trailColor, false, false, true);
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