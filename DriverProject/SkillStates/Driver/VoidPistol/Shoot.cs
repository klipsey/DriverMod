using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.Driver.VoidPistol
{
    public class Shoot : Driver.Shoot
    {
        protected override float _damageCoefficient => 3.5f;
        protected override GameObject tracerPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBeamTracer.prefab").WaitForCompletion();
        public override BulletAttack.FalloffModel falloff => BulletAttack.FalloffModel.None;
    }
}