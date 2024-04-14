using UnityEngine;

namespace RobDriver.Modules.Components
{
    public class DriverWeaponTracker : MonoBehaviour
    {
        public struct StoredWeapon
        {
            public DriverWeaponDef weaponDef;
            public DriverBulletDef bulletDef;
            public float ammo;
            public short ammoIndex;
        };

        public bool isStoringWeapon;

        public DriverWeaponDef storedWeapon;
        public float storedAmmo;
        public short storedAmmoIndex;

        public void StoreWeapon(DriverWeaponDef weaponDef, float ammo, short ammoIndex)
        {
            this.isStoringWeapon = true;
            this.storedWeapon = weaponDef;
            this.storedAmmo = ammo;
            this.storedAmmoIndex = ammoIndex;
        }

        public StoredWeapon RetrieveWeapon()
        {
            this.isStoringWeapon = false;
            return new StoredWeapon
            {
                weaponDef = this.storedWeapon,
                ammo = this.storedAmmo,
                ammoIndex = this.storedAmmoIndex
            };
        }
    }
}