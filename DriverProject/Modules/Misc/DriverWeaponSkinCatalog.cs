using System;
using System.Collections.Generic;

namespace DriverMod.Modules.Misc
{
    internal static class DriverWeaponSkinCatalog
    {
        internal static List<DriverWeaponSkinDef[]> driverSkinDefs { get; set; } = new List<DriverWeaponSkinDef[]>();
        internal static DriverWeaponSkinDef[] Default => driverSkinDefs[0];
        internal static void AddSkin(DriverWeaponSkinDef[] skinDef)
        {
            driverSkinDefs.Add(skinDef);
        }

        internal static DriverWeaponSkinDef[] GetSkin(int index)
        {
            return driverSkinDefs[index];
        }
    }
}