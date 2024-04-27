using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RobDriver.SkillStates.Driver;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.SendMouseEvents;

namespace RobDriver.Modules.Components
{
    public class CoinController : NetworkBehaviour, IProjectileImpactBehavior, IOnIncomingDamageServerReceiver
    {
        public HealthComponent projectileHealthComponent;
        public ProjectileController controller;
        public DriverController iDrive;

        public NetworkSoundEventDef ricochetSound;

        public bool canRicochet = true;
        private float graceTimer = 0.4f;
        private float coolStopwatchScale = 0.001f;
        private bool startCoolStopwatch = false;
        public float ricochetMultiplier = 1.75f;
        private Vector3 rotationSpeed = new Vector3(2000f, 0f, 0f);
        public static Action<CoinController> onCoinAwakeGlobal;
        public int bounceCountStored = 0;
        private DamageInfo damageInfo;

        public void OnIncomingDamageServer(DamageInfo damageInfo)
        {
            damageInfo.damageColorIndex = DamageColorIndex.Item;
            if (damageInfo.attacker.GetComponent<DriverController>() == null)
            {
                damageInfo.rejected = true;
            }
            RicochetBullet(damageInfo);
        }

        private void Start()
        {
            float speed = UnityEngine.Random.Range(500f, 2000f);
            this.rotationSpeed = new Vector3(speed, 0f, 0f);

            iDrive = controller.owner.GetComponent<DriverController>();
            if (onCoinAwakeGlobal != null)
            {
                onCoinAwakeGlobal(this);
            }
            this.GetComponent<TeamFilter>().teamIndex = TeamIndex.Neutral;
        }

        private void FixedUpdate()
        {
            this.graceTimer -= Time.fixedDeltaTime;
            base.transform.Rotate(this.rotationSpeed * Time.fixedDeltaTime);
            if(startCoolStopwatch)
            {
                this.coolStopwatchScale -= Time.fixedDeltaTime;
                if (damageInfo.attacker && this.coolStopwatchScale <= 0f)
                {
                    this.canRicochet = false;
                    TeamComponent teamComponent = damageInfo.attacker.GetComponent<TeamComponent>();
                    float co = damageInfo.damage / teamComponent.body.damage;
                    CoinRicochetOrb orb = new CoinRicochetOrb
                    {
                        coinPosition = base.transform.position,
                        origin = base.transform.position,
                        speed = 180f + (10f * bounceCountStored),
                        attacker = damageInfo.attacker,
                        damageCoefficient = co,
                        damageValue = damageInfo.damage * this.ricochetMultiplier,
                        damageType = DamageType.Generic | damageInfo.damageType,
                        teamIndex = teamComponent.teamIndex,
                        procCoefficient = 1f,
                        isCrit = damageInfo.crit,
                        bounceCount = bounceCountStored
                    };
                    if (damageInfo.HasModdedDamageType(iDrive.ModdedDamageType)) orb.moddedDamageTypeHolder.Add(iDrive.ModdedDamageType);
                    this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    OrbManager.instance.AddOrb(orb);
                    EffectManager.SimpleSoundEffect(this.ricochetSound.index, base.transform.position, true);

                    Destroy(base.gameObject);
                }
            }
        }
        public bool CanBeShot()
        {
            return this.canRicochet;
        }

        public RicochetUtils.RicochetPriority GetRicochetPriority()
        {
            return RicochetUtils.RicochetPriority.Coin;
        }
        [Command]
        public void CmdRicochetBullet(GameObject attacker, GameObject inflictor, bool isCrit, float damage, uint procChainMask, Vector3 force, bool canRejectForce, byte colorIndex, uint damageType)
        {
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.attacker = attacker;
            damageInfo.inflictor = inflictor;
            damageInfo.crit = isCrit;
            damageInfo.damage = damage;
            damageInfo.procChainMask.mask = procChainMask;
            damageInfo.procCoefficient = 0f;
            damageInfo.force = force;
            damageInfo.canRejectForce = canRejectForce;
            damageInfo.damageColorIndex = (DamageColorIndex)colorIndex;
            damageInfo.damageType = (DamageType)damageType;
            bounceCountStored++;
            coolStopwatchScale = (coolStopwatchScale * bounceCountStored) + 0.01f;
            startCoolStopwatch = true;
            this.damageInfo = damageInfo;
        }
        public void RicochetBullet(DamageInfo damageInfo)
        {
            damageInfo.procCoefficient = 0f;
            bounceCountStored++;
            coolStopwatchScale = (coolStopwatchScale * bounceCountStored) + 0.01f;
            startCoolStopwatch = true;
            this.damageInfo = damageInfo;
        }
        public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
        {
            if (this.graceTimer <= 0f && !impactInfo.collider.GetComponent<HurtBox>() && !impactInfo.collider.GetComponent<CoinController>())
            {
                Destroy(base.gameObject);
            }
        }
        public struct CoinMethods
        {
            public static void ModifyCoinOnSpawn(CoinController coin, float damageMultiplier)
            {
                coin.ricochetMultiplier = damageMultiplier;
            }
           
            public static List<CoinController> OverlapAttackGetCoins(OverlapAttack attack)
            {
                List<CoinController> CoinController = new List<CoinController>();
                foreach (HealthComponent healthComponent in attack.ignoredHealthComponentList)
                {
                    if (healthComponent)
                    {
                        CoinController coin = healthComponent.GetComponent<CoinController>();
                        if (coin != null)
                        {
                            CoinController.Add(coin);
                        }
                    }
                }
                return CoinController;
            }
        }
    }
}