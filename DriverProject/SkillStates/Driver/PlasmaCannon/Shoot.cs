using UnityEngine;

namespace RobDriver.SkillStates.Driver.PlasmaCannon
{
    public class Shoot : RocketLauncher.Shoot
    {
        protected override float _damageCoefficient => 14f;
        protected override GameObject projectilePrefab => Modules.Projectiles.plasmaCannonProjectilePrefab;
    }
}
