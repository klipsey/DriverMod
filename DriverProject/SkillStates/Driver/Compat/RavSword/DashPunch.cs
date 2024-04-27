using System;
using System.Collections.Generic;
using System.Linq;
using EntityStates;
using EntityStates.Commando;
using RoR2;
using RoR2.Audio;
using RoR2.Projectile;
using UnityEngine;
using RobDriver.Modules;
using UnityEngine.Networking;
using static RoR2.CameraTargetParams;

namespace RobDriver.SkillStates.Driver.Compat
{
    public class DashPunch : BaseDriverSkillState
    {
        protected AnimationCurve dashSpeedCurve;
        private float grabDuration = 0.5f;

        private float windupDuration = 0.2f;

        protected GameObject swingEffectPrefab;
        protected GameObject hitEffectPrefab;
        protected NetworkSoundEventIndex impactSound;
        public static float punchDamageCoefficient = 12.5f;
        private Vector3 aimDirection;
        private float stopwatch;
        private float grabRadius = 8f;
        private SubState subState;

        protected virtual string startAnimString => "DashPunchStart";
        protected virtual string dashAnimString => "DashPunch";

        private enum SubState
        {
            Windup,
            DashGrab,
            MissedGrab
        }

        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();
            aimDirection = GetAimRay().direction;
            aimDirection.y = Mathf.Clamp(aimDirection.y, -0.75f, 0.75f);
            stopwatch = 0f;

            base.PlayAnimation("FullBody, Override Soft", "BufferEmpty");
            PlayAnimation("FullBody, Override", startAnimString, "Grab.playbackRate", windupDuration);

            if(DriverPlugin.ravagerInstalled) Util.PlaySound("sfx_ravager_shine", gameObject);

            characterMotor.velocity *= 0.1f;

            dashSpeedCurve = new AnimationCurve(new Keyframe[]
            {
                new Keyframe(0f, 14f),
                new Keyframe(0.8f, 0f),
                new Keyframe(1f, 0f)
            });

            subState = SubState.Windup;


            GetModelAnimator().SetFloat("leapDir", inputBank.aimDirection.y);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (subState == SubState.Windup)
            {
                characterBody.isSprinting = false;

                if (stopwatch >= windupDuration)
                {
                    stopwatch = 0f;
                    subState = SubState.DashGrab;
                    PlayAnimation("FullBody, Override", dashAnimString, "Grab.playbackRate", grabDuration * 1.25f);
                    if(DriverPlugin.ravagerInstalled) Util.PlaySound("sfx_ravager_lunge", gameObject);
                    else Util.PlaySound("sfx_driver_dodge", this.gameObject);
                }

                characterMotor.velocity.y = 0f;
            }
            else
            {
                if (subState == SubState.DashGrab)
                {
                    characterBody.isSprinting = true;

                    float num = dashSpeedCurve.Evaluate(stopwatch / grabDuration);
                    characterMotor.rootMotion += aimDirection * (num * moveSpeedStat * Time.fixedDeltaTime);
                    characterMotor.velocity.y = 0f;

                    AttemptGrab(grabRadius);

                    if (stopwatch >= grabDuration)
                    {
                        stopwatch = 0f;
                        outer.SetNextStateToMain();
                        subState = SubState.MissedGrab;
                    }
                }
            }
        }


        public override void OnExit()
        {
            base.OnExit();
        }

        public void AttemptGrab(float grabRadius)
        {
            Ray aimRay = GetAimRay();

            BullseyeSearch bullseyeSearch = new BullseyeSearch
            {
                teamMaskFilter = TeamMask.GetEnemyTeams(GetTeam()),
                filterByLoS = false,
                searchOrigin = transform.position,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = grabRadius,
                maxAngleFilter = 360f
            };
            bullseyeSearch.RefreshCandidates();
            bullseyeSearch.FilterOutGameObject(gameObject);
            bullseyeSearch.FilterCandidatesByHealthFraction();
            var hurtBox = bullseyeSearch.GetResults().Where(Util.IsValid).FirstOrDefault();

            if (hurtBox)
            {
                this.ravController.RefreshBlink();
                EffectManager.SpawnEffect(Modules.Assets.bloodExplosionEffect, new EffectData
                {
                    origin = hurtBox.transform.position,
                    scale = 2f
                }, false);

                if (DriverPlugin.ravagerInstalled)
                {
                    Util.PlaySound("sfx_ravager_punch", gameObject);
                    Util.PlaySound("sfx_ravager_punch_generic", hurtBox.gameObject);
                }
                else
                {
                    Util.PlaySound("Play_loader_shift_release", gameObject);
                    Util.PlaySound("sfx_driver_impact_hammer", hurtBox.gameObject);
                }

                if (isAuthority)
                {
                    float dmg = punchDamageCoefficient * damageStat;
                    //if (this.empowered) dmg *= 2f;

                    float force = 4000f;
                    if (hurtBox.healthComponent.body.isChampion) force = 24000f;

                    // damage
                    BlastAttack.Result result = new BlastAttack
                    {
                        attacker = gameObject,
                        procChainMask = default,
                        impactEffect = EffectIndex.Invalid,
                        losType = BlastAttack.LoSType.None,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Stun1s | DamageType.NonLethal,
                        procCoefficient = 1f,
                        bonusForce = GetAimRay().direction.normalized * force,
                        baseForce = 0f,
                        baseDamage = dmg,
                        falloffModel = BlastAttack.FalloffModel.None,
                        radius = 0.4f,
                        position = hurtBox.transform.position,
                        attackerFiltering = AttackerFiltering.NeverHitSelf,
                        teamIndex = GetTeam(),
                        inflictor = gameObject,
                        crit = RollCrit()
                    }.Fire();

                    // shockwave
                    FireProjectileInfo fireProjectileInfo = default;
                    fireProjectileInfo.position = hurtBox.transform.position + aimRay.direction * -4f;
                    fireProjectileInfo.rotation = Quaternion.LookRotation(aimRay.direction);
                    fireProjectileInfo.crit = RollCrit();
                    fireProjectileInfo.damage = 10f * damageStat;
                    fireProjectileInfo.owner = gameObject;
                    fireProjectileInfo.projectilePrefab = Modules.Projectiles.punchShockwave;
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);

                    outer.SetNextState(new PunchRecoil());
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}