using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RobDriver.Modules.Components
{
    public class DriverArsenal : MonoBehaviour
    {
        public GenericSkill weaponSkillSlot;

        private DriverWeaponDef weaponDef;

        public DriverWeaponDef DefaultWeapon
        {
            get
            {
                if (this.weaponSkillSlot?.skillDef is null) this.weaponDef = DriverWeaponCatalog.Pistol;
                else if (this.weaponDef is null) this.weaponDef = DriverWeaponCatalog.weaponDefs.FirstOrDefault(def =>
                        def.name == this.weaponSkillSlot.skillDef.skillName) ?? DriverWeaponCatalog.Pistol;
                
                return this.weaponDef;
            }
        }

        public void UnsetDefaultWeapon()
        {
            weaponDef = null;
        }
    }
}