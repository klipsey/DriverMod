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
    public class DashGrab : BaseDriverSkillState
    {
        private float finalAirTime;
        private Vector3 lastSafeFootPosition;
        private float airTimeDamageCoefficient = 1.5f;
        private GameObject fireEffect;
        private GameObject dragEffect;
        private float minDropTime = 0.35f;
        private float attackRecoil = 7f;
        protected AnimationCurve dashSpeedCurve;
        protected AnimationCurve dragSpeedCurve;
        protected AnimationCurve dropSpeedCurve;
        private float grabDuration = 0.5f;
        private Vector3 targetMoveVector;
        private Vector3 targetMoveVectorVelocity;
        private bool wasGrounded;
        public static float upForce = 800f;
        public static float launchForce = 1200f;
        public static float throwForce = 12000f;
        public static float turnSmoothTime = 0.01f;
        public static float turnSpeed = 20f;
        public static float dragMaxSpeedCoefficient = 5f;
        private bool canGrabBoss;

        private float dragDamageCoefficient = 1f;
        private float dragDamageInterval = 0.1f;
        private float dragDamageStopwatch;
        private float dragStopwatch;
        private float baseDragDuration = 2.5f;
        private float dragDuration;
        private float dragMaxSpeedTime = 0.8f;
        private float maxAirTime = 0.67f;
        private float smallHopVelocity = 15f;
        private float windupDuration = 0.2f;
        private float exitDuration = 0.5f;

        protected GameObject swingEffectPrefab;
        protected GameObject hitEffectPrefab;
        protected NetworkSoundEventIndex impactSound;
        public static float groundSlamDamageCoefficient = 10f;
        public static float punchDamageCoefficient = 12.5f;
        private float chargeDamageCoefficient = 8f;
        private float chargeImpactForce = 2000f;
        private Vector3 bonusForce = Vector3.up * 2000f;
        private Vector3 aimDirection;
        private float stopwatch;
        private Animator animator;
        private bool hasGrabbed;
        private OverlapAttack attack;
        private float grabRadius = 8f;
        private float groundSlamRadius = 4f;
        private SubState subState;
        public static float dodgeFOV = DodgeState.dodgeFOV;
        private uint soundID;
        private bool c1;
        private bool c2;
        private bool releaseEnemies;
        private CameraParamsOverrideHandle camParamsOverrideHandle;
        //private CameraParamsOverrideHandle camParamsOverrideHandle2;
        private bool s1;
        private int slamCount;

        public static event Action<int> onSlamCountIncremented;

        protected virtual bool forcePunch
        {
            get
            {
                return false;
            }
        }

        protected virtual bool forceThrow
        {
            get
            {
                return false;
            }
        }

        protected virtual string startAnimString
        {
            get
            {
                return "DashGrabStart";
            }
        }

        protected virtual string dashAnimString
        {
            get
            {
                return "DashGrab";
            }
        }

        private enum SubState
        {
            Windup,
            DashGrab,
            MissedGrab
        }

        public override void OnEnter()
        {
            base.OnEnter();
            animator = GetModelAnimator();
            aimDirection = GetAimRay().direction;
            aimDirection.y = Mathf.Clamp(aimDirection.y, -0.75f, 0.75f);
            stopwatch = 0f;
            dragDuration = baseDragDuration;
            this.penis.skibidi = true;

            base.PlayAnimation("FullBody, Override Soft", "BufferEmpty");
            PlayAnimation("FullBody, Override", startAnimString, "Grab.playbackRate", windupDuration);

            if(DriverPlugin.ravagerInstalled) Util.PlaySound("sfx_ravager_shine", gameObject);

            Transform modelTransform = GetModelTransform();
            HitBoxGroup hitBoxGroup = null;
            if (modelTransform)
            {
                hitBoxGroup = Array.Find(modelTransform.GetComponents<HitBoxGroup>(), (element) => element.groupName == "Drag");
            }

            characterMotor.velocity *= 0.1f;

            attack = new OverlapAttack();
            attack.damageType = DamageType.Generic;
            attack.attacker = gameObject;
            attack.inflictor = gameObject;
            attack.teamIndex = GetTeam();
            attack.damage = chargeDamageCoefficient * damageStat;
            attack.procCoefficient = 1f;
            attack.hitEffectPrefab = hitEffectPrefab;
            attack.forceVector = bonusForce;
            attack.pushAwayForce = chargeImpactForce;
            attack.hitBoxGroup = hitBoxGroup;
            attack.isCrit = RollCrit();
            dashSpeedCurve = new AnimationCurve(new Keyframe[]
            {
                new Keyframe(0f, 14f),
                new Keyframe(0.8f, 0f),
                new Keyframe(1f, 0f)
            });
            dragSpeedCurve = new AnimationCurve(new Keyframe[]
            {
                new Keyframe(0f, 0f),
                new Keyframe(0.35f, 1f),
                new Keyframe(0.9f, 5f),
                new Keyframe(1f, 5f)
            });
            dropSpeedCurve = new AnimationCurve(new Keyframe[]
            {
                new Keyframe(0f, 0f),
                new Keyframe(0.9f, 25f),
                new Keyframe(1f, 25f)
            });
            subState = SubState.Windup;

            this.penis.inGrab = true;

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

                    if (!hasGrabbed)
                    {
                        AttemptGrab(grabRadius);
                    }

                    if (stopwatch >= grabDuration)
                    {
                        if (fireEffect) Destroy(fireEffect);
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
            this.penis.inGrab = false;

            if (c1) cameraTargetParams.RemoveParamsOverride(camParamsOverrideHandle);
            //if (this.c2) this.cameraTargetParams.RemoveParamsOverride(this.camParamsOverrideHandle2);
            if (dragEffect) Destroy(dragEffect);
            if (fireEffect) Destroy(fireEffect);

            AkSoundEngine.StopPlayingID(soundID);
            RaycastHit raycastHit;

            if (!Physics.Raycast(new Ray(characterBody.footPosition, Vector3.down), out raycastHit, 100f, LayerIndex.world.mask, QueryTriggerInteraction.Collide))
                transform.position = lastSafeFootPosition + Vector3.up * 5;
            AkSoundEngine.StopPlayingID(soundID);
            modelLocator.normalizeToFloor = false;
        }
        protected virtual void ForceFlinch(CharacterBody body)
        {
            if (Util.HasEffectiveAuthority(body.gameObject))
            {
                SetStateOnHurt component = body.healthComponent.GetComponent<SetStateOnHurt>();
                if (component)
                {
                    if (component.canBeHitStunned)
                    {
                        component.SetPain();
                    }
                    else if (component.canBeStunned)
                    {
                        component.SetStun(1f);
                    }
                    foreach (EntityStateMachine e in body.gameObject.GetComponents<EntityStateMachine>())
                    {
                        if (e && e.customName.Equals("Weapon"))
                        {
                            e.SetNextStateToMain();
                        }
                    }
                }
            }
        }

        public void AttemptGrab(float grabRadius)
        {
            if (hasGrabbed) return;

            Ray aimRay = GetAimRay();

            BullseyeSearch2 bullseyeSearch = new BullseyeSearch2
            {
                teamMaskFilter = TeamMask.GetEnemyTeams(GetTeam()),
                filterByLoS = false,
                searchOrigin = transform.position,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch2.SortMode.Distance,
                onlyBullseyes = false,
                maxDistanceFilter = grabRadius,
                maxAngleFilter = 360f
            };
            bullseyeSearch.RefreshCandidates();
            bullseyeSearch.FilterOutGameObject(gameObject);

            List<HealthComponent> grabbedList = new List<HealthComponent>();
            List<HurtBox> list = bullseyeSearch.GetResults().ToList<HurtBox>();
            foreach (HurtBox hurtBox in list)
            {
                if (hurtBox)
                {
                    if (hurtBox.healthComponent && hurtBox.healthComponent.body)
                    {
                        bool canGrab = !forcePunch;

                        if (!grabbedList.Contains(hurtBox.healthComponent))
                        {
                            if (forcePunch)
                            {
                                this.penis.RefreshBlink();
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

                                return;
                            }
                        }
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}