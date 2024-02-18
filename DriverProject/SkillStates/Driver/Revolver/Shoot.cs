using UnityEngine;
using RoR2;
using EntityStates;

namespace RobDriver.SkillStates.Driver.Revolver
{
    public class Shoot : SkillStates.Driver.Shoot
    {
        protected override float _damageCoefficient => 3.2f;
        public override string shootSoundString => "sfx_driver_pistol_shoot_charged";

        public override void OnEnter()
        {
            base.OnEnter();
            if (this.iDrive) this.iDrive.StartTimer();
        }
    }
}