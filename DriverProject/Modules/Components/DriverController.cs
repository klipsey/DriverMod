using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

// this is definitely the worst way to do this
// please make a system for this eventually
// don't just stack on a thousand entries in this fucking enum
public enum DriverWeapon
{
    Default,
    Shotgun,
    MachineGun,
    RocketLauncher
}

namespace RobDriver.Modules.Components
{
    public class DriverController : MonoBehaviour
    {
        public ushort syncedWeapon;
        public NetworkInstanceId netId;

        public DriverWeapon weapon;

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

            this.PickUpWeapon(DriverWeapon.Default);
        }

        private void Start()
        {
            this.InitShells();
        }

        public void StartTimer()
        {
            this.timerStarted = true;
        }

        private void FixedUpdate()
        {
            if (this.timerStarted) this.weaponTimer -= Time.fixedDeltaTime;
            this.jamTimer = Mathf.Clamp(this.jamTimer - (2f * Time.fixedDeltaTime), 0f, Mathf.Infinity);

            // test
            /*if (Input.GetKeyDown("z"))
            {
                this.PickUpWeapon(DriverWeapon.Default);
            }

            if (Input.GetKeyDown("x"))
            {
                this.PickUpWeapon(DriverWeapon.Shotgun);
            }

            if (Input.GetKeyDown("c"))
            {
                this.PickUpWeapon(DriverWeapon.MachineGun);
            }

            if (Input.GetKeyDown("v"))
            {
                this.PickUpWeapon(DriverWeapon.RocketLauncher);
            }*/

            if (this.weaponTimer <= 0f && this.weapon != DriverWeapon.Default)
            {
                this.PickUpWeapon(DriverWeapon.Default);
            }
        }

        public void ServerPickUpWeapon(DriverWeapon newWeapon, DriverController driverController)
        {
            ushort augh = 0;
            switch (newWeapon)
            {
                case DriverWeapon.Default:
                    augh = 0;
                    break;
                case DriverWeapon.Shotgun:
                    augh = 1;
                    break;
                case DriverWeapon.MachineGun:
                    augh = 2;
                    break;
            }

            NetworkIdentity identity = driverController.gameObject.GetComponent<NetworkIdentity>();
            if (!identity) return;

            new SyncWeapon(identity.netId, augh).Send(NetworkDestination.Clients);
        }

        public void PickUpWeapon(DriverWeapon newWeapon)
        {
            this.timerStarted = false;
            this.weapon = newWeapon;
            this.EquipWeapon();

            if (this.onWeaponUpdate == null) return;
            this.onWeaponUpdate(this);
        }

        private void EquipWeapon()
        {
            // overrides....
            this.skillLocator.primary.UnsetSkillOverride(this.skillLocator.primary, Modules.Survivors.Driver.shotgunPrimarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);
            this.skillLocator.secondary.UnsetSkillOverride(this.skillLocator.secondary, Modules.Survivors.Driver.shotgunSecondarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);

            this.skillLocator.primary.UnsetSkillOverride(this.skillLocator.primary, Modules.Survivors.Driver.machineGunPrimarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);
            this.skillLocator.secondary.UnsetSkillOverride(this.skillLocator.secondary, Modules.Survivors.Driver.machineGunSecondarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);

            this.skillLocator.primary.UnsetSkillOverride(this.skillLocator.primary, Modules.Survivors.Driver.rocketLauncherPrimarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);
            this.skillLocator.secondary.UnsetSkillOverride(this.skillLocator.secondary, Modules.Survivors.Driver.rocketLauncherSecondarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);
            // fuck this

            switch (this.weapon)
            {
                case DriverWeapon.Default:
                    this.characterModel.baseRendererInfos[1].defaultMaterial = Modules.Assets.pistolMat;
                    this.characterModel.baseRendererInfos[1].renderer.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = Modules.Assets.pistolMesh;
                    this.EnableLayer("");

                    this.characterBody._defaultCrosshairPrefab = Modules.Assets.LoadCrosshair("Standard");

                    this.maxWeaponTimer = 0f;
                    this.weaponTimer = 0f;
                    break;
                case DriverWeapon.Shotgun:
                    this.characterModel.baseRendererInfos[1].defaultMaterial = Modules.Assets.shotgunMat;
                    this.characterModel.baseRendererInfos[1].renderer.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = Modules.Assets.shotgunMesh;
                    this.EnableLayer("Body, Shotgun");

                    this.characterBody._defaultCrosshairPrefab = Modules.Assets.LoadCrosshair("SMG");

                    this.skillLocator.primary.SetSkillOverride(this.skillLocator.primary, Modules.Survivors.Driver.shotgunPrimarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                    this.skillLocator.secondary.SetSkillOverride(this.skillLocator.secondary, Modules.Survivors.Driver.shotgunSecondarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);

                    this.maxWeaponTimer = Modules.Config.shotgunDuration.Value;
                    this.weaponTimer = Modules.Config.shotgunDuration.Value;
                    break;
                case DriverWeapon.MachineGun:
                    this.characterModel.baseRendererInfos[1].defaultMaterial = Modules.Assets.machineGunMat;
                    this.characterModel.baseRendererInfos[1].renderer.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = Modules.Assets.machineGunMesh;
                    this.EnableLayer("Body, Shotgun");

                    this.characterBody._defaultCrosshairPrefab = Modules.Assets.LoadCrosshair("Standard");

                    this.skillLocator.primary.SetSkillOverride(this.skillLocator.primary, Modules.Survivors.Driver.machineGunPrimarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                    this.skillLocator.secondary.SetSkillOverride(this.skillLocator.secondary, Modules.Survivors.Driver.machineGunSecondarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);

                    this.maxWeaponTimer = Modules.Config.shotgunDuration.Value;
                    this.weaponTimer = Modules.Config.shotgunDuration.Value;
                    break;
                case DriverWeapon.RocketLauncher:
                    this.characterModel.baseRendererInfos[1].defaultMaterial = Modules.Assets.rocketLauncherMat;
                    this.characterModel.baseRendererInfos[1].renderer.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = Modules.Assets.rocketLauncherMesh;
                    this.EnableLayer("Body, Shotgun");

                    this.characterBody._defaultCrosshairPrefab = Modules.Assets.LoadCrosshair("Standard");

                    this.skillLocator.primary.SetSkillOverride(this.skillLocator.primary, Modules.Survivors.Driver.rocketLauncherPrimarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                    this.skillLocator.secondary.SetSkillOverride(this.skillLocator.secondary, Modules.Survivors.Driver.rocketLauncherSecondarySkillDef, GenericSkill.SkillOverridePriority.Upgrade);

                    this.maxWeaponTimer = Modules.Config.shotgunDuration.Value;
                    this.weaponTimer = Modules.Config.shotgunDuration.Value;
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