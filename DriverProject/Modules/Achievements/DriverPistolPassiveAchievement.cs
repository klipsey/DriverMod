using R2API;
using R2API.Utils;
using RoR2;
using System;
using UnityEngine;

namespace RobDriver.Modules.Achievements
{
    internal class DriverPistolPassiveAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_PISTOLPASSIVE_UNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_PISTOLPASSIVE_UNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_PISTOLPASSIVE_UNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_UNLOCKABLE_REWARD_ID";
        public override string UnlockableNameToken { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_PISTOLPASSIVE_UNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_PISTOLPASSIVE_UNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texAltPassiveIcon");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_PISTOLPASSIVE_UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_PISTOLPASSIVE_UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_PISTOLPASSIVE_UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_PISTOLPASSIVE_UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public static bool weaponPickedUp;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RobDriverBody");
        }

        public override void OnInstall()
        {
            base.OnInstall();

            TeleporterInteraction.onTeleporterBeginChargingGlobal += TeleporterInteraction_onTeleporterBeginChargingGlobal;
            TeleporterInteraction.onTeleporterFinishGlobal += TeleporterInteraction_onTeleporterFinishGlobal;
        }

        private void TeleporterInteraction_onTeleporterFinishGlobal(TeleporterInteraction obj)
        {
            if (base.meetsBodyRequirement && !weaponPickedUp)
            {
                base.Grant();
            }
        }

        private void TeleporterInteraction_onTeleporterBeginChargingGlobal(TeleporterInteraction obj)
        {
            weaponPickedUp = false;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            TeleporterInteraction.onTeleporterBeginChargingGlobal -= TeleporterInteraction_onTeleporterBeginChargingGlobal;
            TeleporterInteraction.onTeleporterFinishGlobal -= TeleporterInteraction_onTeleporterFinishGlobal;
        }
    }
}