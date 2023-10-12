using System;
using UnityEngine;

namespace RobDriver
{
    internal static class DriverWeaponCatalog
    {
        internal static DriverWeaponDef[] weaponDefs = new DriverWeaponDef[0];

        internal static void AddWeapon(DriverWeaponDef weaponDef)
        {
            Array.Resize(ref weaponDefs, weaponDefs.Length + 1);

            int index = weaponDefs.Length - 1;
            weaponDef.index = (ushort)index;

            weaponDefs[index] = weaponDef;
        }

        internal static DriverWeaponDef GetWeaponFromIndex(int index)
        {
            return weaponDefs[index];
        }
    }
}