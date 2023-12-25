using UnityEngine;

namespace RobDriver.SkillStates.Driver.PlasmaCannon
{
    public class Shoot : RocketLauncher.Shoot
    {
        protected override float _damageCoefficient => 14f;
        protected override GameObject projectilePrefab => Modules.Projectiles.plasmaCannonProjectilePrefab;
        protected override string soundString => "sfx_driver_plasma_cannon_shoot";
        protected override float ammoMod => 4f;
    }
}
