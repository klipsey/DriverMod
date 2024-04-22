using RobDriver;
using System;
using Unity;
using UnityEngine;
using System.Collections.Generic;
using static DriverWeaponSkinDef;
using System.Reflection;

namespace DriverMod.Modules.Misc
{
    internal static class DriverWeaponSkinCatalog
    {
        internal static Dictionary<string, DriverWeaponSkinDef[]> driverSkinDefs { get; private set; } = new Dictionary<string, DriverWeaponSkinDef[]>();
        //To convert skincontrollerindex to weaponskin
        internal static Dictionary<int, String> mainSkinIndexes = new Dictionary<int, String>();
        internal static DriverWeaponSkinDef badAssSwordDef { get; private set; }
        internal static void AddSkin(string name, DriverWeaponSkinDef[] skinDef)
        {
            if (name == "NemCmdoSamDef" && RobDriver.Modules.Config.enabledRedVfxForKnife.Value)
            {
                //minuano sword
                badAssSwordDef = skinDef[1];
            }
            driverSkinDefs.Add(name, skinDef);
        }

        internal static void AddSkinIndex(int index, string name)
        {
            mainSkinIndexes.Add(index, name);
        }

        internal static DriverWeaponSkinDef[] GetSkin(int index)
        {
            String skinName = mainSkinIndexes[index];

            if (driverSkinDefs.ContainsKey(skinName)) return driverSkinDefs[skinName];
            else return new DriverWeaponSkinDef[0]; 
        }
    }
}