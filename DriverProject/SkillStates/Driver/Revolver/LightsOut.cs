using RoR2;
using UnityEngine;
using EntityStates;

namespace RobDriver.SkillStates.Driver.Revolver
{
    public class LightsOut : BaseDriverSkillState
    {
        public static float damageCoefficient = 15f;
        public static float procCoefficient = 1f;
        public float baseDuration = 0.8f;

        private float duration;
        private bool kill;

        public override void OnEnter()
        {
            base.OnEnter();
            this.characterBody.SetAimTimer(2f);
            this.duration = this.baseDuration / this.attackSpeedStat;

            base.PlayAnimation("Gesture, Override", "ShootLightsOut", "Action.playbackRate", this.duration);

            if (this.iDrive) this.iDrive.weaponTimer = 0.1f;

            this.Fire();
        }

        private void Fire()
        {
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, this.gameObject, "PistolMuzzle", false);

            Util.PlaySound("Play_bandit2_R_fire", this.gameObject);

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                float recoil = 24f;
                base.AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = LightsOut.damageCoefficient * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = 9999f,
                    force = 9999f,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = this.RollCrit(),
                    owner = this.gameObject,
                    muzzleName = "PistolMuzzle",
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = LightsOut.procCoefficient,
                    radius = 1f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = Shoot.critTracerEffectPrefab,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                }.Fire();
            }

            base.characterBody.AddSpreadBloom(1.25f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= (0.5f * this.duration))
            {
                if (!this.kill)
                {
                    this.kill = true;
                    if (this.iDrive) this.iDrive.weaponTimer = 0f;
                    base.PlayAnimation("Gesture, Override", this.iDrive.weaponDef.equipAnimationString);
                }
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}