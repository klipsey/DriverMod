using UnityEngine;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using EntityStates;
using RobDriver.Modules;
using RobDriver.SkillStates.BaseStates;

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
            BullseyeSearch2 bullseyeSearch = new BullseyeSearch2
            {
                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = this.transform.position + (Vector3.up * 0.5f),
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch2.SortMode.Distance,
                onlyBullseyes = false,
                maxDistanceFilter = 5f,
                maxAngleFilter = 360f
            };

            bullseyeSearch.RefreshCandidates();
            bullseyeSearch.FilterOutGameObject(base.gameObject);
            List<HurtBox> list = bullseyeSearch.GetResults().ToList<HurtBox>();
            foreach (HurtBox hurtBox in list)
            {
                if (hurtBox)
                {
                    if (hurtBox.healthComponent && hurtBox.healthComponent.body)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}