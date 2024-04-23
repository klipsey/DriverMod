using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.BaseStates
{
    public class BlinkBig : WallJumpBig
    {
        private Transform modelTransform;
        private CharacterModel characterModel;

        public override void OnEnter()
        {
            duration = 0.35f;
            base.OnEnter();
            modelTransform = this.GetModelTransform();

            base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
        }

        public override void OnExit()
        {
            base.gameObject.layer = LayerIndex.defaultLayer.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();

            base.PlayAnimation("FullBody, Override Soft", "Blink");
            if (DriverPlugin.ravagerInstalled) Util.PlaySound("sfx_ravager_sonido", this.gameObject);

            base.OnExit();

            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(-jumpDir);
            effectData.origin = Util.GetCorePosition(this.gameObject);
            EffectManager.SpawnEffect(EntityStates.ImpMonster.BlinkState.blinkPrefab, effectData, false);

            if (!this.isGrounded) this.characterMotor.velocity = Vector3.up * 10f;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}