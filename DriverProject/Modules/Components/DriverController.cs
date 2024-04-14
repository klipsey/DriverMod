using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RobDriver.Modules.Survivors;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace RobDriver.Modules.Components
{
    public class DriverController : MonoBehaviour
    {
        public ushort syncedWeapon;
        public NetworkInstanceId netId;

        public DriverWeaponDef weaponDef;
        private DriverBulletDef currentBulletDef = BulletTypes.bulletDefs[0];

        public float chargeValue;
        
        //private bool timerStarted;
        private float jamTimer;
        //private EntityStateMachine weaponStateMachine;
        private CharacterBody characterBody;
        private ChildLocator childLocator;
        private CharacterModel characterModel;
        private Animator animator;
        private SkillLocator skillLocator;

        public int maxShellCount = 12;
        public int basePistolAmmo = 20;
        private int currentShell;
        private int currentSlug;
        private GameObject[] shellObjects;
        private GameObject[] slugObjects;

        public Action<DriverController> onWeaponUpdate;

        public float maxWeaponTimer;
        public float weaponTimer;
        public DriverPassive passive;
        private float comboDecay = 1f;
        private DriverWeaponDef pistolWeaponDef;
        private SkinnedMeshRenderer weaponRenderer;

        public float upForce = 9f;
        public float backForce = 2.4f;

        // ooooAAAAAUGHHHHHGAHEM,67TKM
        private SkillDef[] primarySkillOverrides;
        private SkillDef[] secondarySkillOverrides;

        public GameObject crosshairPrefab;

        private int availableSupplyDrops;
        private int lysateCellCount = 0;

        private DriverWeaponDef defaultWeaponDef;
        public DriverBulletDef defaultBulletDef { get; private set; }

        private GameObject muzzleTrail;

        public ParticleSystem machineGunVFX;

        private bool hasPickedUpHammer;
        private GameObject hammerEffectInstance;
        private GameObject hammerEffectInstance2;

        private DriverWeaponDef lastWeaponDef;
        private WeaponNotificationQueue notificationQueue;
        private bool needReload = false;

        public int BulletIndex => this.currentBulletDef.index;
        public bool HasSpecialBullets => this.currentBulletDef.index != 0;
        public DamageType DamageType => this.currentBulletDef.bulletType;
        public DamageAPI.ModdedDamageType ModdedDamageType => this.currentBulletDef.moddedBulletType;

        public DriverBulletDef CurrentBulletInfo => this.currentBulletDef;


        public float ammo
        {
            get
            {
                return this.weaponTimer;
            }
        }

        private void Awake()
        {
            // this was originally used for gun jamming
            /*foreach (EntityStateMachine i in this.GetComponents<EntityStateMachine>())
            {
                if (i && i.customName == "Weapon") this.weaponStateMachine = i;
            }*/
            // probably won't be used but who knows

            this.passive = this.GetComponent<DriverPassive>();
            this.characterBody = this.GetComponent<CharacterBody>();
            ModelLocator modelLocator = this.GetComponent<ModelLocator>();
            this.childLocator = modelLocator.modelBaseTransform.GetComponentInChildren<ChildLocator>();
            this.animator = modelLocator.modelBaseTransform.GetComponentInChildren<Animator>();
            this.characterModel = modelLocator.modelBaseTransform.GetComponentInChildren<CharacterModel>();
            this.skillLocator = this.GetComponent<SkillLocator>();
            this.machineGunVFX = this.childLocator.FindChild("MachineGunVFX").gameObject.GetComponent<ParticleSystem>();

            // really gotta cache this instead of calling a getcomponent on every single weapon pickup
            this.weaponRenderer = this.childLocator.FindChild("PistolModel").GetComponent<SkinnedMeshRenderer>();

            this.GetSkillOverrides();

            this.pistolWeaponDef = DriverWeaponCatalog.GetWeaponFromIndex(Modules.Config.defaultWeaponIndex.Value);
            this.defaultWeaponDef = this.pistolWeaponDef;
            this.defaultBulletDef = BulletTypes.bulletDefs[0];
            this.PickUpWeapon(this.defaultWeaponDef, this.defaultBulletDef, false);

            this.availableSupplyDrops = 1;

            this.CreateHammerEffect();

            this.Invoke("SetInventoryHook", 0.5f);
            this.Invoke("CheckForUpgrade", 2.5f);
        }

        private void GetSkillOverrides()
        {
            // get each skilldef from each weapondef in the catalog...... i hate you
            List<SkillDef> primary = new List<SkillDef>();
            List<SkillDef> secondary = new List<SkillDef>();

            for (int i = 0; i < DriverWeaponCatalog.weaponDefs.Length; i++)
            {
                if (DriverWeaponCatalog.weaponDefs[i])
                {
                    if (DriverWeaponCatalog.weaponDefs[i].primarySkillDef) primary.Add(DriverWeaponCatalog.weaponDefs[i].primarySkillDef);
                    if (DriverWeaponCatalog.weaponDefs[i].secondarySkillDef) secondary.Add(DriverWeaponCatalog.weaponDefs[i].secondarySkillDef);
                }
            }

            this.primarySkillOverrides = primary.ToArray();
            this.secondarySkillOverrides = secondary.ToArray();
        }

        private void Start()
        {
            this.InitShells();

            if (this.passive.isPistolOnly)
            {
                this.maxWeaponTimer = 26f;
                this.weaponTimer = 26f;
            }

            if (this.passive.isBullets || this.passive.isRyan)
            {
                SetBulletAmmo();
            }
        }

        private void SetInventoryHook()
        {
            // swag
            if (this.skillLocator.utility.skillDef == Modules.Survivors.Driver.skateboardSkillDef) this.childLocator.FindChild("SkateboardBackModel").gameObject.SetActive(true);

            if (this.characterBody && this.characterBody.master && this.characterBody.master.inventory)
            {
                this.characterBody.master.inventory.onItemAddedClient += this.Inventory_onItemAddedClient;
                this.characterBody.master.inventory.onInventoryChanged += this.Inventory_onInventoryChanged;
            }

            this.defaultWeaponDef = DriverWeaponCatalog.GetWeaponFromIndex(Modules.Config.defaultWeaponIndex.Value);
            if (DriverWeaponCatalog.IsWeaponPistol(defaultWeaponDef)) pistolWeaponDef = defaultWeaponDef;

            PickUpWeapon(defaultWeaponDef, defaultBulletDef, false);

            // upgrade shit
            this.CheckForLysateCell();

            this.CheckForNeedler();

            this.CheckForStoredWeapon();
        }

        private void CheckForStoredWeapon()
        {
            if (NetworkServer.active)
            {
                if (this.characterBody && this.characterBody.master)
                {
                    DriverWeaponTracker weaponTracker = this._weaponTracker;
                    if (weaponTracker.isStoringWeapon)
                    {
                        DriverWeaponTracker.StoredWeapon storedWeapon = weaponTracker.RetrieveWeapon();
                        this.ServerGetStoredWeapon(this, storedWeapon.weaponDef, storedWeapon.ammo, storedWeapon.ammoIndex);
                    }
                }
            }
        }

        private void StoreWeapon()
        {
            if (this.characterBody && this.characterBody.master)
            {
                DriverWeaponTracker weaponTracker = this._weaponTracker;
                weaponTracker.StoreWeapon(this.weaponDef, this.ammo, (short)this.BulletIndex);
            }
        }

        private DriverWeaponTracker _weaponTracker
        {
            get
            {
                if (this.characterBody && this.characterBody.master)
                {
                    DriverWeaponTracker i = this.characterBody.master.GetComponent<DriverWeaponTracker>();
                    if (!i) i = this.characterBody.master.gameObject.AddComponent<DriverWeaponTracker>();
                    return i;
                }
                return null;
            }
        }

        private void CheckForLysateCell()
        {
            if (this.characterBody && this.characterBody.master && this.characterBody.master.inventory)
            {
                int count = this.characterBody.master.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid);
                if (count > this.lysateCellCount)
                {
                    int diff = count - this.lysateCellCount;
                    this.availableSupplyDrops += diff;
                    this.lysateCellCount = count;
                }
            }
        }

        private void CheckForNeedler()
        {
            if (this.hasPickedUpHammer) return;

            if (this.characterBody && this.characterBody.master && this.characterBody.master.inventory)
            {
                DriverWeaponDef desiredWeapon = this.pistolWeaponDef;

                if (this.characterBody.master.inventory.GetItemCount(RoR2Content.Items.TitanGoldDuringTP) > 0 &&
                    (this.defaultWeaponDef == DriverWeaponCatalog.Pistol || this.defaultWeaponDef == DriverWeaponCatalog.PyriteGun))
                {
                    desiredWeapon = DriverWeaponCatalog.PyriteGun;
                }

                if (this.characterBody.master.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) > 0)
                {
                    desiredWeapon = DriverWeaponCatalog.Needler;
                }

                // this seems sus af
                if (this.maxWeaponTimer <= 0f && desiredWeapon != this.defaultWeaponDef)
                {
                    this.defaultWeaponDef = desiredWeapon;
                    this.PickUpWeapon(this.defaultWeaponDef, this.currentBulletDef, false);
                }
            }
        }

        private void Inventory_onInventoryChanged()
        {
            this.CheckForNeedler();
            this.CheckForLysateCell();
        }

        private void CheckForUpgrade()
        {
            if (!Modules.Config.enablePistolUpgrade.Value) return;

            if (this.hasPickedUpHammer) return;

            // upgrade your pistol for run-ending bosses; this is more interesting than just injecting weapon drops imo
            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene.name == "moon" || currentScene.name == "moon2")
            {
                this.UpgradeToLunar();
            }

            if (currentScene.name == "voidraid")
            {
                this.UpgradeToVoid();
            }

            if (currentScene.name == "limbo")
            {
                this.UpgradeToLunar();
            }
        }

        private bool TryUpgradeWeapon(DriverWeaponDef newWeaponDef)
        {
            if (this.passive && this.passive.isDefault) return false;
            if (this.characterBody && this.characterBody.inventory && this.characterBody.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) > 0) return false;

            this.pistolWeaponDef = newWeaponDef;
            this.defaultWeaponDef = newWeaponDef;

            return true;
        }

        private void UpgradeToLunar()
        {
            bool success = this.TryUpgradeWeapon(DriverWeaponCatalog.LunarPistol);

            if (!success) return;
            this.PickUpWeapon(this.defaultWeaponDef, this.currentBulletDef, false);

            this.TryPickupNotification(true);

            EffectData effectData = new EffectData
            {
                origin = this.childLocator.FindChild("PistolMuzzle").position,
                rotation = Quaternion.identity
            };
            EffectManager.SpawnEffect(Modules.Assets.upgradeEffectPrefab, effectData, false);

            EffectManager.SpawnEffect(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LunarGolem/LunarGolemTwinShotExplosion.prefab").WaitForCompletion(),
                new EffectData
                {
                    origin = this.childLocator.FindChild("Pistol").position,
                    rotation = Quaternion.identity,
                    scale = 1f
                }, false);
        }

        private void UpgradeToVoid()
        {
            bool success = this.TryUpgradeWeapon(DriverWeaponCatalog.VoidPistol);

            if (!success) return;
            this.PickUpWeapon(this.defaultWeaponDef, this.currentBulletDef, false);

            this.TryPickupNotification(true);

            EffectData effectData = new EffectData
            {
                origin = this.childLocator.FindChild("PistolMuzzle").position,
                rotation = Quaternion.identity
            };
            EffectManager.SpawnEffect(Modules.Assets.upgradeEffectPrefab, effectData, false);

            EffectManager.SpawnEffect(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterExplosion.prefab").WaitForCompletion(),
                new EffectData
                {
                    origin = this.childLocator.FindChild("Pistol").position,
                    rotation = Quaternion.identity,
                    scale = 1f
                }, false);
        }

        private void Inventory_onItemAddedClient(ItemIndex itemIndex)
        {
            if (DriverPlugin.litInstalled) // funny compat :-)
            {
                if (this.IsItemGoldenGun(itemIndex))
                {
                    this.ServerPickUpWeapon(this, DriverWeaponCatalog.GoldenGun, false, -1, false);
                }
            }

            if (DriverPlugin.classicItemsInstalled) // not funny anymore
            {
                if (this.IsItemGoldenGun2(itemIndex))
                {
                    this.ServerPickUpWeapon(this, DriverWeaponCatalog.GoldenGun, false, -1, false);
                }
            }

            if (itemIndex == RoR2Content.Items.Behemoth.itemIndex)
            {
                this.ServerPickUpWeapon(this, DriverWeaponCatalog.Behemoth, false, -1, false);
            }
        }

        private bool IsItemGoldenGun(ItemIndex itemIndex)
        {
            // golden gun disabled- forgot to account for that whoops
            if (LostInTransit.LITContent.Items.GoldenGun == null) return false;

            if (itemIndex == LostInTransit.LITContent.Items.GoldenGun.itemIndex) return true;
            return false;
        }

        private bool IsItemGoldenGun2(ItemIndex itemIndex)
        {
            // golden gun disabled- forgot to account for that whoops
            if (ClassicItemsReturns.Items.GoldenGun.Instance == null) return false;
            if (ClassicItemsReturns.Items.GoldenGun.Instance.ItemDef == null) return false;

            if (itemIndex == ClassicItemsReturns.Items.GoldenGun.Instance.ItemDef.itemIndex) return true;
            return false;
        }
        private void CreateHammerEffect()
        {
            #region clone mithrix effect
            this.hammerEffectInstance = GameObject.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponentInChildren<ChildLocator>().FindChild("Phase3HammerFX").gameObject);
            this.hammerEffectInstance.transform.parent = this.childLocator.FindChild("GunR");
            this.hammerEffectInstance.transform.localScale = Vector3.one * 0.0002f;
            this.hammerEffectInstance.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 90f));
            this.hammerEffectInstance.transform.localPosition = new Vector3(0f, 1.6f, 0.05f);
            this.hammerEffectInstance.gameObject.SetActive(true);

            this.hammerEffectInstance.transform.Find("Amb_Fire_Ps, Left").localScale = Vector3.one * 0.6f;
            this.hammerEffectInstance.transform.Find("Amb_Fire_Ps, Right").localScale = Vector3.one * 0.6f;
            this.hammerEffectInstance.transform.Find("Core, Light").localScale = Vector3.one * 0.1f;
            this.hammerEffectInstance.transform.Find("Blocks, Spinny").localScale = Vector3.one * 0.4f;
            this.hammerEffectInstance.transform.Find("Sparks").localScale = Vector3.one * 0.4f;
            #endregion

            this.hammerEffectInstance2 = GameObject.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarWispBody").GetComponentInChildren<CharacterModel>().transform.Find("Amb_Fire_Ps").gameObject);
            this.hammerEffectInstance2.transform.parent = this.childLocator.FindChild("HandL");
            this.hammerEffectInstance2.transform.localPosition = Vector3.zero;
            this.hammerEffectInstance2.transform.localRotation = Quaternion.identity;
            this.hammerEffectInstance2.transform.localScale *= 0.25f;

            this.hammerEffectInstance.SetActive(false);
            this.hammerEffectInstance2.SetActive(false);
        }

        public void ConsumeAmmo(float multiplier = 1f, bool scaleWithAttackSpeed = true)
        {
            if (this.characterBody && this.characterBody.HasBuff(RoR2Content.Buffs.NoCooldowns)) return;

            if (this.characterBody && this.characterBody.inventory && scaleWithAttackSpeed)
            {
                int alienHeadCount = this.characterBody.inventory.GetItemCount(RoR2Content.Items.AlienHead);
                if (alienHeadCount > 0)
                {
                    for (int i = 0; i < alienHeadCount; i++)
                    {
                        if (DriverPlugin.greenAlienHeadInstalled)
                        {
                            multiplier *= 0.85f;
                        }
                        else
                        {
                            multiplier *= 0.75f;
                        }
                    }
                }
            }

            if (scaleWithAttackSpeed) this.weaponTimer -= multiplier / this.characterBody.attackSpeed;
            else this.weaponTimer -= multiplier;
        }

        private void SetBulletAmmo(float ammo = -1, bool cutAmmo = false)
        {
            float shotCount;
            // set ammo for non-pistols
            if (!DriverWeaponCatalog.IsWeaponPistol(weaponDef))
            {
                shotCount = this.weaponDef.shotCount;

                if (Modules.Config.GetWeaponConfig(this.weaponDef)) shotCount = Modules.Config.GetWeaponConfigShotCount(this.weaponDef);
            }
            // set ammo for pistols
            else if (this.HasSpecialBullets)
            {
                shotCount = basePistolAmmo;
            }
            // default pistol
            else
            {
                this.weaponTimer = 0f;
                this.maxWeaponTimer = 0f;
                return;
            }

            if (Modules.Config.backupMagExtendDuration.Value)
            {
                if (this.characterBody && this.characterBody.inventory)
                {
                    shotCount += (0.5f * this.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine));
                }
            }
            if(cutAmmo) shotCount *= 0.5f;
            this.weaponTimer = ammo == -1 ? shotCount : ammo;
            if (cutAmmo) shotCount /= 0.5f;
            this.maxWeaponTimer = shotCount;
        }

        private void FixedUpdate()
        {
            this.jamTimer = Mathf.Clamp(this.jamTimer - (2f * Time.fixedDeltaTime), 0f, Mathf.Infinity);

            if (this.weaponTimer <= 0f && this.maxWeaponTimer > 0f)
            {
                if (this.passive.isPistolOnly)
                {
                    if (!this.needReload)
                    {
                        this.needReload = true;
                        this.skillLocator.primary.SetSkillOverride(this, Driver.pistolReloadSkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                    }
                }
                else if(this.passive.isBullets || this.passive.isRyan)
                {
                    if (this.HasSpecialBullets)
                    {
                        Buffs.RemoveBuff(this.characterBody, this.BulletIndex);
                        currentBulletDef = BulletTypes.bulletDefs[0];

                        UnityEngine.Object.Destroy(muzzleTrail.gameObject);
                        this.muzzleTrail = null;
                    }
                    if (DriverWeaponCatalog.IsWeaponPistol(weaponDef))
                    {
                        if (!needReload)
                        {
                            this.needReload = true;
                            this.skillLocator.primary.SetSkillOverride(this, Driver.pistolReloadSkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                        }
                    }
                    else
                    {
                        this.ReturnToDefaultWeapon();
                    }
                }
                else
                {
                    this.ReturnToDefaultWeapon();
                }
            }
            this.CheckSupplyDrop();
        }

        private void CheckSupplyDrop()
        {
            if (this.skillLocator)
            {
                if (this.skillLocator.special.baseSkill.skillNameToken == DriverPlugin.developerPrefix + "_DRIVER_BODY_SPECIAL_SUPPLY_DROP_LEGACY_NAME")
                {
                    if (this.characterBody && this.characterBody.master && this.characterBody.master.inventory)
                    {
                        if (this.characterBody.master.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement) > 0)
                        {
                            return;
                        }
                    }

                    this.skillLocator.special.stock = this.availableSupplyDrops;
                }
            }
        }

        public void ConsumeSupplyDrop()
        {
            this.availableSupplyDrops--;
        }

        public void ServerResetTimer()
        {
            // just pick up the same weapon again cuz i don't feel like writing even more netcode to sync this
            this.ServerPickUpWeapon(this, this.weaponDef, false, this.BulletIndex, false);
        }

        public void ServerGetStoredWeapon(DriverController driverController, DriverWeaponDef newWeapon, float ammo, int ammoIndex)
        {
            NetworkIdentity identity = driverController.gameObject.GetComponent<NetworkIdentity>();
            if (!identity) return;

            new SyncStoredWeapon(identity.netId, newWeapon.index, ammo, (short)ammoIndex).Send(NetworkDestination.Clients);
        }

        public void ServerPickUpWeapon(DriverController driverController, DriverWeaponDef newWeapon, bool cutAmmo, int ammoIndex, bool isNewAmmoType)
        {
            NetworkIdentity identity = driverController.gameObject.GetComponent<NetworkIdentity>();
            if (!identity) return;

            new SyncWeapon(identity.netId, newWeapon.index, cutAmmo, ammoIndex, isNewAmmoType).Send(NetworkDestination.Clients);
        }

        private void ReturnToDefaultWeapon()
        {
            this.DiscardWeapon();
            if (this.hasPickedUpHammer)
            {
                this.PickUpWeapon(DriverWeaponCatalog.LunarHammer, this.currentBulletDef, false);
            }
            else
            {
                this.PickUpWeapon(this.defaultWeaponDef, this.currentBulletDef, false);
            }
        }

        private void DiscardWeapon()
        {
            // just create the effect here
            GameObject newEffect = GameObject.Instantiate(Modules.Assets.discardedWeaponEffect);
            newEffect.GetComponent<DiscardedWeaponComponent>().Init(this.weaponDef, (this.characterBody.characterDirection.forward * -this.backForce) + (Vector3.up * this.upForce) + this.characterBody.characterMotor.velocity);
            newEffect.transform.rotation = this.characterBody.modelLocator.modelTransform.rotation;
            newEffect.transform.position = this.childLocator.FindChild("Pistol").position + (Vector3.up * 0.5f);
        }
        
        public void FinishReload()
        {
            if (needReload) this.skillLocator.primary.UnsetSkillOverride(this, Driver.pistolReloadSkillDef, GenericSkill.SkillOverridePriority.Upgrade);
            needReload = false;

            if(this.passive.isPistolOnly)
            {
                this.weaponTimer = 26f;
                this.maxWeaponTimer = 26f;
            }

            if (this.passive.isBullets || this.passive.isRyan)
            {
                SetBulletAmmo();
            }

            // notify hud
            this.onWeaponUpdate?.Invoke(this);
        }

        // network interface
        public void PickUpWeaponDrop(DriverWeaponDef newWeapon, float ammo, int ammoIndex, bool isNewAmmoType, bool cutAmmo = false)
        {
            if (this.passive.isPistolOnly)
            {
                this.FinishReload();
            }
            else if (this.passive.isBullets)
            {
                LoadNewBullets(ammoIndex, cutAmmo, ammo);
            }
            else if (this.passive.isRyan)
            {
                // change ammo type
                if (isNewAmmoType) LoadNewBullets(ammoIndex, cutAmmo, ammo);
                // picked up new weapon
                else PickUpWeapon(newWeapon, currentBulletDef, cutAmmo, ammo);
            }
            else
            {
                PickUpWeapon(newWeapon, currentBulletDef, cutAmmo, ammo);
            }
        }

        private void PickUpWeapon(DriverWeaponDef newWeapon, DriverBulletDef newBullet, bool cutAmmo, float ammo = -1f)
        {
            this.weaponDef = newWeapon;

            this.currentBulletDef = newBullet;

            if (newWeapon == DriverWeaponCatalog.LunarHammer) this.hasPickedUpHammer = true; // hardcoding the mithrix hammer as default once picked up. fuck it

            this.EquipWeapon(ammo, cutAmmo);

            this.TryCallout();

            this.TryPickupNotification();

            this.onWeaponUpdate?.Invoke(this);
        }

        private void LoadNewBullets(int newAmmoIndex, bool cutAmmo, float ammo = -1)
        {
            if (this.needReload) this.skillLocator.primary.UnsetSkillOverride(this, RobDriver.Modules.Survivors.Driver.pistolReloadSkillDef, GenericSkill.SkillOverridePriority.Upgrade);
            needReload = false;

            //Just in case
            if (muzzleTrail)
            {
                UnityEngine.Object.Destroy(muzzleTrail);
                muzzleTrail = null;
            }

            Buffs.RemoveBuff(this.characterBody, this.BulletIndex);
            Buffs.GiveBuff(this.characterBody, newAmmoIndex);

            currentBulletDef = BulletTypes.GetBulletDefFromIndex(newAmmoIndex);

            SetBulletAmmo(ammo, cutAmmo);

            Transform muzzleTransform;
            if (DriverWeaponCatalog.IsWeaponPistol(weaponDef)) muzzleTransform = this.childLocator.FindChild("PistolMuzzle");
            else muzzleTransform = this.childLocator.FindChild("ShotgunMuzzle");

            muzzleTrail = Assets.defaultMuzzleTrail;
            muzzleTrail.GetComponent<TrailRenderer>().startColor = currentBulletDef.trailColor;
            muzzleTrail = UnityEngine.Object.Instantiate(muzzleTrail, muzzleTransform);

            // notify hud
            this.onWeaponUpdate?.Invoke(this);
        }

        private void TryPickupNotification(bool force = false)
        {
            if (!Modules.Config.enablePickupNotifications.Value) return;

            // attempt to add the component if it's not there
            if (!this.notificationQueue && this.characterBody.master)
            {
                this.notificationQueue = this.characterBody.master.GetComponent<WeaponNotificationQueue>();
               // if (!this.notificationQueue) this.notificationQueue = this.characterBody.master.gameObject.AddComponent<WeaponNotificationQueue>();
            }

            if (this.notificationQueue)
            {
                if (force)
                {
                    WeaponNotificationQueue.PushWeaponNotification(this.characterBody.master, this.weaponDef.index);
                    this.lastWeaponDef = this.weaponDef;
                    return;
                }

                if (this.weaponDef != this.lastWeaponDef)
                {
                    if (this.weaponDef != this.defaultWeaponDef && this.weaponDef != this.pistolWeaponDef)
                    {
                        WeaponNotificationQueue.PushWeaponNotification(this.characterBody.master, this.weaponDef.index);
                    }
                }
                this.lastWeaponDef = this.weaponDef;
            }
        }

        private void TryCallout()
        {
            if (this.weaponDef && this.weaponDef.calloutSoundString != "")
            {
                if (Modules.Config.weaponCallouts.Value)
                {
                    Util.PlaySound(this.weaponDef.calloutSoundString, this.gameObject);
                }
            }
        }

        private void EquipWeapon(float ammo = -1f, bool cutAmmo = false)
        {
            // unset all the overrides....
            for (int i = 0; i < this.primarySkillOverrides.Length; i++)
            {
                if (this.primarySkillOverrides[i])
                {
                    this.skillLocator.primary.UnsetSkillOverride(this.skillLocator.primary, this.primarySkillOverrides[i], GenericSkill.SkillOverridePriority.Contextual);
                }
            }

            for (int i = 0; i < this.secondarySkillOverrides.Length; i++)
            {
                if (this.secondarySkillOverrides[i])
                {
                    this.skillLocator.secondary.UnsetSkillOverride(this.skillLocator.secondary, this.secondarySkillOverrides[i], GenericSkill.SkillOverridePriority.Contextual);
                }
            }
            // fuck this, seriously

            // set new overrides
            if (this.weaponDef.primarySkillDef) this.skillLocator.primary.SetSkillOverride(this.skillLocator.primary, this.weaponDef.primarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
            if (this.weaponDef.secondarySkillDef) this.skillLocator.secondary.SetSkillOverride(this.skillLocator.secondary, this.weaponDef.secondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

            // model swap
            if (this.weaponDef.mesh)
            {
                this.weaponRenderer.sharedMesh = this.weaponDef.mesh;
                this.characterModel.baseRendererInfos[this.characterModel.baseRendererInfos.Length - 1].defaultMaterial = this.weaponDef.material;
            }
            else
            {
                if (this.weaponDef.animationSet == DriverWeaponDef.AnimationSet.TwoHanded)
                {
                    this.weaponRenderer.sharedMesh = DriverWeaponCatalog.Behemoth.mesh;
                    this.characterModel.baseRendererInfos[this.characterModel.baseRendererInfos.Length - 1].defaultMaterial = DriverWeaponCatalog.Behemoth.material;
                }
                else
                {
                    this.weaponRenderer.sharedMesh = DriverWeaponCatalog.Pistol.mesh;
                    this.characterModel.baseRendererInfos[this.characterModel.baseRendererInfos.Length - 1].defaultMaterial = DriverWeaponCatalog.Pistol.material;
                }
            }

            // pistol ammo scales off attack speed, let SetBulletAmmo handle it
            // what a mess
            if (this.passive.isBullets || (this.passive.isRyan && DriverWeaponCatalog.IsWeaponPistol(weaponDef)))
            {
                SetBulletAmmo(ammo, cutAmmo);
            }
            else
            {   
                // timer for default passive
                float duration = this.weaponDef.shotCount;

                if (Modules.Config.GetWeaponConfig(this.weaponDef)) duration = Modules.Config.GetWeaponConfigShotCount(this.weaponDef);

                if (Modules.Config.backupMagExtendDuration.Value)
                {
                    if (this.characterBody && this.characterBody.inventory)
                    {
                        duration += (0.5f * this.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine));
                    }
                }

                // infinite ammo
                if (DriverWeaponCatalog.IsWeaponPistol(weaponDef) || this.weaponDef.shotCount == 0) duration = 0f;

                // static pistol ammo
                if (this.passive.isPistolOnly) duration = 26f;

                // set max timer to weaponDef.shotCount
                if (cutAmmo) duration *= 0.5f;
                this.weaponTimer = duration;
                if (cutAmmo) duration /= 0.5f;
                this.maxWeaponTimer = duration;
            }

            // cut ammo
            if (ammo != -1f) this.weaponTimer = ammo;

            // crosshair
            this.crosshairPrefab = this.weaponDef.crosshairPrefab;
            this.characterBody._defaultCrosshairPrefab = this.crosshairPrefab;

            // animator layer
            switch (this.weaponDef.animationSet)
            {
                case DriverWeaponDef.AnimationSet.Default:
                    this.EnableLayer("");
                    break;
                case DriverWeaponDef.AnimationSet.TwoHanded:
                    this.EnableLayer("Body, Shotgun");
                    break;
                case DriverWeaponDef.AnimationSet.BigMelee:
                    this.EnableLayer("Body, Hammer");
                    break;
            }

            // extra shit
            if (this.hammerEffectInstance && this.hammerEffectInstance2)
            {
                if (this.weaponDef == DriverWeaponCatalog.LunarHammer)
                {
                    this.hammerEffectInstance.SetActive(true);
                    this.hammerEffectInstance2.SetActive(false); // this one needs to be remade from scratch ig
                }
                else
                {
                    this.hammerEffectInstance.SetActive(false);
                    this.hammerEffectInstance2.SetActive(false);
                }
            }
        }

        private void EnableLayer(string layerName)
        {
            if (!this.animator) return;

            this.animator.SetLayerWeight(this.animator.GetLayerIndex("Body, Shotgun"), 0f);
            this.animator.SetLayerWeight(this.animator.GetLayerIndex("Body, Hammer"), 0f);

            if (layerName == "") return;

            this.animator.SetLayerWeight(this.animator.GetLayerIndex(layerName), 1f);
        }

        public bool AddJamBuildup(bool jammed = false)
        {
            this.jamTimer += 3f;

            if (this.jamTimer >= 10f)
            {
                this.jamTimer = 0f;
                jammed = true;
            }

            return jammed;
        }

        private void InitShells()
        {
            this.currentShell = 0;

            this.shellObjects = new GameObject[this.maxShellCount + 1];

            GameObject desiredShell = Assets.shotgunShell;

            for (int i = 0; i < this.maxShellCount; i++)
            {
                this.shellObjects[i] = GameObject.Instantiate(desiredShell, this.childLocator.FindChild("Pistol"), false);
                this.shellObjects[i].transform.localScale = Vector3.one * 1.1f;
                this.shellObjects[i].SetActive(false);
                this.shellObjects[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

                this.shellObjects[i].layer = LayerIndex.ragdoll.intVal;
                this.shellObjects[i].transform.GetChild(0).gameObject.layer = LayerIndex.ragdoll.intVal;
            }

            this.currentSlug = 0;

            this.slugObjects = new GameObject[this.maxShellCount + 1];

            desiredShell = Assets.shotgunSlug;

            for (int i = 0; i < this.maxShellCount; i++)
            {
                this.slugObjects[i] = GameObject.Instantiate(desiredShell, this.childLocator.FindChild("Pistol"), false);
                this.slugObjects[i].transform.localScale = Vector3.one * 1.2f;
                this.slugObjects[i].SetActive(false);
                this.slugObjects[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

                this.slugObjects[i].layer = LayerIndex.ragdoll.intVal;
                this.slugObjects[i].transform.GetChild(0).gameObject.layer = LayerIndex.ragdoll.intVal;
            }
        }

        public void DropShell(Vector3 force)
        {
            if (this.shellObjects == null) return;

            if (this.shellObjects[this.currentShell] == null) return;

            Transform origin = this.childLocator.FindChild("Pistol");

            this.shellObjects[this.currentShell].SetActive(false);

            this.shellObjects[this.currentShell].transform.position = origin.position;
            this.shellObjects[this.currentShell].transform.SetParent(null);

            this.shellObjects[this.currentShell].SetActive(true);

            Rigidbody rb = this.shellObjects[this.currentShell].gameObject.GetComponent<Rigidbody>();
            if (rb) rb.velocity = force;

            this.currentShell++;
            if (this.currentShell >= this.maxShellCount) this.currentShell = 0;
        }

        public void DropSlug(Vector3 force)
        {
            if (this.slugObjects == null) return;

            if (this.slugObjects[this.currentSlug] == null) return;

            Transform origin = this.childLocator.FindChild("Pistol");

            this.slugObjects[this.currentSlug].SetActive(false);

            this.slugObjects[this.currentSlug].transform.position = origin.position;
            this.slugObjects[this.currentSlug].transform.SetParent(null);

            this.slugObjects[this.currentSlug].SetActive(true);

            Rigidbody rb = this.slugObjects[this.currentSlug].gameObject.GetComponent<Rigidbody>();
            if (rb) rb.velocity = force;

            this.currentSlug++;
            if (this.currentSlug >= this.maxShellCount) this.currentSlug = 0;
        }

        private void OnDestroy()
        {
            if (this.shellObjects != null && this.shellObjects.Length > 0)
            {
                for (int i = 0; i < this.shellObjects.Length; i++)
                {
                    if (this.shellObjects[i]) Destroy(this.shellObjects[i]);
                }
            }

            if (this.slugObjects != null && this.slugObjects.Length > 0)
            {
                for (int i = 0; i < this.slugObjects.Length; i++)
                {
                    if (this.slugObjects[i]) Destroy(this.slugObjects[i]);
                }
            }

            if (this.characterBody && this.characterBody.master && this.characterBody.master.inventory)
            {
                this.characterBody.master.inventory.onItemAddedClient -= this.Inventory_onItemAddedClient;
                this.characterBody.master.inventory.onInventoryChanged -= this.Inventory_onInventoryChanged;
            }

            if (NetworkServer.active)
            {
                this.StoreWeapon();
            }
        }

        public void ExtendTimer()
        {
            return;
            // fuck, i have to network this before adding it actually
            if (this.weaponTimer > 0f && this.maxWeaponTimer > 0f)
            {
                float amount = 1f * this.comboDecay;

                this.comboDecay = Mathf.Clamp(this.comboDecay - 0.1f, 0f, 1f);

                this.weaponTimer = Mathf.Clamp(this.weaponTimer + amount, 0f, this.maxWeaponTimer);
            }
        }
    }
}
