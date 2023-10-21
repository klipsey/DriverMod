using R2API;
using R2API.Utils;
using RoR2;
using System;
using UnityEngine;

namespace RobDriver.Modules.Achievements
{
    internal class SuitAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_SUIT_UNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_SUIT_UNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_SUIT_UNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_UNLOCKABLE_REWARD_ID";
        public override string UnlockableNameToken { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_SUIT_UNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_SUIT_UNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSuitSkin");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_SUIT_UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_SUIT_UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_SUIT_UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_SUIT_UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RobDriverBody");
        }

        public override void OnInstall()
        {
            base.OnInstall();

            RoR2.GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport)
        {
            if (damageReport.attackerBody && damageReport.attackerMaster && damageReport.victim && damageReport.attackerBody.baseNameToken == Modules.Survivors.Driver.bodyNameToken)
            {
                if (damageReport.victimIsChampion)
                {
                    Modules.Components.DriverController iDrive = damageReport.attackerBody.gameObject.GetComponent<Modules.Components.DriverController>();
                    if (iDrive)
                    {
                        if (iDrive.weaponDef.nameToken == "ROB_DRIVER_SNIPER_NAME")
                        {
                            if (base.meetsBodyRequirement)
                            {
                                base.Grant();
                            }
                        }
                    }
                }
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            RoR2.GlobalEventManager.onCharacterDeathGlobal -= GlobalEventManager_onCharacterDeathGlobal;
        }
    }
}