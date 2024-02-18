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
        public static GameObject missileProjectilePrefab;
        public static GameObject bazookaProjectilePrefab;

        public static GameObject plasmaCannonProjectilePrefab;

        public static GameObject hmgGrenadeProjectilePrefab;

        public static GameObject lunarShard;

        public static GameObject lunarGrenadeProjectilePrefab;

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
            stunGrenadeImpactEffectPrefab.AddComponent<NetworkIdentity>();
            stunGrenadeImpactEffectPrefab.GetComponent<EffectComponent>().soundName = "sfx_driver_stun_grenade";

            GameObject nadeEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/OmniExplosionVFXCommandoGrenade.prefab").WaitForCompletion();
            GameObject radiusIndicator = GameObject.Instantiate(nadeEffect.transform.Find("Nova Sphere").gameObject);
            radiusIndicator.transform.parent = stunGrenadeImpactEffectPrefab.transform;
            radiusIndicator.transform.localPosition = Vector3.zero;
            radiusIndicator.transform.localScale = Vector3.one * grenadeImpact.blastRadius;
            radiusIndicator.transform.localRotation = Quaternion.identity;

            #region Ghost
            GameObject fcuk = GameObject.Instantiate(Assets.mainAssetBundle.LoadAsset<GameObject>("DriverStunGrenadeImpact"));
            fcuk.transform.parent = stunGrenadeImpactEffectPrefab.transform;
            fcuk.transform.localScale = Vector3.one;
            fcuk.transform.localPosition = Vector3.zero;
            fcuk.transform.localRotation = Quaternion.identity;

            Assets.AddNewEffectDef(stunGrenadeImpactEffectPrefab, "sfx_driver_stun_grenade");
            #endregion

            #region ImpactExplosion
            //grenadeImpact.lifetimeExpiredSoundString = "";
            //grenadeImpact.explosionSoundString = "";
            grenadeImpact.lifetimeExpiredSound = Addressables.LoadAssetAsync<NetworkSoundEventDef>("RoR2/Base/Commando/nseCommandoGrenadeBounce.asset").WaitForCompletion();
            grenadeImpact.offsetForLifetimeExpiredSound = 0.1f;
            grenadeImpact.destroyOnEnemy = false;
            grenadeImpact.destroyOnWorld = false;
            grenadeImpact.timerAfterImpact = true;
            grenadeImpact.falloffModel = BlastAttack.FalloffModel.None;
            grenadeImpact.lifetime = 12;
            grenadeImpact.lifetimeAfterImpact = 0.1f;
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

            CreateHMGGrenade();
            CreateLunarShard();
            CreateLunarGrenade();

            rocketProjectilePrefab = CreateRocket(false, "DriverRocketProjectile", "DriverRocketGhost", "DriverBigRocketGhost");
            missileProjectilePrefab = CreateRocket(false, "DriverMissileProjectile", "DriverMissileGhost", "DriverMissileGhost");
            bazookaProjectilePrefab = CreateRocket(true, "DriverBazookaProjectile", "DriverBazookaGhost", "DriverRocketGhost");

            plasmaCannonProjectilePrefab = CreateRocket(false, "DriverPlasmaCannonProjectile");
            plasmaCannonProjectilePrefab.GetComponent<ProjectileController>().ghostPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BFG/BeamSphereGhost.prefab").WaitForCompletion();
            plasmaCannonProjectilePrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BFG/BeamSphereExplosion.prefab").WaitForCompletion();
        }

        private static void CreateLunarShard()
        {
            lunarShard = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LunarShardProjectile"), "DriverLunarShard", true);
            DriverPlugin.Destroy(lunarShard.GetComponent<ProjectileSteerTowardTarget>());
            lunarShard.GetComponent<ProjectileImpactExplosion>().blastDamageCoefficient = 1f;

            Prefabs.projectilePrefabs.Add(lunarShard);
        }

        private static void CreateHMGGrenade()
        {
            hmgGrenadeProjectilePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "DriverHMGGrenade");
            hmgGrenadeProjectilePrefab.AddComponent<Modules.Components.RocketRotation>();
            hmgGrenadeProjectilePrefab.transform.localScale *= 2f;

            ProjectileImpactExplosion impactExplosion = hmgGrenadeProjectilePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(impactExplosion);

            GameObject fuckMyLife = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFX.prefab").WaitForCompletion().InstantiateClone("StupidFuckExplosion2", true);
            fuckMyLife.AddComponent<NetworkIdentity>();

            GameObject nadeEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/OmniExplosionVFXCommandoGrenade.prefab").WaitForCompletion();
            GameObject radiusIndicator = GameObject.Instantiate(nadeEffect.transform.Find("Nova Sphere").gameObject);
            radiusIndicator.transform.parent = fuckMyLife.transform;
            radiusIndicator.transform.localPosition = Vector3.zero;
            radiusIndicator.transform.localScale = Vector3.one;
            radiusIndicator.transform.localRotation = Quaternion.identity;

            Assets.AddNewEffectDef(fuckMyLife, "sfx_driver_grenade_explosion");

            impactExplosion.blastRadius = 6f;
            impactExplosion.destroyOnEnemy = true;
            impactExplosion.lifetime = 12f;
            impactExplosion.impactEffect = fuckMyLife;
            //impactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("sfx_driver_explosion");
            impactExplosion.timerAfterImpact = true;
            impactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController rocketController = hmgGrenadeProjectilePrefab.GetComponent<ProjectileController>();

            rocketController.ghostPrefab = stunGrenadeProjectilePrefab.GetComponent<ProjectileController>().ghostPrefab;
            rocketController.startSound = "";

            hmgGrenadeProjectilePrefab.GetComponent<Rigidbody>().useGravity = true;

            Prefabs.projectilePrefabs.Add(hmgGrenadeProjectilePrefab);
        }

        private static void CreateLunarGrenade()
        {
            lunarGrenadeProjectilePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "DriverLunarGrenade");
            lunarGrenadeProjectilePrefab.transform.localScale *= 2f;
            ProjectileImpactExplosion impactExplosion = lunarGrenadeProjectilePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(impactExplosion);

            impactExplosion.blastRadius = 7f;
            impactExplosion.destroyOnEnemy = true;
            impactExplosion.lifetime = 12f;
            impactExplosion.impactEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LunarGolem/LunarGolemTwinShotExplosion.prefab").WaitForCompletion();
            impactExplosion.timerAfterImpact = true;
            impactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController rocketController = lunarGrenadeProjectilePrefab.GetComponent<ProjectileController>();

            rocketController.ghostPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LunarExploder/LunarExploderShardGhost.prefab").WaitForCompletion();
            rocketController.startSound = "";

            lunarGrenadeProjectilePrefab.GetComponent<Rigidbody>().useGravity = true;

            Prefabs.projectilePrefabs.Add(lunarGrenadeProjectilePrefab);
        }

        private static GameObject CreateRocket(bool gravity, string projectileName, string ghostName = "", string ghostToLoad = "")
        {
            GameObject projectilePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", projectileName);
            projectilePrefab.AddComponent<Modules.Components.RocketRotation>();
            projectilePrefab.transform.localScale *= 2f;

            ProjectileImpactExplosion impactExplosion = projectilePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(impactExplosion);

            GameObject fuckMyLife = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFX.prefab").WaitForCompletion().InstantiateClone("StupidFuckExplosion", true);
            fuckMyLife.AddComponent<NetworkIdentity>();

            GameObject nadeEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/OmniExplosionVFXCommandoGrenade.prefab").WaitForCompletion();
            GameObject radiusIndicator = GameObject.Instantiate(nadeEffect.transform.Find("Nova Sphere").gameObject);
            radiusIndicator.transform.parent = fuckMyLife.transform;
            radiusIndicator.transform.localPosition = Vector3.zero;
            radiusIndicator.transform.localScale = Vector3.one;
            radiusIndicator.transform.localRotation = Quaternion.identity;

            Assets.AddNewEffectDef(fuckMyLife, "sfx_driver_explosion");

            impactExplosion.blastRadius = 10f;
            impactExplosion.destroyOnEnemy = true;
            impactExplosion.lifetime = 12f;
            impactExplosion.impactEffect = fuckMyLife;
            //impactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("sfx_driver_explosion");
            impactExplosion.timerAfterImpact = true;
            impactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController rocketController = projectilePrefab.GetComponent<ProjectileController>();

            if (ghostName != "")
            {
                GameObject ghost = CreateGhostPrefab(ghostToLoad);
                ghost.name = ghostName;
                ghost.transform.GetChild(0).Find("Smoke").gameObject.AddComponent<Modules.Components.DetachOnDestroy>();
                ghost.transform.GetChild(0).Find("Smoke").GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matDustDirectional.mat").WaitForCompletion();
                ghost.transform.GetChild(0).Find("Flame").GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Golem/matVFXFlame1.mat").WaitForCompletion();

                rocketController.ghostPrefab = ghost;
                rocketController.startSound = "";
            }

            projectilePrefab.GetComponent<Rigidbody>().useGravity = gravity;

            Prefabs.projectilePrefabs.Add(projectilePrefab);

            return projectilePrefab;
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
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName).InstantiateClone(ghostName);
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