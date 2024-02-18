using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.Modules.Weapons
{
    public class ArmBFG : BaseWeapon<ArmBFG>
    {
        public override string weaponNameToken => "BFG";
        public override string weaponName => "Preon Accelerator";
        public override string weaponDesc => "Devastating blasts of particles.";
        public override string iconName => "texPreonWeaponIcon";
        public override GameObject crosshairPrefab => Modules.Assets.grenadeLauncherCrosshairPrefab;
        public override DriverWeaponTier tier => DriverWeaponTier.Legendary;
        public override int shotCount => 12;
        public override Mesh mesh => Modules.Assets.LoadMesh("meshArmBFG");
        public override Material material => Addressables.LoadAssetAsync<Material>("RoR2/Base/BFG/matBFG.mat").WaitForCompletion();
        public override DriverWeaponDef.AnimationSet animationSet => DriverWeaponDef.AnimationSet.Default;
        public override string calloutSoundString => "sfx_driver_callout_generic";
        public override string configIdentifier => "Preon Accelerator";
        public override float dropChance => 0f;
        public override bool addToPool => true;
        public override string uniqueDropBodyName => "";

        public override SkillDef primarySkillDef => Modules.Skills.CreatePrimarySkillDef(
new EntityStates.SerializableEntityStateType(typeof(SkillStates.Driver.ArmBFG.Shoot)),
"Weapon",
"ROB_DRIVER_BODY_PRIMARY_BFG_NAME",
"ROB_DRIVER_BODY_PRIMARY_BFG_DESCRIPTION",
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