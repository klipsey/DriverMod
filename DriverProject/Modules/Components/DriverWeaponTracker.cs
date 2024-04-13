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

        public DriverWeaponDef storedWeapon;
        public DriverBulletDef storedBullet;
        public float storedAmmo;

        public void StoreWeapon(DriverWeaponDef weaponDef, DriverBulletDef bulletDef, float ammo)
        {
            this.isStoringWeapon = true;
            this.storedWeapon = weaponDef;
            this.storedBullet = bulletDef;
            this.storedAmmo = ammo;
        }

        public StoredWeapon RetrieveWeapon()
        {
            this.isStoringWeapon = false;
            return new StoredWeapon
            {
                weaponDef = this.storedWeapon,
                bulletDef = this.storedBullet,
                ammo = this.storedAmmo
            };
        }
    }
}