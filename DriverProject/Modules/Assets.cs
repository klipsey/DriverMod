using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using RoR2.Audio;
using System.Collections.Generic;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using TMPro;
using RoR2.UI;
using UnityEngine.UI;

namespace RobDriver.Modules
{
    internal static class Assets
    {
        internal static AssetBundle mainAssetBundle;

        internal static Shader hotpoo = Resources.Load<Shader>("Shaders/Deferred/HGStandard");
        internal static Material commandoMat;

        internal static List<EffectDef> effectDefs = new List<EffectDef>();
        internal static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();

        public static GameObject jammedEffectPrefab;
        public static GameObject stunGrenadeModelPrefab;

        public static GameObject pistolAimCrosshairPrefab;
        public static GameObject bazookaCrosshairPrefab;
        public static GameObject rocketLauncherCrosshairPrefab;
        public static GameObject grenadeLauncherCrosshairPrefab;

        public static Mesh pistolMesh;
        public static Mesh goldenGunMesh;
        public static Mesh shotgunMesh;
        public static Mesh riotShotgunMesh;
        public static Mesh slugShotgunMesh;
        public static Mesh machineGunMesh;
        public static Mesh heavyMachineGunMesh;
        public static Mesh bazookaMesh;
        public static Mesh rocketLauncherMesh;
        public static Mesh sniperMesh;
        public static Mesh armCannonMesh;
        public static Mesh plasmaCannonMesh;
        public static Mesh behemothMesh;
        public static Mesh beetleShieldMesh;
        public static Mesh grenadeLauncherMesh;

        public static Material pistolMat;
        public static Material goldenGunMat;
        public static Material shotgunMat;
        public static Material riotShotgunMat;
        public static Material slugShotgunMat;
        public static Material machineGunMat;
        public static Material heavyMachineGunMat;
        public static Material rocketLauncherMat;
        public static Material rocketLauncherAltMat;
        public static Material bazookaMat;
        public static Material sniperMat;
        public static Material armCannonMat;
        public static Material plasmaCannonMat;
        public static Material grenadeLauncherMat;

        public static Material knifeMat;

        public static GameObject shotgunShell;

        public static GameObject weaponPickup;
        public static GameObject weaponPickupLegendary;
        public static GameObject weaponPickupUnique;
        public static GameObject weaponPickupOld;

        public static GameObject weaponPickupEffect;

        internal static Texture pistolWeaponIcon;
        internal static Texture goldenGunWeaponIcon;
        internal static Texture shotgunWeaponIcon;
        internal static Texture riotShotgunWeaponIcon;
        internal static Texture slugShotgunWeaponIcon;
        internal static Texture machineGunWeaponIcon;
        internal static Texture heavyMachineGunWeaponIcon;
        internal static Texture bazookaWeaponIcon;
        internal static Texture rocketLauncherWeaponIcon;
        internal static Texture rocketLauncherAltWeaponIcon;
        internal static Texture sniperWeaponIcon;
        internal static Texture armCannonWeaponIcon;
        internal static Texture plasmaCannonWeaponIcon;
        internal static Texture beetleShieldWeaponIcon;
        internal static Texture grenadeLauncherWeaponIcon;

        public static GameObject shotgunTracer;
        public static GameObject shotgunTracerCrit;

        internal static DriverWeaponDef pistolWeaponDef;
        internal static DriverWeaponDef goldenGunWeaponDef;
        internal static DriverWeaponDef shotgunWeaponDef;
        internal static DriverWeaponDef riotShotgunWeaponDef;
        internal static DriverWeaponDef slugShotgunWeaponDef;
        internal static DriverWeaponDef machineGunWeaponDef;
        internal static DriverWeaponDef heavyMachineGunWeaponDef;
        internal static DriverWeaponDef bazookaWeaponDef;
        internal static DriverWeaponDef rocketLauncherWeaponDef;
        internal static DriverWeaponDef rocketLauncherAltWeaponDef;
        internal static DriverWeaponDef sniperWeaponDef;
        internal static DriverWeaponDef armCannonWeaponDef;
        internal static DriverWeaponDef plasmaCannonWeaponDef;
        internal static DriverWeaponDef beetleShieldWeaponDef;
        internal static DriverWeaponDef behemothWeaponDef;
        internal static DriverWeaponDef grenadeLauncherWeaponDef;

        internal static void PopulateAssets()
        {
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DriverMod.robdriver"))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }

            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("DriverMod.driver_bank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }

            //drainPunchChargeEffect = LoadEffect("DrainPunchChargeEffect", true);

            //punchSoundDef = CreateNetworkSoundEventDef("RegigigasPunchImpact");

            jammedEffectPrefab = CreateTextPopupEffect("DriverGunJammedEffect", "ROB_DRIVER_JAMMED_POPUP");

            #region Pistol Aim Mode Crosshair
            pistolAimCrosshairPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/StandardCrosshair.prefab").WaitForCompletion().InstantiateClone("DriverPistolAimCrosshair", false);

            GameObject stockHolder = GameObject.Instantiate(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageCrosshair.prefab").WaitForCompletion().transform.Find("Stock").gameObject);
            stockHolder.transform.parent = pistolAimCrosshairPrefab.transform;

            CrosshairController pistolCrosshair = pistolAimCrosshairPrefab.GetComponent<CrosshairController>();

            pistolCrosshair.skillStockSpriteDisplays = new CrosshairController.SkillStockSpriteDisplay[]
            {
                new CrosshairController.SkillStockSpriteDisplay
                {
                    target = stockHolder.transform.GetChild(0).gameObject,
                    skillSlot = SkillSlot.Secondary,
                    minimumStockCountToBeValid = 1,
                    maximumStockCountToBeValid = 99
                },
                new CrosshairController.SkillStockSpriteDisplay
                {
                    target = stockHolder.transform.GetChild(1).gameObject,
                    skillSlot = SkillSlot.Secondary,
                    minimumStockCountToBeValid = 2,
                    maximumStockCountToBeValid = 99
                },
                new CrosshairController.SkillStockSpriteDisplay
                {
                    target = stockHolder.transform.GetChild(2).gameObject,
                    skillSlot = SkillSlot.Secondary,
                    minimumStockCountToBeValid = 3,
                    maximumStockCountToBeValid = 99
                },
                new CrosshairController.SkillStockSpriteDisplay
                {
                    target = stockHolder.transform.GetChild(3).gameObject,
                    skillSlot = SkillSlot.Secondary,
                    minimumStockCountToBeValid = 4,
                    maximumStockCountToBeValid = 99
                }
            };

            GameObject chargeBar = GameObject.Instantiate(mainAssetBundle.LoadAsset<GameObject>("ChargeBar"));
            chargeBar.transform.SetParent(pistolAimCrosshairPrefab.transform);

            RectTransform rect = chargeBar.GetComponent<RectTransform>();

            rect.localScale = new Vector3(0.75f, 0.075f, 1f);
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(0f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchoredPosition = new Vector2(50f, 0f);
            rect.localPosition = new Vector3(0f, -60f, 0f);

            chargeBar.transform.GetChild(0).gameObject.AddComponent<Modules.Components.CrosshairChargeBar>().crosshairController = pistolAimCrosshairPrefab.GetComponent<RoR2.UI.CrosshairController>();
            #endregion

            #region Bazooka Crosshair
            bazookaCrosshairPrefab = PrefabAPI.InstantiateClone(LoadCrosshair("ToolbotGrenadeLauncher"), "DriverBazookaCrosshair", false);
            CrosshairController crosshair = bazookaCrosshairPrefab.GetComponent<CrosshairController>();
            crosshair.skillStockSpriteDisplays = new CrosshairController.SkillStockSpriteDisplay[0];

            bazookaCrosshairPrefab.transform.GetChild(0).GetComponentInChildren<Image>().sprite = Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/Railgunner/texCrosshairRailgunSniperNib.png").WaitForCompletion();
            rect = bazookaCrosshairPrefab.transform.GetChild(0).GetComponent<RectTransform>();
            rect.localEulerAngles = Vector3.zero;
            rect.anchoredPosition = new Vector2(-50f, -10f);

            bazookaCrosshairPrefab.transform.GetChild(1).GetComponentInChildren<Image>().sprite = Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/Railgunner/texCrosshairRailgunSniperNib.png").WaitForCompletion();
            rect = bazookaCrosshairPrefab.transform.GetChild(1).GetComponent<RectTransform>();
            rect.localEulerAngles = new Vector3(0f, 0f, 90f);

            bazookaCrosshairPrefab.transform.GetChild(2).GetComponentInChildren<Image>().sprite = Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/Railgunner/texCrosshairRailgunSniperNib.png").WaitForCompletion();
            rect = bazookaCrosshairPrefab.transform.GetChild(2).GetComponent<RectTransform>();
            rect.localEulerAngles = Vector3.zero;
            rect.anchoredPosition = new Vector2(50f, -10f);

            bazookaCrosshairPrefab.transform.Find("StockCountHolder").gameObject.SetActive(false);
            bazookaCrosshairPrefab.transform.Find("Image, Arrow (1)").gameObject.SetActive(true);

            crosshair.spriteSpreadPositions[0].zeroPosition = new Vector3(0f, 25f, 0f);
            crosshair.spriteSpreadPositions[0].onePosition = new Vector3(-50f, 25f, 0f);

            crosshair.spriteSpreadPositions[1].zeroPosition = new Vector3(100f, 0f, 0f);
            crosshair.spriteSpreadPositions[1].onePosition = new Vector3(150f, 0f, 0f);

            crosshair.spriteSpreadPositions[2].zeroPosition = new Vector3(0f, 25f, 0f);
            crosshair.spriteSpreadPositions[2].onePosition = new Vector3(50f, 25f, 0f);

            chargeBar = GameObject.Instantiate(mainAssetBundle.LoadAsset<GameObject>("ChargeBar"));
            chargeBar.transform.SetParent(bazookaCrosshairPrefab.transform);

            rect = chargeBar.GetComponent<RectTransform>();

            rect.localScale = new Vector3(0.5f, 0.1f, 1f);
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(0f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchoredPosition = new Vector2(50f, 0f);
            rect.localPosition = new Vector3(40f, -40f, 0f);
            rect.localEulerAngles = new Vector3(0f, 0f, 90f);

            chargeBar.transform.GetChild(0).gameObject.AddComponent<Modules.Components.CrosshairChargeBar>().crosshairController = crosshair;
            #endregion

            #region Grenade Launcher Crosshair
            grenadeLauncherCrosshairPrefab = PrefabAPI.InstantiateClone(LoadCrosshair("ToolbotGrenadeLauncher"), "DriverGrenadeLauncherCrosshair", false);
            crosshair = grenadeLauncherCrosshairPrefab.GetComponent<CrosshairController>();
            crosshair.skillStockSpriteDisplays = new CrosshairController.SkillStockSpriteDisplay[0];

            grenadeLauncherCrosshairPrefab.transform.GetChild(0).GetComponentInChildren<Image>().sprite = Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/Railgunner/texCrosshairRailgunSniperNib.png").WaitForCompletion();
            rect = grenadeLauncherCrosshairPrefab.transform.GetChild(0).GetComponent<RectTransform>();
            rect.localEulerAngles = Vector3.zero;
            rect.anchoredPosition = new Vector2(-50f, -10f);

            grenadeLauncherCrosshairPrefab.transform.GetChild(1).GetComponentInChildren<Image>().sprite = Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/Railgunner/texCrosshairRailgunSniperNib.png").WaitForCompletion();
            rect = grenadeLauncherCrosshairPrefab.transform.GetChild(1).GetComponent<RectTransform>();
            rect.localEulerAngles = new Vector3(0f, 0f, 90f);

            grenadeLauncherCrosshairPrefab.transform.GetChild(2).GetComponentInChildren<Image>().sprite = Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/Railgunner/texCrosshairRailgunSniperNib.png").WaitForCompletion();
            rect = grenadeLauncherCrosshairPrefab.transform.GetChild(2).GetComponent<RectTransform>();
            rect.localEulerAngles = Vector3.zero;
            rect.anchoredPosition = new Vector2(50f, -10f);

            grenadeLauncherCrosshairPrefab.transform.Find("StockCountHolder").gameObject.SetActive(false);
            grenadeLauncherCrosshairPrefab.transform.Find("Image, Arrow (1)").gameObject.SetActive(true);

            crosshair.spriteSpreadPositions[0].zeroPosition = new Vector3(25f, 25f, 0f);
            crosshair.spriteSpreadPositions[0].onePosition = new Vector3(-25f, 25f, 0f);

            crosshair.spriteSpreadPositions[1].zeroPosition = new Vector3(75f, 0f, 0f);
            crosshair.spriteSpreadPositions[1].onePosition = new Vector3(125f, 0f, 0f);

            crosshair.spriteSpreadPositions[2].zeroPosition = new Vector3(-25f, 25f, 0f);
            crosshair.spriteSpreadPositions[2].onePosition = new Vector3(25f, 25f, 0f);
            #endregion

            #region Rocket Launcher Crosshair
            rocketLauncherCrosshairPrefab = PrefabAPI.InstantiateClone(LoadCrosshair("ToolbotGrenadeLauncher"), "DriveRocketLauncherCrosshair", false);
            crosshair = rocketLauncherCrosshairPrefab.GetComponent<CrosshairController>();
            crosshair.skillStockSpriteDisplays = new CrosshairController.SkillStockSpriteDisplay[0];
            rocketLauncherCrosshairPrefab.transform.Find("StockCountHolder").gameObject.SetActive(false);
            #endregion

            pistolMesh = mainAssetBundle.LoadAsset<Mesh>("meshPistol");
            goldenGunMesh = mainAssetBundle.LoadAsset<Mesh>("meshGoldenGun");
            shotgunMesh = mainAssetBundle.LoadAsset<Mesh>("meshSuperShotgun");
            riotShotgunMesh = mainAssetBundle.LoadAsset<Mesh>("meshRiotShotgun");
            slugShotgunMesh = mainAssetBundle.LoadAsset<Mesh>("meshSlugShotgun");
            machineGunMesh = mainAssetBundle.LoadAsset<Mesh>("meshMachineGun");
            heavyMachineGunMesh = mainAssetBundle.LoadAsset<Mesh>("meshHeavyMachineGun");
            bazookaMesh = mainAssetBundle.LoadAsset<Mesh>("meshBazooka");
            rocketLauncherMesh = mainAssetBundle.LoadAsset<Mesh>("meshRocketLauncher");
            sniperMesh = mainAssetBundle.LoadAsset<Mesh>("meshSniperRifle");
            armCannonMesh = mainAssetBundle.LoadAsset<Mesh>("meshArmCannon");
            plasmaCannonMesh = mainAssetBundle.LoadAsset<Mesh>("meshPlasmaCannon");
            behemothMesh = mainAssetBundle.LoadAsset<Mesh>("meshBehemoth");
            beetleShieldMesh = mainAssetBundle.LoadAsset<Mesh>("meshBeetleShield");
            grenadeLauncherMesh = mainAssetBundle.LoadAsset<Mesh>("meshGrenadeLauncher");

            pistolMat = CreateMaterial("matPistol");
            goldenGunMat = CreateMaterial("matGoldenGun");
            shotgunMat = CreateMaterial("matShotgun");
            riotShotgunMat = CreateMaterial("matRiotShotgun");
            slugShotgunMat = CreateMaterial("matSlugShotgun");
            machineGunMat = CreateMaterial("matMachineGun");
            heavyMachineGunMat = CreateMaterial("matHeavyMachineGun");
            bazookaMat = CreateMaterial("matBazooka");
            rocketLauncherMat = CreateMaterial("matRocketLauncher");
            rocketLauncherAltMat = CreateMaterial("matRocketLauncherAlt");
            sniperMat = CreateMaterial("matSniperRifle");
            armCannonMat = CreateMaterial("matArmCannon", 1f);
            plasmaCannonMat = CreateMaterial("matPlasmaCannon", 45f, Color.white);
            grenadeLauncherMat = CreateMaterial("matGrenadeLauncher");

            knifeMat = CreateMaterial("matKnife");

            shotgunShell = mainAssetBundle.LoadAsset<GameObject>("ShotgunShell");
            shotgunShell.GetComponentInChildren<MeshRenderer>().material = CreateMaterial("matShotgunShell");
            shotgunShell.AddComponent<Modules.Components.ShellController>();


            #region Normal weapon pickup
            weaponPickup = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandolier/AmmoPack.prefab").WaitForCompletion().InstantiateClone("DriverWeaponPickup", true);

            weaponPickup.GetComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = 55f;
            weaponPickup.GetComponent<DestroyOnTimer>().duration = 60f;

            AmmoPickup ammoPickupComponent = weaponPickup.GetComponentInChildren<AmmoPickup>();
            Components.WeaponPickup weaponPickupComponent = ammoPickupComponent.gameObject.AddComponent<Components.WeaponPickup>();

            weaponPickupComponent.baseObject = ammoPickupComponent.baseObject;
            weaponPickupComponent.pickupEffect = ammoPickupComponent.pickupEffect;
            weaponPickupComponent.teamFilter = ammoPickupComponent.teamFilter;

            Material uncommonPickupMat = Material.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/Base/Bandolier/matPickups.mat").WaitForCompletion());
            uncommonPickupMat.SetColor("_TintColor", new Color(0f, 80f / 255f, 0f, 1f));

            weaponPickup.GetComponentInChildren<MeshRenderer>().enabled = false;/*.materials = new Material[]
            {
                Assets.shotgunMat,
                uncommonPickupMat
            };*/

            GameObject pickupModel = GameObject.Instantiate(mainAssetBundle.LoadAsset<GameObject>("WeaponPickup"));
            pickupModel.transform.parent = weaponPickup.transform.Find("Visuals");
            pickupModel.transform.localPosition = new Vector3(0f, -0.35f, 0f);
            pickupModel.transform.localRotation = Quaternion.identity;

            MeshRenderer pickupMesh = pickupModel.GetComponentInChildren<MeshRenderer>();
            /*pickupMesh.materials = new Material[]
            {
                CreateMaterial("matCrate1"),
                CreateMaterial("matCrate2")//,
                //uncommonPickupMat
            };*/
            pickupMesh.material = CreateMaterial("matBriefcase");

            GameObject textShit = GameObject.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BearProc"));
            MonoBehaviour.Destroy(textShit.GetComponent<EffectComponent>());
            textShit.transform.parent = pickupModel.transform;
            textShit.transform.localPosition = Vector3.zero;
            textShit.transform.localRotation = Quaternion.identity;

            textShit.GetComponent<DestroyOnTimer>().enabled = false;

            ObjectScaleCurve whatTheFuckIsThis = textShit.GetComponentInChildren<ObjectScaleCurve>();
            //whatTheFuckIsThis.enabled = false;
            //whatTheFuckIsThis.transform.localScale = Vector3.one * 2;
            //whatTheFuckIsThis.timeMax = 60f;
            Transform helpMe = whatTheFuckIsThis.transform;
            MonoBehaviour.DestroyImmediate(whatTheFuckIsThis);
            helpMe.transform.localScale = Vector3.one * 1.25f;

            MonoBehaviour.Destroy(ammoPickupComponent);
            MonoBehaviour.Destroy(weaponPickup.GetComponentInChildren<RoR2.GravitatePickup>());
            #endregion

            #region Legendary weapon pickup
            weaponPickupLegendary = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandolier/AmmoPack.prefab").WaitForCompletion().InstantiateClone("DriverWeaponPickupLegendary", true);

            weaponPickupLegendary.GetComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = 110f;
            weaponPickupLegendary.GetComponent<DestroyOnTimer>().duration = 120f;

            AmmoPickup ammoPickupComponent2 = weaponPickupLegendary.GetComponentInChildren<AmmoPickup>();
            Components.WeaponPickup weaponPickupComponent2 = ammoPickupComponent2.gameObject.AddComponent<Components.WeaponPickup>();

            weaponPickupComponent2.baseObject = ammoPickupComponent2.baseObject;
            weaponPickupComponent2.pickupEffect = ammoPickupComponent2.pickupEffect;
            weaponPickupComponent2.teamFilter = ammoPickupComponent2.teamFilter;

            weaponPickupLegendary.GetComponentInChildren<MeshRenderer>().enabled = false;

            GameObject pickupModel2 = GameObject.Instantiate(mainAssetBundle.LoadAsset<GameObject>("WeaponPickupLegendary"));
            pickupModel2.transform.parent = weaponPickupLegendary.transform.Find("Visuals");
            pickupModel2.transform.localPosition = new Vector3(0f, -0.35f, 0f);
            pickupModel2.transform.localRotation = Quaternion.identity;

            MeshRenderer pickupMesh2 = pickupModel2.GetComponentInChildren<MeshRenderer>();
            pickupMesh2.material = CreateMaterial("matBriefcaseGold");

            GameObject textShit2 = GameObject.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BearProc"));
            MonoBehaviour.Destroy(textShit2.GetComponent<EffectComponent>());
            textShit2.transform.parent = pickupModel2.transform;
            textShit2.transform.localPosition = Vector3.zero;
            textShit2.transform.localRotation = Quaternion.identity;

            textShit2.GetComponent<DestroyOnTimer>().enabled = false;

            ObjectScaleCurve whatTheFuckIsThis2 = textShit2.GetComponentInChildren<ObjectScaleCurve>();
            //whatTheFuckIsThis.enabled = false;
            //whatTheFuckIsThis.transform.localScale = Vector3.one * 2;
            //whatTheFuckIsThis.timeMax = 60f;
            Transform helpMe2 = whatTheFuckIsThis2.transform;
            MonoBehaviour.DestroyImmediate(whatTheFuckIsThis2);
            helpMe2.transform.localScale = Vector3.one * 1.25f;

            MonoBehaviour.Destroy(ammoPickupComponent2);
            MonoBehaviour.Destroy(weaponPickupLegendary.GetComponentInChildren<RoR2.GravitatePickup>());
            #endregion

            #region Unique weapon pickup
            weaponPickupUnique = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandolier/AmmoPack.prefab").WaitForCompletion().InstantiateClone("DriverweaponPickupUnique", true);

            weaponPickupUnique.GetComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = 110f;
            weaponPickupUnique.GetComponent<DestroyOnTimer>().duration = 120f;

            AmmoPickup ammoPickupComponent3 = weaponPickupUnique.GetComponentInChildren<AmmoPickup>();
            Components.WeaponPickup weaponPickupComponent3 = ammoPickupComponent3.gameObject.AddComponent<Components.WeaponPickup>();

            weaponPickupComponent3.baseObject = ammoPickupComponent3.baseObject;
            weaponPickupComponent3.pickupEffect = ammoPickupComponent3.pickupEffect;
            weaponPickupComponent3.teamFilter = ammoPickupComponent3.teamFilter;

            weaponPickupUnique.GetComponentInChildren<MeshRenderer>().enabled = false;

            GameObject pickupModel3 = GameObject.Instantiate(mainAssetBundle.LoadAsset<GameObject>("WeaponPickupUnique"));
            pickupModel3.transform.parent = weaponPickupUnique.transform.Find("Visuals");
            pickupModel3.transform.localPosition = new Vector3(0f, -0.35f, 0f);
            pickupModel3.transform.localRotation = Quaternion.identity;

            MeshRenderer pickupMesh3 = pickupModel3.GetComponentInChildren<MeshRenderer>();
            pickupMesh3.material = CreateMaterial("matBriefcaseUnique");

            GameObject textShit3 = GameObject.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BearProc"));
            MonoBehaviour.Destroy(textShit3.GetComponent<EffectComponent>());
            textShit3.transform.parent = pickupModel3.transform;
            textShit3.transform.localPosition = Vector3.zero;
            textShit3.transform.localRotation = Quaternion.identity;

            textShit3.GetComponent<DestroyOnTimer>().enabled = false;

            ObjectScaleCurve whatTheFuckIsThis3 = textShit3.GetComponentInChildren<ObjectScaleCurve>();
            Transform helpMe3 = whatTheFuckIsThis3.transform;
            MonoBehaviour.DestroyImmediate(whatTheFuckIsThis3);
            helpMe3.transform.localScale = Vector3.one * 1.25f;

            MonoBehaviour.Destroy(ammoPickupComponent3);
            MonoBehaviour.Destroy(weaponPickupUnique.GetComponentInChildren<RoR2.GravitatePickup>());
            #endregion

            #region Old weapon pickup
            weaponPickupOld = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandolier/AmmoPack.prefab").WaitForCompletion().InstantiateClone("DriverWeaponPickupOld", true);

            weaponPickupOld.GetComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = 55f;
            weaponPickupOld.GetComponent<DestroyOnTimer>().duration = 60f;

            AmmoPickup ammoPickupComponent4 = weaponPickupOld.GetComponentInChildren<AmmoPickup>();
            Components.WeaponPickup weaponPickupComponent4 = ammoPickupComponent4.gameObject.AddComponent<Components.WeaponPickup>();

            weaponPickupComponent4.baseObject = ammoPickupComponent4.baseObject;
            weaponPickupComponent4.pickupEffect = ammoPickupComponent4.pickupEffect;
            weaponPickupComponent4.teamFilter = ammoPickupComponent4.teamFilter;

            weaponPickupOld.GetComponentInChildren<MeshRenderer>().enabled = false;

            GameObject pickupModel4 = GameObject.Instantiate(mainAssetBundle.LoadAsset<GameObject>("WeaponPickupOld"));
            pickupModel4.transform.parent = weaponPickupOld.transform.Find("Visuals");
            pickupModel4.transform.localPosition = new Vector4(0f, -0.35f, 0f);
            pickupModel4.transform.localRotation = Quaternion.identity;

            MeshRenderer pickupMesh4 = pickupModel4.GetComponentInChildren<MeshRenderer>();
            pickupMesh4.materials = new Material[]
            {
                CreateMaterial("matCrate1"),
                CreateMaterial("matCrate2")
            };

             GameObject textShit4 = GameObject.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BearProc"));
            MonoBehaviour.Destroy(textShit4.GetComponent<EffectComponent>());
            textShit4.transform.parent = pickupModel4.transform;
            textShit4.transform.localPosition = Vector4.zero;
            textShit4.transform.localRotation = Quaternion.identity;

            textShit4.GetComponent<DestroyOnTimer>().enabled = false;

            ObjectScaleCurve whatTheFuckIsThis4 = textShit4.GetComponentInChildren<ObjectScaleCurve>();
            //whatTheFuckIsThis.enabled = false;
            //whatTheFuckIsThis.transform.localScale = Vector4.one * 2;
            //whatTheFuckIsThis.timeMax = 60f;
            Transform helpMe4 = whatTheFuckIsThis4.transform;
            MonoBehaviour.DestroyImmediate(whatTheFuckIsThis4);
            helpMe4.transform.localScale = Vector4.one * 1.25f;

            MonoBehaviour.Destroy(ammoPickupComponent4);
            MonoBehaviour.Destroy(weaponPickupOld.GetComponentInChildren<RoR2.GravitatePickup>());
            #endregion

            weaponPickupEffect = weaponPickupComponent.pickupEffect.InstantiateClone("RobDriverWeaponPickupEffect", true);
            weaponPickupEffect.AddComponent<NetworkIdentity>();
            AddNewEffectDef(weaponPickupEffect, "sfx_driver_pickup");


            weaponPickupComponent.pickupEffect = weaponPickupEffect;
            weaponPickupComponent2.pickupEffect = weaponPickupEffect;
            weaponPickupComponent3.pickupEffect = weaponPickupEffect;
            weaponPickupComponent4.pickupEffect = weaponPickupEffect;


            pistolWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texPistolWeaponIcon");
            goldenGunWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texGoldenGunWeaponIcon");
            shotgunWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texShotgunWeaponIcon");
            riotShotgunWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texRiotShotgunWeaponIcon");
            slugShotgunWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texSlugShotgunWeaponIcon");
            machineGunWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texMachineGunWeaponIcon");
            heavyMachineGunWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texHeavyMachineGunWeaponIcon");
            bazookaWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texBazookaWeaponIcon");
            rocketLauncherWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texRocketLauncherWeaponIcon");
            rocketLauncherAltWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texRocketLauncherAltWeaponIcon");
            sniperWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texSniperRifleWeaponIcon");
            armCannonWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texArmCannonWeaponIcon");
            plasmaCannonWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texPlasmaCannonWeaponIcon");
            beetleShieldWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texBeetleShieldWeaponIcon");
            grenadeLauncherWeaponIcon = mainAssetBundle.LoadAsset<Texture>("texGrenadeLauncherWeaponIcon");


            shotgunTracer = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun").InstantiateClone("DriverShotgunTracer", true);

            if (!shotgunTracer.GetComponent<EffectComponent>()) shotgunTracer.AddComponent<EffectComponent>();
            if (!shotgunTracer.GetComponent<VFXAttributes>()) shotgunTracer.AddComponent<VFXAttributes>();
            if (!shotgunTracer.GetComponent<NetworkIdentity>()) shotgunTracer.AddComponent<NetworkIdentity>();

            Material bulletMat = null;

            foreach (LineRenderer i in shotgunTracer.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {
                    bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
                    bulletMat.SetColor("_TintColor", new Color(0.68f, 0.58f, 0.05f));
                    i.material = bulletMat;
                    i.startColor = new Color(0.68f, 0.58f, 0.05f);
                    i.endColor = new Color(0.68f, 0.58f, 0.05f);
                }
            }

            shotgunTracerCrit = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun").InstantiateClone("DriverShotgunTracerCritical", true);

            if (!shotgunTracerCrit.GetComponent<EffectComponent>()) shotgunTracerCrit.AddComponent<EffectComponent>();
            if (!shotgunTracerCrit.GetComponent<VFXAttributes>()) shotgunTracerCrit.AddComponent<VFXAttributes>();
            if (!shotgunTracerCrit.GetComponent<NetworkIdentity>()) shotgunTracerCrit.AddComponent<NetworkIdentity>();

            foreach (LineRenderer i in shotgunTracerCrit.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {
                    Material material = UnityEngine.Object.Instantiate<Material>(i.material);
                    material.SetColor("_TintColor", Color.yellow);
                    i.material = material;
                    i.startColor = new Color(0.8f, 0.24f, 0f);
                    i.endColor = new Color(0.8f, 0.24f, 0f);
                }
            }

            AddNewEffectDef(shotgunTracer);
            AddNewEffectDef(shotgunTracerCrit);

            Modules.Config.InitROO(Assets.mainAssetBundle.LoadAsset<Sprite>("texDriverIcon"), "Literally me");

            // actually i have to run this in driver's script, so the skilldefs can be created first
            //InitWeaponDefs();
            // kinda jank kinda not impactful enough to care about changing
        }

        internal static void InitWeaponDefs()
        {
            // ignore this one, this is the default
            pistolWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = "ROB_DRIVER_PISTOL_NAME",
                descriptionToken = "ROB_DRIVER_PISTOL_DESC",
                icon = Assets.pistolWeaponIcon,
                crosshairPrefab = Assets.LoadCrosshair("Standard"),
                tier = DriverWeaponTier.Common,
                baseDuration = 0f,
                primarySkillDef = null,
                secondarySkillDef = null,
                mesh = Assets.pistolMesh,
                material = Assets.pistolMat,
                animationSet = DriverWeaponDef.AnimationSet.Default,
            });
            DriverWeaponCatalog.AddWeapon(pistolWeaponDef);

            goldenGunWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = "ROB_DRIVER_GOLDENGUN_NAME",
                descriptionToken = "ROB_DRIVER_GOLDENGUN_DESC",
                icon = Assets.goldenGunWeaponIcon,
                crosshairPrefab = Assets.LoadCrosshair("Standard"),
                tier = DriverWeaponTier.Unique,
                baseDuration = 8f,
                primarySkillDef = Survivors.Driver.goldenGunPrimarySkillDef,
                secondarySkillDef = Survivors.Driver.goldenGunSecondarySkillDef,
                mesh = Assets.goldenGunMesh,
                material = Assets.goldenGunMat,
                animationSet = DriverWeaponDef.AnimationSet.Default,
                calloutSoundString = "sfx_driver_callout_generic"
            });
            DriverWeaponCatalog.AddWeapon(goldenGunWeaponDef);
            DriverWeaponCatalog.GoldenGun = goldenGunWeaponDef;

            beetleShieldWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = "ROB_DRIVER_BEETLESHIELD_NAME",
                descriptionToken = "ROB_DRIVER_BEETLESHIELD_DESC",
                icon = Assets.beetleShieldWeaponIcon,
                crosshairPrefab = Assets.LoadCrosshair("Standard"),
                tier = DriverWeaponTier.Unique,
                baseDuration = 0f,
                primarySkillDef = Survivors.Driver.beetleShieldPrimarySkillDef,
                secondarySkillDef = Survivors.Driver.beetleShieldSecondarySkillDef,
                mesh = Assets.beetleShieldMesh,
                material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Beetle/matBeetle.mat").WaitForCompletion(),
                animationSet = DriverWeaponDef.AnimationSet.Default,
                calloutSoundString = "sfx_driver_callout_generic"
            });
            DriverWeaponCatalog.AddWeapon(beetleShieldWeaponDef);
            DriverWeaponCatalog.BeetleShield = beetleShieldWeaponDef;

            // example of creating a WeaponDef through code and adding it to the catalog for driver to obtain
            if (Modules.Config.shotgunEnabled.Value)
            {
                shotgunWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
                {
                    nameToken = "ROB_DRIVER_SHOTGUN_NAME",
                    descriptionToken = "ROB_DRIVER_SHOTGUN_DESC",
                    icon = Assets.shotgunWeaponIcon,
                    crosshairPrefab = Assets.LoadCrosshair("SMG"),
                    tier = DriverWeaponTier.Uncommon,
                    baseDuration = Config.shotgunDuration.Value,
                    primarySkillDef = Survivors.Driver.shotgunPrimarySkillDef,
                    secondarySkillDef = Survivors.Driver.shotgunSecondarySkillDef,
                    mesh = Assets.shotgunMesh,
                    material = Assets.shotgunMat,
                    animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                    calloutSoundString = "sfx_driver_callout_shotgun"
                });// now add it to the catalog here; catalog is necessary for networking
                DriverWeaponCatalog.AddWeapon(shotgunWeaponDef);
            }

            if (Modules.Config.riotShotgunEnabled.Value)
            {
                riotShotgunWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
                {
                    nameToken = "ROB_DRIVER_RIOT_SHOTGUN_NAME",
                    descriptionToken = "ROB_DRIVER_RIOT_SHOTGUN_DESC",
                    icon = Assets.riotShotgunWeaponIcon,
                    crosshairPrefab = Assets.LoadCrosshair("SMG"),
                    tier = DriverWeaponTier.Uncommon,
                    baseDuration = Config.riotShotgunDuration.Value,
                    primarySkillDef = Survivors.Driver.riotShotgunPrimarySkillDef,
                    secondarySkillDef = Survivors.Driver.riotShotgunSecondarySkillDef,
                    mesh = Assets.riotShotgunMesh,
                    material = Assets.riotShotgunMat,
                    animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                    calloutSoundString = "sfx_driver_callout_shotgun"
                });
                DriverWeaponCatalog.AddWeapon(riotShotgunWeaponDef);
            }

            if (Modules.Config.slugShotgunEnabled.Value)
            {
                slugShotgunWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
                {
                    nameToken = "ROB_DRIVER_SLUG_SHOTGUN_NAME",
                    descriptionToken = "ROB_DRIVER_SLUG_SHOTGUN_DESC",
                    icon = Assets.slugShotgunWeaponIcon,
                    crosshairPrefab = Assets.LoadCrosshair("SMG"),
                    tier = DriverWeaponTier.Uncommon,
                    baseDuration = Config.slugShotgunDuration.Value,
                    primarySkillDef = Survivors.Driver.slugShotgunPrimarySkillDef,
                    secondarySkillDef = Survivors.Driver.slugShotgunSecondarySkillDef,
                    mesh = Assets.slugShotgunMesh,
                    material = Assets.slugShotgunMat,
                    animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                    calloutSoundString = "sfx_driver_callout_shotgun"
                });
                DriverWeaponCatalog.AddWeapon(slugShotgunWeaponDef);
            }

            if (Modules.Config.machineGunEnabled.Value)
            {
                machineGunWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
                {
                    nameToken = "ROB_DRIVER_MACHINEGUN_NAME",
                    descriptionToken = "ROB_DRIVER_MACHINEGUN_DESC",
                    icon = Assets.machineGunWeaponIcon,
                    crosshairPrefab = Assets.LoadCrosshair("Standard"),
                    tier = DriverWeaponTier.Uncommon,
                    baseDuration = Config.machineGunDuration.Value,
                    primarySkillDef = Survivors.Driver.machineGunPrimarySkillDef,
                    secondarySkillDef = Survivors.Driver.machineGunSecondarySkillDef,
                    mesh = Assets.machineGunMesh,
                    material = Assets.machineGunMat,
                    animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                    calloutSoundString = "sfx_driver_callout_machine_gun"
                });
                DriverWeaponCatalog.AddWeapon(machineGunWeaponDef);
            }

            if (Modules.Config.heavyMachineGunEnabled.Value)
            {
                heavyMachineGunWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
                {
                    nameToken = "ROB_DRIVER_HEAVY_MACHINEGUN_NAME",
                    descriptionToken = "ROB_DRIVER_HEAVY_MACHINEGUN_DESC",
                    icon = Assets.heavyMachineGunWeaponIcon,
                    crosshairPrefab = Assets.LoadCrosshair("Standard"),
                    tier = DriverWeaponTier.Uncommon,
                    baseDuration = Config.heavyMachineGunDuration.Value,
                    primarySkillDef = Survivors.Driver.heavyMachineGunPrimarySkillDef,
                    secondarySkillDef = Survivors.Driver.heavyMachineGunSecondarySkillDef,
                    mesh = Assets.heavyMachineGunMesh,
                    material = Assets.heavyMachineGunMat,
                    animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                    calloutSoundString = "sfx_driver_callout_hmg"
                });
                DriverWeaponCatalog.AddWeapon(heavyMachineGunWeaponDef);
            }

            if (Modules.Config.heavyMachineGunEnabled.Value)
            {
                sniperWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
                {
                    nameToken = "ROB_DRIVER_SNIPER_NAME",
                    descriptionToken = "ROB_DRIVER_SNIPER_DESC",
                    icon = Assets.sniperWeaponIcon,
                    crosshairPrefab = Assets.LoadCrosshair("Standard"),
                    tier = DriverWeaponTier.Uncommon,
                    baseDuration = Config.heavyMachineGunDuration.Value,
                    primarySkillDef = Survivors.Driver.sniperPrimarySkillDef,
                    secondarySkillDef = Survivors.Driver.sniperSecondarySkillDef,
                    mesh = Assets.sniperMesh,
                    material = Assets.sniperMat,
                    animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                    calloutSoundString = "sfx_driver_callout_sniper"
                });
                DriverWeaponCatalog.AddWeapon(sniperWeaponDef);
            }

            bazookaWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = "ROB_DRIVER_BAZOOKA_NAME",
                descriptionToken = "ROB_DRIVER_BAZOOKA_DESC",
                icon = Assets.bazookaWeaponIcon,
                crosshairPrefab = bazookaCrosshairPrefab,
                tier = DriverWeaponTier.Uncommon,
                baseDuration = Config.rocketLauncherDuration.Value,
                primarySkillDef = Survivors.Driver.bazookaPrimarySkillDef,
                secondarySkillDef = Survivors.Driver.bazookaSecondarySkillDef,
                mesh = Assets.bazookaMesh,
                material = Assets.bazookaMat,
                animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                calloutSoundString = "sfx_driver_callout_rocket_launcher"
            });
            DriverWeaponCatalog.AddWeapon(bazookaWeaponDef);

            grenadeLauncherWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = "ROB_DRIVER_GRENADELAUNCHER_NAME",
                descriptionToken = "ROB_DRIVER_GRENADELAUNCHER_DESC",
                icon = Assets.grenadeLauncherWeaponIcon,
                crosshairPrefab = grenadeLauncherCrosshairPrefab,
                tier = DriverWeaponTier.Uncommon,
                baseDuration = Config.rocketLauncherDuration.Value,
                primarySkillDef = Survivors.Driver.grenadeLauncherPrimarySkillDef,
                secondarySkillDef = Survivors.Driver.grenadeLauncherSecondarySkillDef,
                mesh = Assets.grenadeLauncherMesh,
                material = Assets.grenadeLauncherMat,
                animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                calloutSoundString = "sfx_driver_callout_grenade_launcher"
            });
            DriverWeaponCatalog.AddWeapon(grenadeLauncherWeaponDef);

            if (Modules.Config.rocketLauncherEnabled.Value)
            {
                rocketLauncherWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
                {
                    nameToken = "ROB_DRIVER_ROCKETLAUNCHER_NAME",
                    descriptionToken = "ROB_DRIVER_ROCKETLAUNCHER_DESC",
                    icon = Assets.rocketLauncherWeaponIcon,
                    crosshairPrefab = rocketLauncherCrosshairPrefab,
                    tier = DriverWeaponTier.Legendary,
                    baseDuration = Config.rocketLauncherDuration.Value,
                    primarySkillDef = Survivors.Driver.rocketLauncherPrimarySkillDef,
                    secondarySkillDef = Survivors.Driver.rocketLauncherSecondarySkillDef,
                    mesh = Assets.rocketLauncherMesh,
                    material = Assets.rocketLauncherMat,
                    animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                    calloutSoundString = "sfx_driver_callout_rocket_launcher"
                });
                DriverWeaponCatalog.AddWeapon(rocketLauncherWeaponDef);
            }

            behemothWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = "ROB_DRIVER_BEHEMOTH_NAME",
                descriptionToken = "ROB_DRIVER_BEHEMOTH_DESC",
                icon = Addressables.LoadAssetAsync<Texture>("RoR2/Base/Behemoth/texBehemothIcon.png").WaitForCompletion(),
                crosshairPrefab = rocketLauncherCrosshairPrefab,
                tier = DriverWeaponTier.Unique,
                baseDuration = Config.rocketLauncherDuration.Value,
                primarySkillDef = Survivors.Driver.behemothPrimarySkillDef,
                secondarySkillDef = Survivors.Driver.behemothSecondarySkillDef,
                mesh = Assets.behemothMesh,
                material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Behemoth/matBehemoth.mat").WaitForCompletion(),
                animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                calloutSoundString = "sfx_driver_callout_rocket_launcher"
            });
            DriverWeaponCatalog.AddWeapon(behemothWeaponDef);
            DriverWeaponCatalog.Behemoth = behemothWeaponDef;

            rocketLauncherAltWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = "ROB_DRIVER_ROCKETLAUNCHER_ALT_NAME",
                descriptionToken = "ROB_DRIVER_ROCKETLAUNCHER_ALT_DESC",
                icon = Assets.rocketLauncherAltWeaponIcon,
                crosshairPrefab = rocketLauncherCrosshairPrefab,
                tier = DriverWeaponTier.Unique,
                baseDuration = Config.rocketLauncherDuration.Value,
                primarySkillDef = Survivors.Driver.rocketLauncherAltPrimarySkillDef,
                secondarySkillDef = Survivors.Driver.rocketLauncherAltSecondarySkillDef,
                mesh = Assets.rocketLauncherMesh,
                material = Assets.rocketLauncherAltMat,
                animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                calloutSoundString = "sfx_driver_callout_rocket_launcher"
            });
            DriverWeaponCatalog.AddWeapon(rocketLauncherAltWeaponDef);
            DriverWeaponCatalog.PrototypeRocketLauncher = rocketLauncherAltWeaponDef;

            armCannonWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = "ROB_DRIVER_ARMCANNON_NAME",
                descriptionToken = "ROB_DRIVER_ARMCANNON_DESC",
                icon = Assets.armCannonWeaponIcon,
                crosshairPrefab = rocketLauncherCrosshairPrefab,
                tier = DriverWeaponTier.Unique,
                baseDuration = Config.rocketLauncherDuration.Value,
                primarySkillDef = Survivors.Driver.armCannonPrimarySkillDef,
                secondarySkillDef = Survivors.Driver.armCannonSecondarySkillDef,
                mesh = Assets.armCannonMesh,
                material = Assets.armCannonMat,
                animationSet = DriverWeaponDef.AnimationSet.Default,
                calloutSoundString = "sfx_driver_callout_generic"
            });
            DriverWeaponCatalog.AddWeapon(armCannonWeaponDef);
            DriverWeaponCatalog.ArmCannon = armCannonWeaponDef;

            plasmaCannonWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = "ROB_DRIVER_PLASMACANNON_NAME",
                descriptionToken = "ROB_DRIVER_PLASMACANNON_DESC",
                icon = Assets.plasmaCannonWeaponIcon,
                crosshairPrefab = rocketLauncherCrosshairPrefab,
                tier = DriverWeaponTier.Void,
                baseDuration = Config.rocketLauncherDuration.Value,
                primarySkillDef = Survivors.Driver.plasmaCannonPrimarySkillDef,
                secondarySkillDef = Survivors.Driver.plasmaCannonSecondarySkillDef,
                mesh = Assets.plasmaCannonMesh,
                material = Assets.plasmaCannonMat,
                animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                calloutSoundString = "sfx_driver_callout_laser"
            });
            DriverWeaponCatalog.AddWeapon(plasmaCannonWeaponDef);
            DriverWeaponCatalog.PlasmaCannon = plasmaCannonWeaponDef;
        }

        internal static GameObject CreateTextPopupEffect(string prefabName, string token, string soundName = "")
        {
            GameObject i = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BearProc").InstantiateClone(prefabName, true);

            i.GetComponent<EffectComponent>().soundName = soundName;
            if (!i.GetComponent<NetworkIdentity>()) i.AddComponent<NetworkIdentity>();

            i.GetComponentInChildren<RoR2.UI.LanguageTextMeshController>().token = token;

            Assets.AddNewEffectDef(i);

            return i;
        }

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            networkSoundEventDefs.Add(networkSoundEventDef);

            return networkSoundEventDef;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            foreach (Renderer i in objectToConvert.GetComponentsInChildren<Renderer>())
            {
                if (i)
                {
                    if (i.material)
                    {
                        i.material.shader = hotpoo;
                    }
                }
            }
        }

        internal static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] rendererInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                rendererInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            return rendererInfos;
        }

        public static GameObject LoadSurvivorModel(string modelName) {
            GameObject model = mainAssetBundle.LoadAsset<GameObject>(modelName);
            if (model == null) {
                Log.Error("Trying to load a null model- check to see if the name in your code matches the name of the object in Unity");
                return null;
            }

            return PrefabAPI.InstantiateClone(model, model.name, false);
        }

        internal static Texture LoadCharacterIcon(string characterName)
        {
            return mainAssetBundle.LoadAsset<Texture>("tex" + characterName + "Icon");
        }

        internal static GameObject LoadCrosshair(string crosshairName)
        {
            return Resources.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        private static GameObject LoadEffect(string resourceName)
        {
            return LoadEffect(resourceName, "", false);
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            return LoadEffect(resourceName, soundName, false);
        }

        private static GameObject LoadEffect(string resourceName, bool parentToTransform)
        {
            return LoadEffect(resourceName, "", parentToTransform);
        }

        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform)
        {
            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            AddNewEffectDef(newEffect, soundName);

            return newEffect;
        }

        internal static void AddNewEffectDef(GameObject effectPrefab)
        {
            AddNewEffectDef(effectPrefab, "");
        }

        internal static void AddNewEffectDef(GameObject effectPrefab, string soundName)
        {
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = soundName;

            effectDefs.Add(newEffectDef);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!commandoMat) commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            Material mat = UnityEngine.Object.Instantiate<Material>(commandoMat);
            Material tempMat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);

            if (!tempMat) return commandoMat;

            mat.name = materialName;
            mat.SetColor("_Color", tempMat.GetColor("_Color"));
            mat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            mat.SetColor("_EmColor", emissionColor);
            mat.SetFloat("_EmPower", emission);
            mat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            mat.SetFloat("_NormalStrength", normalStrength);

            return mat;
        }

        public static Material CreateMaterial(string materialName)
        {
            return Assets.CreateMaterial(materialName, 0f);
        }

        public static Material CreateMaterial(string materialName, float emission)
        {
            return Assets.CreateMaterial(materialName, emission, Color.black);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor)
        {
            return Assets.CreateMaterial(materialName, emission, emissionColor, 0f);
        }
    }
}