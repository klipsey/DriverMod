using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// this is definitely the worst way to do this
// please make a system for this eventually
// don't just stack on a thousand entries in this fucking enum
/*public enum DriverWeapon
{
    Default,
    Shotgun,
    MachineGun,
    RocketLauncher
}*/
// my wrongs have finally been righted

// and heeeeere we go again
public enum SkateboardState
{
    Inactive,
    Transitioning,
    Active
}

namespace RobDriver.Modules.Components
{
    public class DriverController : MonoBehaviour
    {
        public ushort syncedWeapon;
        public NetworkInstanceId netId;

        public DriverWeaponDef weaponDef;

        public float chargeValue;
        
        private bool timerStarted;
        private float jamTimer;
        //private EntityStateMachine weaponStateMachine;
        private CharacterBody characterBody;
        private ChildLocator childLocator;
        private CharacterModel characterModel;
        private Animator animator;
        private SkillLocator skillLocator;

        public int maxShellCount = 12;
        private int currentShell;
        private GameObject[] shellObjects;

        public Action<DriverController> onWeaponUpdate;

        public float maxWeaponTimer;
        public float weaponTimer;
        private float comboDecay = 1f;
        private DriverWeaponDef pistolWeaponDef;
        private SkinnedMeshRenderer weaponRenderer;

        // ooooAAAAAUGHHHHHGAHEM,67TKM
        private SkillDef[] primarySkillOverrides;
        private SkillDef[] secondarySkillOverrides;

        public GameObject crosshairPrefab;

        private int availableSupplyDrops;

        private SkateboardState skateboardState;// this could have easily been a bool
        private GameObject skateboardObject;
        private GameObject skateboardBackObject;

        private void Awake()
        {
            // this was originally used for gun jamming
            /*foreach (EntityStateMachine i in this.GetComponents<EntityStateMachine>())
            {
                if (i && i.customName == "Weapon") this.weaponStateMachine = i;
            }*/
            // probably won't be used but who knows

            this.characterBody = this.GetComponent<CharacterBody>();
            ModelLocator modelLocator = this.GetComponent<ModelLocator>();
            this.childLocator = modelLocator.modelBaseTransform.GetComponentInChildren<ChildLocator>();
            this.animator = modelLocator.modelBaseTransform.GetComponentInChildren<Animator>();
            this.characterModel = modelLocator.modelBaseTransform.GetComponentInChildren<CharacterModel>();
            this.skillLocator = this.GetComponent<SkillLocator>();

            // really gotta cache this instead of calling a getcomponent on every single weapon pickup
            this.weaponRenderer = this.childLocator.FindChild("PistolModel").GetComponent<SkinnedMeshRenderer>();

            this.GetSkillOverrides();

            this.pistolWeaponDef = DriverWeaponCatalog.GetWeaponFromIndex(0);
            this.PickUpWeapon(this.pistolWeaponDef);

            this.availableSupplyDrops = 1;

            this.skateboardObject = this.childLocator.FindChild("SkateboardModel").gameObject;
            this.skateboardBackObject = this.childLocator.FindChild("SkateboardBackModel").gameObject;

            this.ToggleSkateboard(SkateboardState.Inactive);
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

            this.Invoke("SetInventoryHook", 0.5f);
        }

        private void Start()
        {
            this.InitShells();
        }

        private void SetInventoryHook()
        {
            if (this.characterBody && this.characterBody.master && this.characterBody.master.inventory)
            {
                this.characterBody.master.inventory.onItemAddedClient += this.Inventory_onItemAddedClient;
            }
        }

        private void Inventory_onItemAddedClient(ItemIndex itemIndex)
        {
            if (DriverPlugin.litInstalled) // funny compat :-)
            {
                if (this.IsItemGoldenGun(itemIndex))
                {
                    this.ServerPickUpWeapon(DriverWeaponCatalog.GoldenGun, this);
                }
            }

            if (itemIndex == RoR2Content.Items.Behemoth.itemIndex)
            {
                this.ServerPickUpWeapon(DriverWeaponCatalog.Behemoth, this);
            }
        }

        private bool IsItemGoldenGun(ItemIndex itemIndex)
        {
            // golden gun disabled- forgot to account for that whoops
            if (LostInTransit.LITContent.Items.GoldenGun == null) return false;

            if (itemIndex == LostInTransit.LITContent.Items.GoldenGun.itemIndex) return true;
            return false;
        }

        public void StartTimer()
        {
            this.timerStarted = true;
        }

        public void ToggleSkateboard(SkateboardState newState)
        {
            return;

            this.skateboardState = newState;

            this.skateboardObject.SetActive(false);
            this.skateboardBackObject.SetActive(false);

            if (this.skillLocator.utility.skillDef.skillNameToken != DriverPlugin.developerPrefix + "UTILITY_SKATEBOARD_NAME") return;

            switch (this.skateboardState)
            {
                case SkateboardState.Inactive:
                    this.skateboardObject.SetActive(false);
                    this.skateboardBackObject.SetActive(true);
                    break;
                case SkateboardState.Active:
                    this.skateboardObject.SetActive(true);
                    this.skateboardBackObject.SetActive(false);
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (this.timerStarted) this.weaponTimer -= Time.fixedDeltaTime;
            this.jamTimer = Mathf.Clamp(this.jamTimer - (2f * Time.fixedDeltaTime), 0f, Mathf.Infinity);

            if (this.weaponTimer <= 0f && this.weaponDef != this.pistolWeaponDef)
            {
                this.PickUpWeapon(this.pistolWeaponDef);
            }

            this.CheckSupplyDrop();
        }

        private void CheckSupplyDrop()
        {
            if (this.skillLocator)
            {
                if (this.skillLocator.special.baseSkill.skillNameToken == DriverPlugin.developerPrefix + "_DRIVER_BODY_SPECIAL_SUPPLY_DROP_NAME")
                {
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
            this.ServerPickUpWeapon(this.weaponDef, this);
        }

        public void ServerPickUpWeapon(DriverWeaponDef newWeapon, DriverController driverController)
        {
            NetworkIdentity identity = driverController.gameObject.GetComponent<NetworkIdentity>();
            if (!identity) return;

            new SyncWeapon(identity.netId, newWeapon.index).Send(NetworkDestination.Clients);
        }

        public void PickUpWeapon(DriverWeaponDef newWeapon)
        {
            this.timerStarted = false;
            this.weaponDef = newWeapon;
            this.EquipWeapon();

            this.TryCallout();

            if (this.onWeaponUpdate == null) return;
            this.onWeaponUpdate(this);
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

        private void EquipWeapon()
        {
            // unset all the overrides....
            for (int i = 0; i < this.primarySkillOverrides.Length; i++)
            {
                if (this.primarySkillOverrides[i])
                {
                    this.skillLocator.primary.UnsetSkillOverride(this.skillLocator.primary, this.primarySkillOverrides[i], GenericSkill.SkillOverridePriority.Upgrade);
                }
            }

            for (int i = 0; i < this.secondarySkillOverrides.Length; i++)
            {
                if (this.secondarySkillOverrides[i])
                {
                    this.skillLocator.secondary.UnsetSkillOverride(this.skillLocator.secondary, this.secondarySkillOverrides[i], GenericSkill.SkillOverridePriority.Upgrade);
                }
            }
            // fuck this, seriously

            // set new overrides
            if (this.weaponDef.primarySkillDef) this.skillLocator.primary.SetSkillOverride(this.skillLocator.primary, this.weaponDef.primarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);
            if (this.weaponDef.secondarySkillDef) this.skillLocator.secondary.SetSkillOverride(this.skillLocator.secondary, this.weaponDef.secondarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);

            // model swap
            this.weaponRenderer.sharedMesh = this.weaponDef.mesh;
            this.characterModel.baseRendererInfos[this.characterModel.baseRendererInfos.Length - 1].defaultMaterial = this.weaponDef.material;

            // timer
            float duration = 8f;

            if (Modules.Config.backupMagExtendDuration.Value)
            {
                if (this.characterBody && this.characterBody.inventory)
                {
                    duration += (0.5f * this.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine));
                }
            }

            if (this.weaponDef.tier == DriverWeaponTier.Common) duration = 0f;
            this.maxWeaponTimer = duration;//this.weaponDef.baseDuration;
            this.weaponTimer = duration;//this.weaponDef.baseDuration;

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
            }
        }

        private void EnableLayer(string layerName)
        {
            if (!this.animator) return;

            this.animator.SetLayerWeight(this.animator.GetLayerIndex("Body, Shotgun"), 0f);

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

        private void OnDestroy()
        {
            if (this.shellObjects != null && this.shellObjects.Length > 0)
            {
                for (int i = 0; i < this.shellObjects.Length; i++)
                {
                    if (this.shellObjects[i]) Destroy(this.shellObjects[i]);
                }
            }

            if (this.characterBody && this.characterBody.master && this.characterBody.master.inventory)
            {
                this.characterBody.master.inventory.onItemAddedClient -= this.Inventory_onItemAddedClient;
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