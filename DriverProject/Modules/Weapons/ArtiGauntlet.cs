using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.Modules.Weapons
{
    public class ArtiGauntlet : BaseWeapon<ArtiGauntlet>
    {
        public override string weaponNameToken => "ARTI_GAUNTLET";
        public override string weaponName => "Nano-Gauntlet";
        public override string weaponDesc => "Shoot high damage fireballs that inflict burn.";
        public override string iconName => "texArtiGauntletWeaponIcon";
        public override GameObject crosshairPrefab => Modules.Assets.needlerCrosshairPrefab;
        public override DriverWeaponTier tier => DriverWeaponTier.Unique;
        public override int shotCount => 20;
        public override Mesh mesh => Modules.Assets.LoadMesh("meshArtiGauntlet");
        public override Material material => Addressables.LoadAssetAsync<Material>("RoR2/Base/Mage/matMage.mat").WaitForCompletion();
        public override DriverWeaponDef.AnimationSet animationSet => DriverWeaponDef.AnimationSet.Default;
        public override DriverWeaponDef.BuffType buffType => DriverWeaponDef.BuffType.Damage;
        public override string calloutSoundString => "sfx_driver_callout_generic";
        public override string configIdentifier => "NanoGauntlet";
        public override float dropChance => 100f;
        public override bool addToPool => false;
        public override string uniqueDropBodyName => "Mage";

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