using System;

namespace DriverMod.Modules.Misc
{
    internal static class DriverWeaponSkinCatalog
    {
        internal static DriverWeaponSkinDef[][] skinDefs = new DriverWeaponSkinDef[0][];

        internal static void AddSkin(DriverWeaponSkinDef[] skinDef)
        {
            Log.Debug("Added ");
            Array.Resize(ref skinDefs, skinDefs.Length + 1);

            int index = skinDefs.Length - 1;
            skinDefs[index] = skinDef;
        }

        internal static DriverWeaponSkinDef[] GetSkin(int index)
        {
            Log.Debug("SkinDef Length" + skinDefs.Length);
            return skinDefs[index];
        }
    }
}