using System;
using System.Collections.Generic;
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

        internal static DriverWeaponDef GetRandomWeaponFromTier(DriverWeaponTier tier)
        {
            List<DriverWeaponDef> validWeapons = new List<DriverWeaponDef>();

            for (int i = 0; i < weaponDefs.Length; i++)
            {
                if (weaponDefs[i] && weaponDefs[i].tier == tier) validWeapons.Add(weaponDefs[i]);
            }

            DriverWeaponDef[] _validWeapons = validWeapons.ToArray();

            if (_validWeapons.Length <= 0) return weaponDefs[0]; // pistol failsafe if you disabled rocket launcher like a fucking retard or something

            return _validWeapons[UnityEngine.Random.Range(0, _validWeapons.Length)];
        }
    }
}