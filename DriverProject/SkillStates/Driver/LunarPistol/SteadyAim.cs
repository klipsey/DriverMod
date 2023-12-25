using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.Driver.LunarPistol
{
    public class SteadyAim : Driver.SteadyAim
    {
        protected override float _damageCoefficient => 9f;
        protected override GameObject tracerPrefab
        {
            get
            {
                if (this.lastCharge) return Modules.Assets.chargedLunarTracer;
                return Modules.Assets.lunarTracer;
            }
        }
        protected override bool isPiercing => true;
    }
}