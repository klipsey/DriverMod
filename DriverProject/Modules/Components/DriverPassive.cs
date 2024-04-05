using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace RobDriver.Modules.Components
{
    public class DriverPassive : MonoBehaviour
    {
        public SkillDef defaultPassive;
        public SkillDef pistolOnlyPassive;
        public SkillDef bulletsPassive;
        public SkillDef godslingPassive;
        public GenericSkill passiveSkillSlot;

        public bool isDefault
        {
            get
            {
                if (this.defaultPassive && this.passiveSkillSlot)
                {
                    return this.passiveSkillSlot.skillDef == this.defaultPassive;
                }

                return false;
            }
        }

        public bool isPistolOnly
        {
            get
            {
                if (this.pistolOnlyPassive && this.passiveSkillSlot)
                {
                    return this.passiveSkillSlot.skillDef == this.pistolOnlyPassive;
                }

                return false;
            }
        }


        public bool isBullets
        {
            get
            {
                if (this.bulletsPassive && this.passiveSkillSlot)
                {
                    return this.passiveSkillSlot.skillDef == this.bulletsPassive;
                }
                return false;
            }
        }

        public bool isRyan
        {
            get
            {
                if (this.godslingPassive && this.passiveSkillSlot)
                {
                    return this.passiveSkillSlot.skillDef == this.godslingPassive;
                }
                return false;
            }
        }
    }
}