using System;
using System.Collections.Generic;

namespace DriverMod.Modules.Misc
{
    internal static class DriverWeaponSkinCatalog
    {
        internal static List<DriverWeaponSkinDef[]> driverSkinDefs { get; set; } = new List<DriverWeaponSkinDef[]>();
        internal static DriverWeaponSkinDef[] Default { get; private set; }
        internal static void AddSkin(DriverWeaponSkinDef[] skinDef)
        {
            if (driverSkinDefs.Count == 0) Default = skinDef;
            driverSkinDefs.Add(skinDef);
        }

        internal static DriverWeaponSkinDef[] GetSkin(int index)
        {
            return driverSkinDefs[index];
        }
    }
}