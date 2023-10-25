using System;
using System.Collections.Generic;
using System.Text;

namespace RobDriver.SkillStates.Driver.RocketLauncher
{
    public class NerfedBarrage : Barrage
    {
        protected override float _damageCoefficient => 2.5f;
        protected override float maxSpread => 8f;
        protected override int baseRocketCount => 4;
    }
}
