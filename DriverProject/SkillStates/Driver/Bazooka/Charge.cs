using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RobDriver.SkillStates.Driver.Bazooka
{
    public class Charge : BaseDriverSkillState
    {
        public static float baseChargeDuration = 1.1f;
        public static float minChargeDuration = 0.2f;
        public static float minBloomRadius = 0.1f;
        public static float maxBloomRadius = 2f;

        private float duration;
        private uint chargePlayID;
        private GameObject chargeEffectInstance;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Charge.baseChargeDuration / this.attackSpeedStat;

            this.chargePlayID = Util.PlayAttackSpeedSound("HenryBazookaCharge", this.gameObject, this.attackSpeedStat);

            if (this.iDrive) this.iDrive.StartTimer();

            Transform muzzleTransform = base.FindModelChild("ShotgunMuzzle");
            if (muzzleTransform)
            {
                this.chargeEffectInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.LemurianBruiserMonster.ChargeMegaFireball.chargeEffectPrefab, muzzleTransform.position, muzzleTransform.rotation);
                this.chargeEffectInstance.transform.parent = muzzleTransform;
                this.chargeEffectInstance.transform.localScale *= 0.5f;
                this.chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>().newDuration = this.duration;

                this.chargeEffectInstance.transform.Find("FlameBillboards, Local").gameObject.SetActive(false);
                this.chargeEffectInstance.transform.Find("SmokeBillboard").gameObject.SetActive(false);
            }

            base.PlayAnimation("AimPitch", "SteadyAimPitch");
        }

        private float CalcCharge()
        {
            return Mathf.Clamp01(base.fixedAge / this.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.characterBody.outOfCombatStopwatch = 0f;
            this.characterBody.isSprinting = false;
            base.characterBody.SetAimTimer(0.2f);

            // this is so fucking bad LMAO
            base.PlayAnimation("Gesture, Override", "FireShotgun", "Shoot.playbackRate", this.duration);

            float charge = this.CalcCharge();

            if (base.isAuthority && ((!base.IsKeyDownAuthority() && base.fixedAge >= Charge.minChargeDuration) || base.fixedAge >= this.duration))
            {
                Fire nextState = new Fire()
                {
                    charge = charge
                };
                this.outer.SetNextState(nextState);
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            AkSoundEngine.StopPlayingID(this.chargePlayID);

            base.PlayAnimation("AimPitch", "AimPitch");

            if (this.chargeEffectInstance) EntityState.Destroy(this.chargeEffectInstance);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}