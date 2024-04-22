using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.Modules.Weapons
{
    public class RavSword : BaseWeapon<ArtiGauntlet>
    {
        public override string weaponNameToken => "RAV_SWORD";
        public override string weaponName => "Ravager-Sword";
        public override string weaponDesc => "Slash and jump your way through enemies.";
        public override string iconName => "texRavSomethinSomething";
        public override GameObject crosshairPrefab => Modules.Assets.circleCrosshairPrefab;
        public override DriverWeaponTier tier => DriverWeaponTier.Unique;
        public override int shotCount => -1;
        public override Mesh mesh => Modules.Assets.LoadMesh("meshRavSword");
        public override Material material => Addressables.LoadAssetAsync<Material>("lol").WaitForCompletion();
        public override DriverWeaponDef.AnimationSet animationSet => DriverWeaponDef.AnimationSet.Default;
        public override DriverWeaponDef.BuffType buffType => DriverWeaponDef.BuffType.Damage;
        public override string calloutSoundString => "sfx_driver_callout_generic";
        public override string configIdentifier => "RavagerSword";
        public override float dropChance => 100f;
        public override bool addToPool => false;
        public override string uniqueDropBodyName => "Ravager";

        public override SkillDef primarySkillDef => Modules.Skills.CreatePrimarySkillDef(
        new EntityStates.SerializableEntityStateType(typeof(SkillStates.Driver.ArtiGauntlet.Shoot)),
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