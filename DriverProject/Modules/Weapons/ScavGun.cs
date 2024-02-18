using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.Modules.Weapons
{
    public class ScavGun : BaseWeapon<ScavGun>
    {
        public override string weaponNameToken => "SCAV_GUN";
        public override string weaponName => "Energy Cannon";
        public override string weaponDesc => "";
        public override string iconName => "texScavGunWeaponIcon";
        public override GameObject crosshairPrefab => Modules.Assets.grenadeLauncherCrosshairPrefab;
        public override DriverWeaponTier tier => DriverWeaponTier.Unique;
        public override int shotCount => 12;
        public override Mesh mesh => Modules.Assets.LoadMesh("meshScavGun");
        public override Material material => Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/TrimSheets/matTrimSheetConstructionBlueEmission.mat").WaitForCompletion();
        public override DriverWeaponDef.AnimationSet animationSet => DriverWeaponDef.AnimationSet.TwoHanded;
        public override string calloutSoundString => "sfx_driver_callout_generic";
        public override string configIdentifier => "Energy Cannon";
        public override float dropChance => 25f;
        public override bool addToPool => false;
        public override string uniqueDropBodyName => "Scav";

        public override SkillDef primarySkillDef => Modules.Skills.CreatePrimarySkillDef(
new EntityStates.SerializableEntityStateType(typeof(SkillStates.Driver.VoidRifle.Shoot)),
"Weapon",
"ROB_DRIVER_BODY_PRIMARY_VOIDRIFLE_NAME",
"ROB_DRIVER_BODY_PRIMARY_VOIDRIFLE_DESCRIPTION",
Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texRocketLauncherIcon"),
false);

        public override SkillDef secondarySkillDef => Modules.Skills.CreateSkillDef(new SkillDefInfo
        {
            skillName = "ROB_DRIVER_BODY_SECONDARY_SHOTGUN_NAME",
            skillNameToken = "ROB_DRIVER_BODY_SECONDARY_SHOTGUN_NAME",
            skillDescriptionToken = "ROB_DRIVER_BODY_SECONDARY_SHOTGUN_DESCRIPTION",
            skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texShotgunSecondaryIcon"),
            activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Driver.Shotgun.Bash)),
            activationStateMachineName = "Weapon",
            baseMaxStock = 1,
            baseRechargeInterval = 6f,
            beginSkillCooldownOnSkillEnd = false,
            canceledFromSprinting = false,
            forceSprintDuringState = false,
            fullRestockOnAssign = true,
            interruptPriority = EntityStates.InterruptPriority.Skill,
            resetCooldownTimerOnUse = true,
            isCombatSkill = true,
            mustKeyPress = false,
            cancelSprintingOnActivation = true,
            rechargeStock = 1,
            requiredStock = 1,
            stockToConsume = 1,
        });

        public override void Init()
        {
            CreateLang();
            CreateWeapon();
        }
    }
}