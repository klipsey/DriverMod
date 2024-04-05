using R2API;
using R2API.Utils;
using RoR2;
using System;
using UnityEngine;

namespace RobDriver.Modules.Achievements
{
    internal class DriverGodslingPassiveAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_GODSLING_UNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_GODSLING_UNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_GODSLING_UNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_UNLOCKABLE_REWARD_ID";
        public override string UnlockableNameToken { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_GODSLING_UNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = DriverPlugin.developerPrefix + "_DRIVER_BODY_GODSLING_UNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texAltPassiveIcon");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_GODSLING_UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_GODSLING_UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_GODSLING_UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(DriverPlugin.developerPrefix + "_DRIVER_BODY_GODSLING_UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public static bool weaponPickedUpHard;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RobDriverBody");
        }

        public override void OnInstall()
        {
            base.OnInstall();

            Run.onClientGameOverGlobal += this.ClearCheck;
            Run.onRunStartGlobal += this.Reset;
        }

        private void Reset(Run run)
        {
            weaponPickedUpHard = false;
        }

        public void ClearCheck(Run run, RunReport runReport)
        {
            if (run is null) return;
            if (runReport is null) return;

            if (!runReport.gameEnding) return;

            if (runReport.gameEnding.isWin)
            {
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());

                if (difficultyDef != null && difficultyDef.countsAsHardMode)
                {
                    if (base.meetsBodyRequirement && !weaponPickedUpHard)
                    {
                        base.Grant();
                    }
                }
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            Run.onClientGameOverGlobal -= this.ClearCheck;
            Run.onRunStartGlobal -= this.Reset;
        }
    }
}