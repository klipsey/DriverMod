using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.Driver.Compat.NemmandoGun
{
    public class Shoot : Driver.Shoot
    {
        protected override float _damageCoefficient => 3.8f;
        //protected override GameObject tracerPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBeamTracer.prefab").WaitForCompletion();
        public override BulletAttack.FalloffModel falloff => BulletAttack.FalloffModel.None;
        public override string shootSoundString => DriverPlugin.starstormInstalled ? "NemmandoShoot" : "sfx_driver_pistol_shoot_charged";
    }
}