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

        internal static void RegisterProjectiles()
        {
            stunGrenadeProjectilePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("RobDriverStunGrenade", true);

            ProjectileController grenadeController = stunGrenadeProjectilePrefab.GetComponent<ProjectileController>();

            // ghost
            //grenadeController.ghostPrefab = ItemDisplays.LoadDisplay("DisplayStunGrenade");

            ProjectileDamage grenadeDamage = stunGrenadeProjectilePrefab.GetComponent<ProjectileDamage>();
            ProjectileSimple simple = stunGrenadeProjectilePrefab.GetComponent<ProjectileSimple>();
            ProjectileImpactExplosion grenadeImpact = stunGrenadeProjectilePrefab.GetComponent<ProjectileImpactExplosion>();

            //GameObject grenadeModel = Assets.tearGasGrenadeModel.InstantiateClone("TearGasGhost", true);
            //grenadeModel.AddComponent<NetworkIdentity>();
            //grenadeModel.AddComponent<ProjectileGhostController>();

            //grenadeController.ghostPrefab = grenadeModel;

            // whoops, forgor
            Prefabs.projectilePrefabs.Add(stunGrenadeProjectilePrefab);

            stunGrenadeImpactEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/StunChanceOnHit/ImpactStunGrenade.prefab").WaitForCompletion().InstantiateClone("DriverStunGrenadeImpact", true);

            stunGrenadeImpactEffectPrefab.GetComponent<EffectComponent>().soundName = "";

            GameObject fcuk = GameObject.Instantiate(Assets.mainAssetBundle.LoadAsset<GameObject>("DriverStunGrenadeImpact"));
            fcuk.transform.parent = stunGrenadeImpactEffectPrefab.transform;
            fcuk.transform.localScale = Vector3.one;
            fcuk.transform.localPosition = Vector3.zero;
            fcuk.transform.localRotation = Quaternion.identity;

            Assets.AddNewEffectDef(stunGrenadeImpactEffectPrefab, "sfx_driver_stun_grenade");

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