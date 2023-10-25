using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.Driver.VoidPistol
{
    public class SteadyAim : Driver.SteadyAim
    {
        protected override float _damageCoefficient => 9f;

        protected override GameObject tracerPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBeamTracer.prefab").WaitForCompletion();
    }
}