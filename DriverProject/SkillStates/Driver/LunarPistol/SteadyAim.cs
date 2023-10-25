using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.Driver.LunarPistol
{
    public class SteadyAim : Driver.SteadyAim
    {
        protected override float _damageCoefficient => 9f;

        protected override GameObject tracerPrefab => Modules.Assets.lunarTracer;
    }
}