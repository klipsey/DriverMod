using R2API;
using System;

namespace RobDriver.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            string prefix = DriverPlugin.developerPrefix + "_DRIVER_BODY_";

            string desc = "The Driver is literally me.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > ." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > ." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > ." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > ." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so it left, leaving irreparable damage in its wake.";
            string outroFailure = "..and so it vanished, returning to its eternal slumber.";

            string lore = "A construction Chimera designed to destroy the old Chimera Lab to make way for a new building. Seems to have gotten lost in transit at some point and went berserk as a result.";

            LanguageAPI.Add(prefix + "NAME", "Driver");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Literally Me");
            LanguageAPI.Add(prefix + "LORE", lore);
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MONSOON_SKIN_NAME", "Jacket");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Slow Start");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"<style=cIsHealth>Stats are halved</style> upon spawning. Defeating <style=cIsUtility>10 enemies</style> will restore Regigigas to <style=cIsDamage>full strength</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_PISTOL_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_PISTOL_DESCRIPTION", $"Fire your pistol for <style=cIsDamage>{100f * SkillStates.Driver.Shoot.damageCoefficient}% damage</style>.\n<style=cIsDamage>Critical hits shoot twice.</style>");

            //LanguageAPI.Add(prefix + "PRIMARY_PUNCH_NAME", "Brick Break");
            //LanguageAPI.Add(prefix + "PRIMARY_PUNCH_DESCRIPTION", $"Punch for <style=cIsDamage>{SkillStates.Regigigas.PunchCombo.damageCoefficientOverride * 100f}% damage</style>.");

            //LanguageAPI.Add(prefix + "PRIMARY_DRAINPUNCH_NAME", "Drain Punch");
            //LanguageAPI.Add(prefix + "PRIMARY_DRAINPUNCH_DESCRIPTION", $"Punch for <style=cIsDamage>{SkillStates.Regigigas.DrainPunch.damageCoefficientOverride * 100f}% damage</style>, <style=cIsHealing>healing for 50% of damage dealt</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_PISTOL_NAME", "Focus");
            LanguageAPI.Add(prefix + "SECONDARY_PISTOL_DESCRIPTION", $"Take aim and charge a shot for up to <style=cIsDamage>{100f * SkillStates.Driver.SteadyAim.damageCoefficient}% damage</style>.");

            //LanguageAPI.Add(prefix + "SECONDARY_ANCIENTPOWER_NAME", "Ancient Power");
            //LanguageAPI.Add(prefix + "SECONDARY_ANCIENTPOWER_DESCRIPTION", $"Charge up a barrage of rocks for <style=cIsDamage>{100f * SkillStates.Regigigas.FireAncientPower.damageCoefficient}% damage</style> each. Costs <style=cIsHealth>10% max health</style> for each rock if out of stock.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_SLIDE_NAME", "Combat Slide");
            LanguageAPI.Add(prefix + "UTILITY_SLIDE_DESCRIPTION", "<style=cIsUtility>Slide</style> on the ground for a short distance. You can <style=cIsDamage>fire while sliding.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_NAME", "Stun Grenade");
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_DESCRIPTION", $"Throw a grenade that <style=cIsUtility>concusses</style> enemies for <style=cIsDamage>{100f * SkillStates.Driver.ThrowGrenade.damageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_UNLOCKABLE_NAME", "Driver: Mastery");
            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_NAME", "Driver: Mastery");
            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_DESC", "As Driver, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}