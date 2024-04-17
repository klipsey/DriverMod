using UnityEngine;
using RoR2;
using UnityEngine.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;

namespace RobDriver.Modules.Components
{
    public class WeaponPickup : MonoBehaviour
    {
        [Tooltip("The base object to destroy when this pickup is consumed.")]
        public GameObject baseObject;
        [Tooltip("The team filter object which determines who can pick up this pack.")]
        public TeamFilter teamFilter;

        public DriverWeaponDef weaponDef;
        public DriverBulletDef bulletDef;

        public GameObject pickupEffect;

        public bool cutAmmo = false;
        public bool isNewAmmoType = false;
        private bool alive = true;

        private void Awake()
        {
            // disable visuals for non driver
            if (!Modules.Config.sharedPickupVisuals.Value)
            {
                BeginRapidlyActivatingAndDeactivating blinker = this.transform.parent.GetComponentInChildren<BeginRapidlyActivatingAndDeactivating>();
                if (blinker)
                {
                    bool isDriver = false;

                    var localPlayers = LocalUserManager.readOnlyLocalUsersList;
                    foreach (LocalUser i in localPlayers)
                    {
                        if (i.cachedBody.baseNameToken == Modules.Survivors.Driver.bodyNameToken) isDriver = true;
                    }

                    if (!isDriver)
                    {
                        blinker.blinkingRootObject.SetActive(false);
                        Destroy(blinker);
                    }
                }
            }

            // uh will this work?
            /*if (Run.instance)
			{
				float rng = Run.instance.stageRng.nextNormalizedFloat;

				if (rng > 0.5f) this.SetWeapon(DriverWeapon.MachineGun);
				else this.SetWeapon(DriverWeapon.Shotgun);
			}*/
            // no it doesn't, clients don't have the rng

            // i'm a dirty hack
            // lock me up and throw away the key
            this.Invoke("Fuck", 59.5f);
        }

        private void Start()
        {
            this.SetWeapon(this.weaponDef, this.bulletDef, this.cutAmmo, this.isNewAmmoType);
        }

        private void OnTriggerStay(Collider collider)
        {
            // can this run on every client? i don't know but let's find out
            if (NetworkServer.active && this.alive/* && TeamComponent.GetObjectTeam(collider.gameObject) == this.teamFilter.teamIndex*/)
            {
                // well it can but it's not a fix.
                DriverController iDrive = collider.GetComponent<DriverController>();
                if (iDrive)
                {
                    this.alive = false;

                    Modules.Achievements.DriverPistolPassiveAchievement.weaponPickedUp = true;
                    Modules.Achievements.DriverGodslingPassiveAchievement.weaponPickedUpHard = true;

                    iDrive.ServerPickUpWeapon(iDrive, this.weaponDef, this.bulletDef, this.cutAmmo, this.isNewAmmoType);
                    EffectManager.SimpleEffect(this.pickupEffect, this.transform.position, Quaternion.identity, true);
                    UnityEngine.Object.Destroy(this.baseObject);
                }
            }
        }

        public void SetWeapon(DriverWeaponDef newWeapon, DriverBulletDef newBullet, bool cutAmmo, bool isNewAmmoType)
        {
            this.weaponDef = newWeapon;
            this.bulletDef = newBullet;
            this.cutAmmo = cutAmmo;
            this.isNewAmmoType = isNewAmmoType;

            SwapToAmmoVisuals();
        }

        private void SwapToAmmoVisuals()
        {
            // swap to ammo visuals
            foreach (LocalUser i in LocalUserManager.readOnlyLocalUsersList)
            {
                if (i.cachedBody.hasEffectiveAuthority)
                {
                    var driverController = i.cachedBody.GetComponent<DriverController>();
                    if (i.cachedBody.baseNameToken == Modules.Survivors.Driver.bodyNameToken && driverController)
                    {
                        if (driverController.passive.isPistolOnly || driverController.passive.isBullets || (driverController.passive.isRyan && this.isNewAmmoType))
                        {
                            BeginRapidlyActivatingAndDeactivating blinker = this.transform.parent.GetComponentInChildren<BeginRapidlyActivatingAndDeactivating>();
                            if (blinker)
                            {
                                foreach (MeshRenderer h in blinker.blinkingRootObject.GetComponentsInChildren<MeshRenderer>())
                                {
                                    h.enabled = false;
                                }

                                GameObject ammoPickup = GameObject.Instantiate(Modules.Assets.ammoPickupModel, blinker.blinkingRootObject.transform);
                                ammoPickup.transform.localPosition = Vector3.zero;
                                ammoPickup.transform.localRotation = Quaternion.identity;
                                DoTextStuff(ammoPickup.transform, true);
                            }
                        }
                        else
                        {
                            DoTextStuff(this.transform.parent, false);
                        }
                    }
                    break;
                }
            }
        }

        private void DoTextStuff(Transform transform, bool isAmmo)
        {
            RoR2.UI.LanguageTextMeshController textComponent = transform.GetComponentInChildren<RoR2.UI.LanguageTextMeshController>();
            if (textComponent)
            {
                if (!this.weaponDef)
                {
                    // band-aid i don't have the time to keep fighting with this code rn
                    textComponent.token = "Weapon";
                    return;
                }
                if((isAmmo && !this.bulletDef))
                {
                    textComponent.token = "Bullets";
                    return;
                }

                if (isAmmo) textComponent.token = this.bulletDef.nameToken;
                else textComponent.token = this.weaponDef.nameToken;

                //Because all guns are "uncommon" we have to do this for ammo
                Color commonColor = Modules.Helpers.whiteItemColor;
                Color uncommonColor = Modules.Helpers.greenItemColor;
                if (isAmmo)
                {
                    commonColor = Modules.Helpers.greenItemColor;
                    uncommonColor = Modules.Helpers.whiteItemColor;
                }

                if (this.cutAmmo)
                {
                    textComponent.textMeshPro.color = Modules.Helpers.badColor;
                }
                else
                {
                    switch (this.weaponDef.tier)
                    {
                        case DriverWeaponTier.Common:
                            textComponent.textMeshPro.color = commonColor;
                            break;
                        case DriverWeaponTier.Uncommon:
                            textComponent.textMeshPro.color = uncommonColor;
                            break;
                        case DriverWeaponTier.Legendary:
                            textComponent.textMeshPro.color = Modules.Helpers.redItemColor;
                            break;
                        case DriverWeaponTier.Unique:
                            textComponent.textMeshPro.color = Modules.Helpers.yellowItemColor;
                            break;
                        case DriverWeaponTier.Lunar:
                            textComponent.textMeshPro.color = Modules.Helpers.lunarItemColor;
                            break;
                        case DriverWeaponTier.Void:
                            textComponent.textMeshPro.color = Modules.Helpers.voidItemColor;
                            break;
                    }
                }
            }
        }
        private void Fuck()
        {
            if (this.alive)
            {
                Modules.Achievements.SupplyDropAchievement.weaponHasDespawned = true;
            }
        }
    }
}