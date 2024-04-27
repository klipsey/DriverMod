using UnityEngine;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using EntityStates;
using RobDriver.Modules;
using RobDriver.SkillStates.BaseStates;
using UnityEngine.UI;

namespace RobDriver.SkillStates.Driver.Compat
{
    public class WallJump : BaseDriverState
    {
        private float airTime;

        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(this.iDrive.weaponDef.nameToken == "ROB_DRIVER_WEAPON_RAV_SWORD_NAME")
            {
                if (this.isGrounded)
                {
                    this.ravController.blinkReady = true;
                    this.airTime = 0f;
                    this.ravController.wallJumpCounter = 0;
                }
                else this.airTime += Time.fixedDeltaTime;

                if (this.inputBank.jump.justPressed && !this.isGrounded && base.isAuthority)
                {
                    if (this.airTime >= 0.15f)
                    {
                        if (this.ravController.blinkReady)
                        {
                            // hopoo feather interaction
                            if (this.ravController.hopoFeatherTimer > 0f)
                            {
                                EntityStateMachine.FindByCustomName(this.gameObject, "Body").SetInterruptState(new ChargeBlink
                                {
                                    hopoo = true
                                }, InterruptPriority.Any);

                                return;
                            }

                            this.ravController.blinkReady = false;

                            EntityStateMachine.FindByCustomName(this.gameObject, "Body").SetInterruptState(new ChargeBlink(), InterruptPriority.Any);

                            return;
                        }

                        if (this.AttemptEnemyStep())
                        {
                            base.PlayAnimation("Body", "JumpEnemy");
                            Util.PlaySound("sfx_ravager_enemystep", this.gameObject);
                            GenericCharacterMain.ApplyJumpVelocity(base.characterMotor, base.characterBody, 1.5f, 1.5f, false);
                            return;
                        }
                    }
                }
                return;
            }
        }

        private bool AttemptEnemyStep()
        {
            SphereSearch s = new SphereSearch()
            {
                origin = this.transform.position,
                radius = 5f,
                mask = LayerIndex.entityPrecise.mask
            }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.GetTeam()));
            s.searchData.FilterByHurtBoxHealthComponents();
            return s.GetHurtBoxes().Any();
        }
    }
}