using UnityEngine;
using RoR2;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using EntityStates;

namespace RobDriver.SkillStates.Driver.SupplyDrop
{
    public class FireSupplyDrop : BaseDriverSkillState
    {
        public float baseDuration = 0.8f;

        public static float damageCoefficient = 25f;

        public Vector3 dropPosition;
        public Quaternion dropRotation;

        private float duration;
        private bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;

            base.PlayAnimation("Gesture, Override", "PressButton", "Action.playbackRate", this.duration);
        }

        public override void OnExit()
        {
            base.OnExit();
            this.FindModelChild("ButtonModel").gameObject.SetActive(false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= (0.4f * this.duration))
            {
                if (!this.hasFired)
                {
                    this.hasFired = true;
                    this.iDrive.ConsumeSupplyDrop();
                    this.Fire();
                }
            }

            if (base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        private void Fire()
        {
            if (NetworkServer.active)
            {
                GameObject weaponPickup = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.weaponPickupLegendary, this.dropPosition, UnityEngine.Random.rotation);

                TeamFilter teamFilter = weaponPickup.GetComponent<TeamFilter>();
                if (teamFilter) teamFilter.teamIndex = this.teamComponent.teamIndex;
                DriverWeaponTier weaponTier = DriverWeaponTier.Legendary;
                weaponPickup.GetComponentInChildren<Modules.Components.WeaponPickup>().weaponDef = DriverWeaponCatalog.GetRandomWeaponFromTier(weaponTier);

                NetworkServer.Spawn(weaponPickup);
            }

            if (base.isAuthority)
            {
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = AimSupplyDrop.radius;
                blastAttack.procCoefficient = 1f;
                blastAttack.position = this.dropPosition;
                blastAttack.attacker = this.gameObject;
                blastAttack.crit = this.RollCrit();
                blastAttack.baseDamage = this.damageStat * FireSupplyDrop.damageCoefficient;
                blastAttack.falloffModel = BlastAttack.FalloffModel.Linear;
                blastAttack.baseForce = 4000f;
                blastAttack.teamIndex = this.teamComponent.teamIndex;
                blastAttack.damageType = DamageType.Stun1s;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

                blastAttack.Fire();

                EffectManager.SpawnEffect(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/SurvivorPod/PodGroundImpact.prefab").WaitForCompletion(),
                    new EffectData
                    {
                        origin = this.dropPosition,
                        rotation = this.dropRotation,
                        scale = AimSupplyDrop.radius
                    }, true);
            }

            Util.PlaySound("sfx_driver_explosion", this.gameObject);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}