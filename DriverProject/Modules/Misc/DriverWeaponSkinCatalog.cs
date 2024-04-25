using RobDriver;
using System.Collections.Generic;
using System.Linq;

namespace DriverMod.Modules.Misc
{
    internal static class DriverWeaponSkinCatalog
    {
        internal static Dictionary<string, DriverWeaponSkinDef[]> driverSkinDefs { get; private set; } = new Dictionary<string, DriverWeaponSkinDef[]>();

        internal static void AddSkin(string name, DriverWeaponSkinDef[] skinDef)
        {
            driverSkinDefs.Add(name, skinDef);
        }

        internal static DriverWeaponSkinDef[] GetSkin(int index)
        {
            return driverSkinDefs.Values.ElementAtOrDefault(index);
        }
    }
}