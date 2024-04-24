using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RobDriver.SkillStates.BaseStates;

namespace RobDriver.SkillStates.Driver.Compat
{
    public class ChargeBlink : ChargeJump
    {
        private GameObject predictionEffectInstance;

        public override void OnEnter()
        {
            duration = 0.6f;
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();

            if (predictionEffectInstance)
            {
                foreach (ParticleSystem i in predictionEffectInstance.GetComponentsInChildren<ParticleSystem>())
                {
                    if (i) i.Stop();
                }

                Object.Destroy(predictionEffectInstance, 5f);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.characterBody.isSprinting = true;

            if (predictionEffectInstance)
            {
                Ray aimRay = this.GetAimRay();

                float movespeed = Mathf.Clamp(this.characterBody.moveSpeed, 1f, 18f);
                float charge = Mathf.Clamp01(Util.Remap(base.fixedAge, 0f, duration, 0f, 1f));
                float fakeDuration = 0.35f;
                float fakeJumpForce = jumpForce = Util.Remap(charge, 0f, 1f, 0.17733990147f, 0.37334975369f) * this.characterBody.jumpPower * movespeed;

                Vector3 predictedPos = this.transform.position + aimRay.direction * (jumpForce * 1.5f * fakeDuration);
                predictionEffectInstance.transform.position = predictedPos;
            }
        }

        protected override void PlayAnim()
        {
            base.PlayCrossfade("Body", "BlinkCharge", "Jump.playbackRate", duration, 0.1f);
        }

        protected override void SetJumpTime()
        {
            jumpTime = 0.5f;

            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(jumpDir);
            effectData.origin = Util.GetCorePosition(this.gameObject);
            EffectManager.SpawnEffect(EntityStates.ImpMonster.BlinkState.blinkPrefab, effectData, false);
        }

        protected override void NextState()
        {
            this.outer.SetNextState(new BlinkBig
            {
                jumpDir = jumpDir,
                jumpForce = jumpForce * 1.2f
            });
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}