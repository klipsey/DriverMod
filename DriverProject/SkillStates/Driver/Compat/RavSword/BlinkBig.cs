using EntityStates;
using RoR2;
using UnityEngine;
using RobDriver.SkillStates.BaseStates;

namespace RobDriver.SkillStates.Driver.Compat
{
    public class BlinkBig : WallJumpBig
    {
        public override void OnEnter()
        {
            duration = 0.35f;
            base.OnEnter();

            base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
        }

        public override void OnExit()
        {
            base.gameObject.layer = LayerIndex.defaultLayer.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();

            base.PlayAnimation("FullBody, Override Soft", "Blink");
            if (DriverPlugin.ravagerInstalled) Util.PlaySound("sfx_ravager_sonido", this.gameObject);
            else Util.PlaySound("sfx_driver_bazooka_shoot", this.gameObject);
            base.OnExit();

            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(-jumpDir);
            effectData.origin = Util.GetCorePosition(this.gameObject);
            EffectManager.SpawnEffect(EntityStates.ImpMonster.BlinkState.blinkPrefab, effectData, false);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}