using UnityEngine;

namespace RobDriver.Modules.Components
{
    public class DriverWeaponTracker : MonoBehaviour
    {
        public struct StoredWeapon
        {
            public DriverWeaponDef weaponDef;
            public float ammo;
        };

        public bool isStoringWeapon;

        public DriverWeaponDef storedWeapon;
        public float storedAmmo;

        public void StoreWeapon(DriverWeaponDef weaponDef, float ammo)
        {
            this.isStoringWeapon = true;
            this.storedWeapon = weaponDef;
            this.storedAmmo = ammo;
        }

        public StoredWeapon RetrieveWeapon()
        {
            this.isStoringWeapon = false;
            return new StoredWeapon
            {
                weaponDef = this.storedWeapon,
                ammo = this.storedAmmo
            };
        }
    }
}