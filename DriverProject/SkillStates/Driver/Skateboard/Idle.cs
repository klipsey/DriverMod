using UnityEngine;
using RoR2;
using EntityStates;

namespace RobDriver.SkillStates.Driver.Skateboard
{
    public class Idle : BaseDriverSkillState
    {
        protected override bool hideGun => true;
        protected override string prop => "SkateboardModel";

        private float skateSpeedMultiplier = 0.8f;
        private Vector3 idealDirection;
        private uint skatePlayID = 0u;
        private bool isSprinting;

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority)
            {
                // this sucks but it works
                if (this.inputBank.skill1.down || this.inputBank.skill2.down || this.inputBank.skill4.down ||
                    (this.inputBank.skill3.justPressed && this.skillLocator.utility.stock > 0))
                {
                    outer.SetNextState(new Stop());
                    return;
                }

                if (this.inputBank.sprint.justPressed)
                {
                    this.isSprinting = !this.isSprinting;
                }

                this.UpdateSkateDirection();

                if (base.characterDirection)
                {
                    base.characterDirection.moveVector = this.idealDirection;
                    if (base.characterMotor && !(base.characterMotor.disableAirControlUntilCollision))
                    {
                        base.characterMotor.rootMotion += this.GetIdealVelocity() * Time.fixedDeltaTime;
                    }
                }
            }

            bool wasSprinting = this.characterBody.isSprinting;

            this.characterBody.isSprinting = this.isSprinting;

            if (!wasSprinting && this.isSprinting) base.PlayAnimation("FullBody, Override", "SkateAccelerate");

            //sound
            if (base.isGrounded)
            {
                if (this.skatePlayID == 0u)
                {
                    Util.PlaySound("Landing", this.gameObject);
                    this.skatePlayID = Util.PlaySound("rollconcrete02", this.gameObject);
                }

                AkSoundEngine.SetRTPCValue("Skateboard_Speed", Util.Remap(base.characterMotor.velocity.magnitude, 7f, 60f, 1f, 4f));
            }
            else
            {
                if (this.skatePlayID != 0u)
                {
                    if (base.characterMotor.velocity.y >= 0.1f) Util.PlaySound("Ollie", this.gameObject);
                    AkSoundEngine.StopPlayingID(this.skatePlayID);
                    this.skatePlayID = 0u;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.skatePlayID != 0u)
            {
                AkSoundEngine.StopPlayingID(this.skatePlayID);
                this.skatePlayID = 0u;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        private void UpdateSkateDirection()
        {
            if (base.inputBank)
            {
                Vector2 vector = Util.Vector3XZToVector2XY(base.inputBank.moveVector);
                if (vector != Vector2.zero)
                {
                    vector.Normalize();
                    this.idealDirection = new Vector3(vector.x, 0f, vector.y).normalized;
                }
            }
        }

        private Vector3 GetIdealVelocity()
        {
            return base.characterDirection.forward * base.characterBody.moveSpeed * this.skateSpeedMultiplier;
        }
    }
}