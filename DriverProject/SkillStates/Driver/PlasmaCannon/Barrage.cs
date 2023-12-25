using UnityEngine;

namespace RobDriver.SkillStates.Driver.PlasmaCannon
{
    public class Barrage : RocketLauncher.Barrage
    {
        protected override float _damageCoefficient => 10f;
        protected override GameObject projectilePrefab => Modules.Projectiles.plasmaCannonProjectilePrefab;
        protected override float ammoMod => 2f;
    }
}
