using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace RobDriver.Modules.Components
{
    public class DriverPassive : MonoBehaviour
    {
        public SkillDef defaultPassive;
        public SkillDef pistolOnlyPassive;
        public SkillDef isBulletsPassive;
        public GenericSkill passiveSkillSlot;

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
                if (this.isBulletsPassive && this.passiveSkillSlot)
                {
                    return this.passiveSkillSlot.skillDef == this.isBulletsPassive;
                }
                return false;
            }
        }
    }
}