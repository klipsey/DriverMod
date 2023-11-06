using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobDriver
{
    internal static class DriverWeaponCatalog
    {
        internal static DriverWeaponDef[] weaponDefs = new DriverWeaponDef[0];

        internal static DriverWeaponDef Pistol;
        internal static DriverWeaponDef PyriteGun;
        internal static DriverWeaponDef GoldenGun;
        internal static DriverWeaponDef PrototypeRocketLauncher;
        internal static DriverWeaponDef ArmCannon;
        internal static DriverWeaponDef PlasmaCannon;
        internal static DriverWeaponDef Behemoth;
        internal static DriverWeaponDef BeetleShield;
        internal static DriverWeaponDef LunarPistol;
        internal static DriverWeaponDef VoidPistol;
        internal static DriverWeaponDef Needler;
        internal static DriverWeaponDef GolemRifle;
        internal static DriverWeaponDef LunarRifle;
        internal static DriverWeaponDef LunarHammer;
        internal static DriverWeaponDef NemmandoGun;
        internal static DriverWeaponDef NemmercGun;

        internal static void AddWeapon(DriverWeaponDef weaponDef)
        {
            Array.Resize(ref weaponDefs, weaponDefs.Length + 1);

            int index = weaponDefs.Length - 1;
            weaponDef.index = (ushort)index;

            weaponDefs[index] = weaponDef;
            weaponDef.index = (ushort)index;

            // heheheha
            weaponDef.pickupPrefab = Modules.Assets.CreatePickupObject(weaponDef);

            // add config
            Modules.Config.InitWeaponConfig(weaponDef);
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
                if (weaponDefs[i] && weaponDefs[i].tier == tier)
                {
                    if (Modules.Config.GetWeaponConfigEnabled(weaponDefs[i])) validWeapons.Add(weaponDefs[i]);
                }
            }

            DriverWeaponDef[] _validWeapons = validWeapons.ToArray();

            if (_validWeapons.Length <= 0) return weaponDefs[0]; // pistol failsafe if you disabled rocket launcher like a fucking retard or something

            return _validWeapons[UnityEngine.Random.Range(0, _validWeapons.Length)];
        }
    }
}