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
        };

        public bool isStoringWeapon;

        public DriverWeaponDef storedWeaponDef;
        public DriverBulletDef storedBulletDef;
        public float storedAmmo;

        public void StoreWeapon(DriverWeaponDef weaponDef, DriverBulletDef bulletDef, float ammo)
        {
            this.isStoringWeapon = true;
            this.storedWeaponDef = weaponDef;
            this.storedBulletDef = bulletDef;
            this.storedAmmo = ammo;
        }

        public StoredWeapon RetrieveWeapon()
        {
            this.isStoringWeapon = false;
            return new StoredWeapon
            {
                weaponDef = this.storedWeaponDef,
                bulletDef = this.storedBulletDef,
                ammo = this.storedAmmo
            };
        }
    }
}