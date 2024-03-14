using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.Modules.Weapons
{
    public class LunarGrenade : BaseWeapon<LunarGrenade>
    {
        public override string weaponNameToken => "LUNAR_GRENADE";
        public override string weaponName => "Lunar Launcher";
        public override string weaponDesc => "Fire orbs of lunar energy.";
        public override string iconName => "texLunarGrenadeWeaponIcon";
        public override GameObject crosshairPrefab => Modules.Assets.grenadeLauncherCrosshairPrefab;
        public override DriverWeaponTier tier => DriverWeaponTier.Lunar;
        public override int shotCount => 40;
        public override Mesh mesh => Modules.Assets.LoadMesh("meshLunarGrenade");
        public override Material material => Addressables.LoadAssetAsync<Material>("RoR2/Base/LunarGolem/matLunarGolem.mat").WaitForCompletion();
        public override DriverWeaponDef.AnimationSet animationSet => DriverWeaponDef.AnimationSet.TwoHanded;
        public override DriverWeaponDef.BuffType buffType => DriverWeaponDef.BuffType.Damage;
        public override string calloutSoundString => "sfx_driver_callout_generic";
        public override string configIdentifier => "Lunar Launcher";
        public override float dropChance => 5f;
        public override bool addToPool => false;
        public override string uniqueDropBodyName => "LunarExploder";

        public override SkillDef primarySkillDef => Modules.Skills.CreatePrimarySkillDef(
new EntityStates.SerializableEntityStateType(typeof(SkillStates.Driver.LunarGrenade.Shoot)),
"Weapon",
"ROB_DRIVER_BODY_PRIMARY_GRENADELAUNCHER_NAME",
"ROB_DRIVER_BODY_PRIMARY_GRENADELAUNCHER_DESCRIPTION",
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