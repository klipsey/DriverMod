using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.Modules.Weapons
{
    public class BanditRevolver : BaseWeapon<BanditRevolver>
    {
        public override string weaponNameToken => "BANDITREVOLVER";
        public override string weaponName => "Outlaw's Revolver";
        public override string weaponDesc => "A revolver that's been through hell and back.";
        public override string iconName => "texBanditRevolverWeaponIcon";
        public override GameObject crosshairPrefab => Modules.Assets.revolverCrosshairPrefab;
        public override DriverWeaponTier tier => DriverWeaponTier.Unique;
        public override int shotCount => 6;
        public override Mesh mesh => Modules.Assets.LoadMesh("meshRevolver");
        public override Material material => Addressables.LoadAssetAsync<Material>("RoR2/Base/Bandit2/matBandit2Revolver.mat").WaitForCompletion();
        public override DriverWeaponDef.AnimationSet animationSet => DriverWeaponDef.AnimationSet.Default;
        public override DriverWeaponDef.BuffType buffType => DriverWeaponDef.BuffType.Crit;
        public override string calloutSoundString => "sfx_driver_callout_generic";
        public override string configIdentifier => "Outlaws Revolver";
        public override float dropChance => 100f;
        public override bool addToPool => false;
        public override string uniqueDropBodyName => "Bandit2";

        public override SkillDef primarySkillDef => Modules.Skills.CreatePrimarySkillDef(
            new EntityStates.SerializableEntityStateType(typeof(SkillStates.Driver.Revolver.Shoot)),
            "Weapon",
            "ROB_DRIVER_BODY_PRIMARY_BFG_NAME",
            "ROB_DRIVER_BODY_PRIMARY_BFG_DESCRIPTION",
            Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texRocketLauncherIcon"),
            false);

        public override SkillDef secondarySkillDef => Modules.Skills.CreateSkillDef(new SkillDefInfo
        {
            skillName = "ROB_DRIVER_BODY_SECONDARY_GOLDENGUN_NAME",
            skillNameToken = "ROB_DRIVER_BODY_SECONDARY_GOLDENGUN_NAME",
            skillDescriptionToken = "ROB_DRIVER_BODY_SECONDARY_GOLDENGUN_DESCRIPTION",
            skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texGoldenGunSecondaryIcon"),
            activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Driver.Revolver.AimLightsOutReset)),
            activationStateMachineName = "Weapon",
            baseMaxStock = 1,
            baseRechargeInterval = 6f,
            beginSkillCooldownOnSkillEnd = true,
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