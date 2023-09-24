using UnityEngine;
using RoR2;
using EntityStates;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.Driver
{
    public class ThrowGrenade : GenericProjectileBaseState
    {
        public static float baseDuration = 0.65f;
        public static float baseDelayDuration = 0.1f * baseDuration;

        public static float damageCoefficient = 4f;

        public override void OnEnter()
        {
            base.projectilePrefab = Modules.Projectiles.stunGrenadeProjectilePrefab;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            base.attackSoundString = "";

            base.baseDuration = baseDuration;
            base.baseDelayBeforeFiringProjectile = baseDelayDuration;

            base.damageCoefficient = damageCoefficient;
            base.force = 80f;

            //base.projectilePitchBonus = 0;
            //base.minSpread = 0;
            //base.maxSpread = 0;

            base.recoilAmplitude = 0.1f;
            base.bloom = 10;

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void PlayAnimation(float duration)
        {
            if (base.GetModelAnimator())
            {
                base.PlayAnimation("Gesture, Override", "ThrowGrenade", "Grenade.playbackRate", this.duration);
            }
        }
    }
}