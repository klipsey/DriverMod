using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RobDriver.Modules.Weapons
{
    public class CrabGun : BaseWeapon<CrabGun>
    {
        public override string weaponNameToken => "VOID_RIFLE";
        public override string weaponName => "Nullifier";
        public override string weaponDesc => "Erase everything in sight.";
        public override string iconName => "texCrabGunWeaponIcon";
        public override GameObject crosshairPrefab => Modules.Assets.circleCrosshairPrefab;
        public override DriverWeaponTier tier => DriverWeaponTier.Void;
        public override int shotCount => 120;
        public override Mesh mesh => Modules.Assets.LoadMesh("meshCrabGun");
        public override Material material => Addressables.LoadAssetAsync<Material>("RoR2/DLC1/VoidMegaCrab/matVoidMegaCrab.mat").WaitForCompletion();
        public override DriverWeaponDef.AnimationSet animationSet => DriverWeaponDef.AnimationSet.TwoHanded;
        public override DriverWeaponDef.BuffType buffType => DriverWeaponDef.BuffType.AttackSpeed;
        public override string calloutSoundString => "sfx_driver_callout_generic";
        public override string configIdentifier => "Nullifier";
        public override float dropChance => 25f;
        public override bool addToPool => false;
        public override string uniqueDropBodyName => "VoidMegaCrab";

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