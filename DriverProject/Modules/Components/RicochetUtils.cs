using System.Collections.Generic;
using RobDriver.Modules.Components;
using RoR2;

namespace RobDriver.Modules.Components
{
    public static class RicochetUtils
    {
        public enum RicochetPriority
        {
            None,
            StunnedBody,
            Body,
            Projectile,
            Explosive,
            Rocket,
            Bike,
            Coin
        }
    }
}