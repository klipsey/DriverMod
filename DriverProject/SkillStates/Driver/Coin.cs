using RobDriver.Modules.Components;
using RobDriver.Modules;
using RoR2.Projectile;
using RoR2;
using UnityEngine;
using static UnityEngine.ParticleSystem.PlaybackState;
using static UnityEngine.UI.GridLayoutGroup;
using static UnityEngine.UI.Image;

namespace RobDriver.SkillStates.Driver
{
    public class Coin : BaseDriverSkillState
    {
        public static float coinHorizontalSpeed = 7f;
        public static GameObject coinPrefab = Projectiles.coinProjectile;
        public static float ricochetMultiplier = 1.75f;
        private float baseDuration = 0.5f;
        private float duration;

        public override void OnEnter()
        {   
            RefreshState();
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            Util.PlaySound("sfx_driver_coin", base.gameObject);

            base.PlayAnimation("LeftArm, Override", "FireShard");

            this.FireProjectile();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        private void FireProjectile()
        {
            if (base.isAuthority)
            {
                Ray aimRay = GetAimRay();
                aimRay = ModifyProjectileAimRay(aimRay);
                aimRay.direction = Util.ApplySpread(aimRay.direction, 0, 0, 1f, 1f, 0f, -10);
                Vector3 flickDirection = aimRay.direction;
                flickDirection *= Mathf.Clamp(base.rigidbody.velocity.magnitude, 1f, 20f);
                flickDirection.y += base.rigidbody.velocity.y;
                CoinController.onCoinAwakeGlobal += (coin) => CoinController.CoinMethods.ModifyCoinOnSpawn(coin, ricochetMultiplier);
                ProjectileManager.instance.FireProjectile(Projectiles.coinProjectile, aimRay.origin, Util.QuaternionSafeLookRotation(flickDirection),
                    base.gameObject, 0f, 0f, false, DamageColorIndex.Default, null);
            }
        }
        protected virtual Ray ModifyProjectileAimRay(Ray aimRay)
        {
            return aimRay;
        }

    }
}