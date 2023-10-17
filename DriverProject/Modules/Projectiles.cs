using R2API;
using Rewired.ComponentControls.Effects;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;

namespace RobDriver.Modules
{
    internal static class Projectiles
    {
        public static GameObject stunGrenadeProjectilePrefab;
        public static GameObject stunGrenadeImpactEffectPrefab;

        public static GameObject rocketProjectilePrefab;

        internal static void RegisterProjectiles()
        {
            #region Stun Grenade
            stunGrenadeProjectilePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("RobDriverStunGrenade", true);

            ProjectileController grenadeController = stunGrenadeProjectilePrefab.GetComponent<ProjectileController>();

            ProjectileDamage grenadeDamage = stunGrenadeProjectilePrefab.GetComponent<ProjectileDamage>();
            ProjectileSimple simple = stunGrenadeProjectilePrefab.GetComponent<ProjectileSimple>();
            ProjectileImpactExplosion grenadeImpact = stunGrenadeProjectilePrefab.GetComponent<ProjectileImpactExplosion>();

            Prefabs.projectilePrefabs.Add(stunGrenadeProjectilePrefab);

            stunGrenadeImpactEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/StunChanceOnHit/ImpactStunGrenade.prefab").WaitForCompletion().InstantiateClone("DriverStunGrenadeImpact", true);

            stunGrenadeImpactEffectPrefab.GetComponent<EffectComponent>().soundName = "";

            #region Ghost
            GameObject fcuk = GameObject.Instantiate(Assets.mainAssetBundle.LoadAsset<GameObject>("DriverStunGrenadeImpact"));
            fcuk.AddComponent<NetworkIdentity>();
            fcuk.transform.parent = stunGrenadeImpactEffectPrefab.transform;
            fcuk.transform.localScale = Vector3.one;
            fcuk.transform.localPosition = Vector3.zero;
            fcuk.transform.localRotation = Quaternion.identity;

            Assets.AddNewEffectDef(stunGrenadeImpactEffectPrefab, "sfx_driver_stun_grenade");
            #endregion

            #region ImpactExplosion
            grenadeImpact.lifetimeExpiredSoundString = "";
            grenadeImpact.explosionSoundString = "sfx_driver_stun_grenade";// Sounds.GasExplosion;
            grenadeImpact.offsetForLifetimeExpiredSound = 1;
            grenadeImpact.destroyOnEnemy = false;
            grenadeImpact.destroyOnWorld = false;
            grenadeImpact.timerAfterImpact = true;
            grenadeImpact.falloffModel = BlastAttack.FalloffModel.None;
            grenadeImpact.lifetime = 12;
            grenadeImpact.lifetimeAfterImpact = 0.15f;
            grenadeImpact.lifetimeRandomOffset = 0;
            grenadeImpact.blastRadius = 16;
            grenadeImpact.blastDamageCoefficient = 1;
            grenadeImpact.blastProcCoefficient = 1;
            grenadeImpact.fireChildren = false;
            grenadeImpact.childrenCount = 0;
            grenadeImpact.childrenProjectilePrefab = null;
            grenadeImpact.childrenDamageCoefficient = 0;
            grenadeImpact.impactEffect = stunGrenadeImpactEffectPrefab;

            grenadeController.startSound = "";
            grenadeController.procCoefficient = 1;

            grenadeDamage.crit = false;
            grenadeDamage.damage = 0f;
            grenadeDamage.damageColorIndex = DamageColorIndex.Default;
            grenadeDamage.damageType = DamageType.Stun1s;
            grenadeDamage.force = 1500f;
            #endregion
            #endregion

            CreateRocket();
        }

        private static void CreateRocket()
        {
            rocketProjectilePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "DriverRocketProjectile");
            rocketProjectilePrefab.AddComponent<Modules.Components.RocketRotation>();
            rocketProjectilePrefab.transform.localScale *= 2f;

            ProjectileImpactExplosion impactExplosion = rocketProjectilePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(impactExplosion);

            GameObject fuckMyLife = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFX.prefab").WaitForCompletion().InstantiateClone("StupidFuckExplosion", true);
            fuckMyLife.AddComponent<NetworkIdentity>();
            Assets.AddNewEffectDef(fuckMyLife, "sfx_driver_explosion");

            impactExplosion.blastRadius = 10f;
            impactExplosion.destroyOnEnemy = true;
            impactExplosion.lifetime = 12f;
            impactExplosion.impactEffect = fuckMyLife;
            //impactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("sfx_driver_explosion");
            impactExplosion.timerAfterImpact = true;
            impactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController rocketController = rocketProjectilePrefab.GetComponent<ProjectileController>();

            GameObject ghost = CreateGhostPrefab("DriverRocketGhost");
            ghost.transform.Find("model").Find("Smoke").gameObject.AddComponent<Modules.Components.DetachOnDestroy>();
            ghost.transform.Find("model").Find("Smoke").GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matDustDirectional.mat").WaitForCompletion();
            ghost.transform.Find("model").Find("Flame").GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Golem/matVFXFlame1.mat").WaitForCompletion();

            ghost.GetComponentInChildren<MeshRenderer>().material = Assets.rocketLauncherMat;

            rocketController.ghostPrefab = ghost;
            rocketController.startSound = "";

            rocketProjectilePrefab.GetComponent<Rigidbody>().useGravity = false;

            Prefabs.projectilePrefabs.Add(rocketProjectilePrefab);
        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.explosionSoundString = "";
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeExpiredSoundString = "";
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            ghostPrefab.AddComponent<NetworkIdentity>();
            ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}