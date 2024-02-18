using UnityEngine;
using EntityStates;
using RoR2;
using UnityEngine.Networking;

namespace RobDriver.SkillStates.Driver
{
    public class Heal : BaseDriverSkillState
    {
        public float startupDelay = 0.65f;
        public float healPercentPerTick = 0.05f;
        public float tickStopwatch = 0.25f;

        protected override string prop => "MedkitModel";
        protected override bool hideGun => true;

        private float stopwatch;

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.attackSpeedStat = this.characterBody.attackSpeed;

            if (base.fixedAge >= (this.startupDelay / this.attackSpeedStat))
            {
                this.HandleHeal();
            }

            if (base.isAuthority)
            {
                if (!this.isGrounded) this.outer.SetNextStateToMain();

                if (this.healthComponent.health >= this.healthComponent.fullHealth || !this.inputBank.skill4.down)
                {
                    this.outer.SetNextStateToMain();
                }
            }
        }

        protected virtual void HandleHeal()
        {
            this.stopwatch += Time.fixedDeltaTime;

            if (this.stopwatch >= (this.tickStopwatch / this.attackSpeedStat))
            {
                this.TryHeal();
            }
        }

        protected virtual void TryHeal()
        {
            if (NetworkServer.active)
            {
                this.healthComponent.Heal(this.healthComponent.fullHealth * this.healPercentPerTick, default(ProcChainMask));
            }
        }
    }
}