using RoR2;
using System.Linq;
using UnityEngine;

namespace RobDriver.Modules.Components
{
    public class DriverArsenal : MonoBehaviour
    {
        public GenericSkill weaponSkillSlot;

        public DriverWeaponDef weaponDef;

        public DriverWeaponDef DefaultWeapon
        {
            get
            {
                if (!this.weaponSkillSlot) return DriverWeaponCatalog.Pistol;
                // what the fuck was i smoking, this is hideous
                if (this.weaponSkillSlot?.skillDef is null) this.weaponDef = DriverWeaponCatalog.Pistol;
                else if (this.weaponDef is null) this.weaponDef = DriverWeaponCatalog.weaponDefs.FirstOrDefault(def =>
                        def.name == this.weaponSkillSlot.skillDef.skillName) ?? DriverWeaponCatalog.Pistol;

                return this.weaponDef;
            }
        }
    }
}