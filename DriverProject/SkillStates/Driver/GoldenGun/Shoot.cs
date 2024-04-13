using UnityEngine;
using RoR2;
using EntityStates;

namespace RobDriver.SkillStates.Driver.GoldenGun
{
    public class Shoot : SkillStates.Driver.Shoot
    {
        protected override float _damageCoefficient => 3.9f;
        public override string shootSoundString => "sfx_driver_pistol_shoot_charged";
        public override BulletAttack.FalloffModel falloff => BulletAttack.FalloffModel.None;
    }
}