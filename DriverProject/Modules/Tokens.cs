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
            desc = desc + "< ! > Use Focus to slightly increase your fire rate." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Be careful not to get flanked while using Focus." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Combat Slide while shooting to make sure your damage has no downtime." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Flashbang can be used to make a clean getaway in a pinch." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, in search of a new purpose.";
            string outroFailure = "..and so he vanished, never to become a real human being.";

            string lore = "Back against the wall and odds\n";
            lore += "With the strength of a will and a cause\n";
            lore += "Your pursuits are called outstanding\n";
            lore += "You’re emotionally complex\n\n";
            lore += "Against the grain of dystopic claims\n";
            lore += "Not the thoughts your actions entertain\n";
            lore += "And you have proved to be\n\n\n";
            lore += "A real human being and a real hero";

            LanguageAPI.Add(prefix + "NAME", "Driver");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Enigmatic Man");
            LanguageAPI.Add(prefix + "LORE", lore);
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MONSOON_SKIN_NAME", "Jacket");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Survivalist");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"Enemies killed by the Driver have a chance to drop a new <style=cIsUtility>weapon</style>. These give you <style=cIsDamage>powerful attacks</style> for a limited time!");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_PISTOL_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_PISTOL_DESCRIPTION", $"Fire your pistol for <style=cIsDamage>{100f * SkillStates.Driver.Shoot.damageCoefficient}% damage</style>.\n<style=cIsDamage>Critical hits shoot twice.</style>");

            LanguageAPI.Add(prefix + "PRIMARY_SHOTGUN_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_SHOTGUN_DESCRIPTION", $"Fire a short-range blast for <style=cIsDamage>{SkillStates.Driver.Shotgun.Shoot.bulletCount}x{100f * SkillStates.Driver.Shotgun.Shoot.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_MACHINEGUN_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_MACHINEGUN_DESCRIPTION", $"Fire a rapid spray of shots for <style=cIsDamage>{100f * SkillStates.Driver.MachineGun.Shoot.damageCoefficient}% damage</style>.");

            //LanguageAPI.Add(prefix + "PRIMARY_PUNCH_NAME", "Brick Break");
            //LanguageAPI.Add(prefix + "PRIMARY_PUNCH_DESCRIPTION", $"Punch for <style=cIsDamage>{SkillStates.Regigigas.PunchCombo.damageCoefficientOverride * 100f}% damage</style>.");

            //LanguageAPI.Add(prefix + "PRIMARY_DRAINPUNCH_NAME", "Drain Punch");
            //LanguageAPI.Add(prefix + "PRIMARY_DRAINPUNCH_DESCRIPTION", $"Punch for <style=cIsDamage>{SkillStates.Regigigas.DrainPunch.damageCoefficientOverride * 100f}% damage</style>, <style=cIsHealing>healing for 50% of damage dealt</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_PISTOL_NAME", "Focus");
            LanguageAPI.Add(prefix + "SECONDARY_PISTOL_DESCRIPTION", $"Take aim and charge a shot for up to <style=cIsDamage>{100f * SkillStates.Driver.SteadyAim.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_SHOTGUN_NAME", "Bash");
            LanguageAPI.Add(prefix + "SECONDARY_SHOTGUN_DESCRIPTION", $"<style=cIsDamage>Stun</style> and <style=cIsUtility>knock back</style> nearby enemies for <style=cIsDamage>{100f * SkillStates.Driver.Shotgun.Bash.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_MACHINEGUN_NAME", "Zap");
            LanguageAPI.Add(prefix + "SECONDARY_MACHINEGUN_DESCRIPTION", $"<style=cIsDamage>Shocking.</style> Fire a quick laser for <style=cIsDamage>{100f * SkillStates.Driver.MachineGun.Zap.damageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_SLIDE_NAME", "Combat Slide");
            LanguageAPI.Add(prefix + "UTILITY_SLIDE_DESCRIPTION", "<style=cIsUtility>Slide</style> on the ground for a short distance. You can <style=cIsDamage>fire while sliding.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_NAME", "Flashbang");
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_DESCRIPTION", $"Throw a grenade that <style=cIsUtility>stuns</style> enemies for <style=cIsDamage>{100f * SkillStates.Driver.ThrowGrenade.damageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "UNLOCKABLE_UNLOCKABLE_NAME", "A Real Hero");
            LanguageAPI.Add(prefix + "UNLOCKABLE_ACHIEVEMENT_NAME", "A Real Hero");
            LanguageAPI.Add(prefix + "UNLOCKABLE_ACHIEVEMENT_DESC", "Reach stage 3 in less than 15 minutes.");

            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_UNLOCKABLE_NAME", "Driver: Mastery");
            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_NAME", "Driver: Mastery");
            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_DESC", "As Driver, beat the game or obliterate on Monsoon.");
            #endregion


            LanguageAPI.Add("ROB_DRIVER_JAMMED_POPUP", "JAMMED...");


            #region Gun shit
            LanguageAPI.Add("ROB_DRIVER_PISTOL_NAME", "Pistol");
            LanguageAPI.Add("ROB_DRIVER_PISTOL_DESC", "A reliable handgun that excels at nothing.");

            LanguageAPI.Add("ROB_DRIVER_SHOTGUN_NAME", "Shotgun");
            LanguageAPI.Add("ROB_DRIVER_SHOTGUN_DESC", "A powerhouse that excels at crowd control and heavy damage.");

            LanguageAPI.Add("ROB_DRIVER_MACHINEGUN_NAME", "Machine Gun");
            LanguageAPI.Add("ROB_DRIVER_MACHINEGUN_DESC", "A gun that excels at something, I'm not really sure what.");

            LanguageAPI.Add("ROB_DRIVER_ROCKETLAUNCHER_NAME", "Rocket Launcher");
            LanguageAPI.Add("ROB_DRIVER_ROCKETLAUNCHER_DESC", "");
            #endregion
        }
    }
}