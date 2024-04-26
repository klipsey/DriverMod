using System.Collections.Generic;
using R2API;
using RobDriver.Modules.Components;
using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace RobDriver.Modules.Components
{
    public class CoinRicochetOrb : GenericDamageOrb
    {
        public static GameObject orbPrefab = Assets.coinOrbEffect;
        public static GameObject explosionPrefab = Assets.explosionEffect;
        public static GameObject bloodExplosionPrefab = Assets.bloodExplosionEffect;
        public static float redDamageCoefficient = 16f;

        public float searchRadius = 50f;
        public SphereSearch search;
        public Vector3 coinPosition;
        public List<DamageAPI.ModdedDamageType> moddedDamageTypeHolder = new List<DamageAPI.ModdedDamageType>();
        public int bounceCount = 1;

        public float damageCoefficient = 1f;

        public override void Begin()
        {
            this.target = PickNextTarget(this.coinPosition);

            this.duration = this.distanceToTarget / this.speed;

            Color color = Color.Lerp(Color.yellow, Color.red, damageCoefficient / redDamageCoefficient);
            float scale = Mathf.Lerp(1, 2f, damageCoefficient / redDamageCoefficient);
            EffectData effectData = new EffectData
            {
                scale = this.scale * scale,
                origin = this.coinPosition,
                genericFloat = this.duration,
                color = color,
            };
            effectData.SetHurtBoxReference(this.target);
            EffectManager.SpawnEffect(Assets.coinOrbEffect, effectData, true);

        }


        public override void OnArrival()
        {
            if (this.target)
            {
                HealthComponent healthComponent = target.healthComponent;
                if (healthComponent)
                {
                    IShootable shootable = healthComponent.GetComponent<IShootable>();

                    DamageInfo damageInfo = new DamageInfo();
                    damageInfo.damage = damageValue;
                    damageInfo.attacker = attacker;
                    damageInfo.inflictor = null;
                    damageInfo.force = Vector3.zero;
                    damageInfo.crit = isCrit;
                    damageInfo.procChainMask = procChainMask;
                    damageInfo.procCoefficient = procCoefficient;
                    damageInfo.position = target.transform.position;
                    damageInfo.damageColorIndex = damageColorIndex;
                    damageInfo.damageType = DamageType.Stun1s | damageType;
                    foreach (DamageAPI.ModdedDamageType i in moddedDamageTypeHolder)
                    {
                        damageInfo.AddModdedDamageType(i);
                    }
                    damageInfo.AddModdedDamageType(DamageTypes.bloodExplosionIdentifier);

                    if (bounceCount > 2 && shootable == null)
                    {
                        BlastAttack blastAttack = new BlastAttack();
                        blastAttack.baseDamage = damageInfo.damage;
                        blastAttack.attacker = damageInfo.attacker;
                        blastAttack.teamIndex = TeamIndex.Player;
                        blastAttack.inflictor = damageInfo.inflictor;
                        blastAttack.baseForce = 1000f;
                        blastAttack.bonusForce = damageInfo.force;
                        blastAttack.crit = damageInfo.crit;
                        blastAttack.procChainMask = damageInfo.procChainMask;
                        blastAttack.procCoefficient = damageInfo.procCoefficient;
                        blastAttack.falloffModel = BlastAttack.FalloffModel.Linear;
                        blastAttack.position = damageInfo.position;
                        blastAttack.radius = bounceCount + 3f;
                        blastAttack.damageColorIndex = damageInfo.damageColorIndex;
                        blastAttack.damageType = damageInfo.damageType;
                        foreach (DamageAPI.ModdedDamageType i in moddedDamageTypeHolder)
                        {
                            blastAttack.AddModdedDamageType(i);
                        }

                        blastAttack.AddModdedDamageType(DamageTypes.bloodExplosionIdentifier);

                        blastAttack.Fire();
                        if (explosionPrefab)
                        {
                            EffectData effectData = new EffectData
                            {
                                origin = damageInfo.position,
                                scale = bounceCount
                            };
                            EffectManager.SpawnEffect(explosionPrefab, effectData, transmit: true);
                        }
                    }
                    else
                    {
                        healthComponent.TakeDamage(damageInfo);

                        if (shootable != null)
                        {
                            healthComponent.gameObject.GetComponent<CoinController>().bounceCountStored = bounceCount;
                            shootable.OnShot(damageInfo);
                        }
                        else
                        {
                            GlobalEventManager.instance.OnHitEnemy(damageInfo, healthComponent.gameObject);
                            GlobalEventManager.instance.OnHitAll(damageInfo, healthComponent.gameObject);
                        }
                    }
                }
                moddedDamageTypeHolder.Clear();
            }
        }

        public HurtBox PickNextTarget(Vector3 position)
        {
            HurtBox target = null;

            this.search = new SphereSearch
            {
                mask = LayerIndex.entityPrecise.mask,
                radius = searchRadius,
                origin = position
            };

            TeamMask teamMask = TeamMask.GetUnprotectedTeams(teamIndex);
            HurtBox[] hurtBoxes = search.RefreshCandidates().OrderCandidatesByDistance().GetHurtBoxes();
            RicochetUtils.RicochetPriority prio = RicochetUtils.RicochetPriority.None;

            foreach (HurtBox hurtBox in hurtBoxes)
            {
                if (!hurtBox.healthComponent)
                    continue;

                List<IShootable> shootables = new List<IShootable>();
                hurtBox.healthComponent.GetComponents(shootables);
                foreach (IShootable shootable in shootables)
                {
                    if (shootable.CanBeShot())
                    {
                        RicochetUtils.RicochetPriority myPrio = shootable.GetRicochetPriority();
                        if (prio < myPrio)
                        {
                            target = hurtBox;
                            prio = myPrio;
                        }
                    }
                }
                CharacterBody body = hurtBox.healthComponent.body;
                if (teamMask.HasTeam(body.teamComponent.teamIndex))
                {
                    if (prio < RicochetUtils.RicochetPriority.Body && body)
                    {
                        target = hurtBox;
                        prio = RicochetUtils.RicochetPriority.Body;
                    }
                }
            }
            return target;
        }
    }

}