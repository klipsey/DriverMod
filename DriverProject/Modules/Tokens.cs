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
            desc = desc + "< ! > Weapons each last 8 seconds and have unique strengths and weaknesses so be sure to pick the right tool for the job." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Focus greatly increases your damage output, but be careful not to get flanked while aiming." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Combat Slide while shooting to make sure your damage has no downtime." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Flashbang can be used to make a clean getaway in a pinch." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, still the same as he was when he began.";
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
            LanguageAPI.Add(prefix + "SUBTITLE", "Driver? I hardly know 'er!");
            LanguageAPI.Add(prefix + "LORE", lore);
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MONSOON_SKIN_NAME", "Jacket");
            LanguageAPI.Add(prefix + "TYPHOON_SKIN_NAME", "Slugger");
            LanguageAPI.Add(prefix + "SUIT_SKIN_NAME", "Hitman");
            LanguageAPI.Add(prefix + "SUIT2_SKIN_NAME", "Hitman EX");
            LanguageAPI.Add(prefix + "MINECRAFT_SKIN_NAME", "Minecraft");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Survivalist");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"Enemies killed by the Driver have a chance to drop a new <style=cIsUtility>weapon</style>. These give you <style=cIsDamage>powerful attacks</style> for a limited time!");

            LanguageAPI.Add(prefix + "CONFIRM_NAME", "Confirm");
            LanguageAPI.Add(prefix + "CONFIRM_DESCRIPTION", "Proceed with the current skill.");

            LanguageAPI.Add(prefix + "CANCEL_NAME", "Cancel");
            LanguageAPI.Add(prefix + "CANCEL_DESCRIPTION", "Cancel the current skill.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_PISTOL_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_PISTOL_DESCRIPTION", $"Fire your pistol for <style=cIsDamage>{100f * SkillStates.Driver.Shoot.damageCoefficient}% damage</style>.\n<style=cIsDamage>Critical hits shoot twice.</style>");

            LanguageAPI.Add(prefix + "PRIMARY_PYRITE_PISTOL_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_PYRITE_PISTOL_DESCRIPTION", $"Fire your pistol for <style=cIsDamage>{100f * 2.5f}% damage</style>.\n<style=cIsDamage>Critical hits shoot twice.</style>");

            LanguageAPI.Add(prefix + "PRIMARY_LUNAR_PISTOL_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_LUNAR_PISTOL_DESCRIPTION", $"Fire your pistol for <style=cIsDamage>{100f * 3.5f}% damage</style>.\n<style=cIsDamage>Critical hits shoot twice.</style>");

            LanguageAPI.Add(prefix + "PRIMARY_VOID_PISTOL_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_VOID_PISTOL_DESCRIPTION", $"Fire your pistol for <style=cIsDamage>{100f * 3.5f}% damage</style>.\n<style=cIsDamage>Critical hits shoot twice.</style>");

            LanguageAPI.Add(prefix + "PRIMARY_GOLDENGUN_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_GOLDENGUN_DESCRIPTION", $"Fire your pistol for <style=cIsDamage>{100f * 3.9f}% damage</style>.\n<style=cIsDamage>Critical hits shoot twice.</style>");

            LanguageAPI.Add(prefix + "PRIMARY_SHOTGUN_NAME", "Blast");
            LanguageAPI.Add(prefix + "PRIMARY_SHOTGUN_DESCRIPTION", $"Fire a short-range blast for <style=cIsDamage>{SkillStates.Driver.Shotgun.Shoot.bulletCount}x{100f * SkillStates.Driver.Shotgun.Shoot.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_RIOT_SHOTGUN_NAME", "Blast");
            LanguageAPI.Add(prefix + "PRIMARY_RIOT_SHOTGUN_DESCRIPTION", $"Fire a short-range <style=cIsUtilitye>piercing</style> blast for <style=cIsDamage>{SkillStates.Driver.RiotShotgun.Shoot.bulletCount}x{100f * SkillStates.Driver.RiotShotgun.Shoot.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_SLUG_SHOTGUN_NAME", "Blast");
            LanguageAPI.Add(prefix + "PRIMARY_SLUG_SHOTGUN_DESCRIPTION", $"Fire a short-range slug for <style=cIsDamage>{100f * SkillStates.Driver.SlugShotgun.Shoot.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_BADASS_SHOTGUN_NAME", "Blast");
            LanguageAPI.Add(prefix + "PRIMARY_BADASS_SHOTGUN_DESCRIPTION", $"Fire a short-range blast for <style=cIsDamage>{SkillStates.Driver.BadassShotgun.Shoot.bulletCount}x{100f * SkillStates.Driver.BadassShotgun.Shoot.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_MACHINEGUN_NAME", "Spray");
            LanguageAPI.Add(prefix + "PRIMARY_MACHINEGUN_DESCRIPTION", $"Fire a rapid spray of shots for <style=cIsDamage>{100f * SkillStates.Driver.MachineGun.Shoot.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_HEAVY_MACHINEGUN_NAME", "Spray");
            LanguageAPI.Add(prefix + "PRIMARY_HEAVY_MACHINEGUN_DESCRIPTION", $"Fire a spray of <style=cIsUtilitye>armor piercing</style> shots for <style=cIsDamage>{100f * SkillStates.Driver.HeavyMachineGun.Shoot.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_BAZOOKA_NAME", "Fire");
            LanguageAPI.Add(prefix + "PRIMARY_BAZOOKA_DESCRIPTION", $"Charge and fire a rocket for <style=cIsDamage>{100f * SkillStates.Driver.Bazooka.Fire.minDamageCoefficient}-{100f * SkillStates.Driver.Bazooka.Fire.maxDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_ROCKETLAUNCHER_NAME", "Fire");
            LanguageAPI.Add(prefix + "PRIMARY_ROCKETLAUNCHER_DESCRIPTION", $"Fire a rocket for <style=cIsDamage>{100f * SkillStates.Driver.RocketLauncher.Shoot.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_ROCKETLAUNCHER_ALT_NAME", "Fire");
            LanguageAPI.Add(prefix + "PRIMARY_ROCKETLAUNCHER_ALT_DESCRIPTION", $"Fire a rocket for <style=cIsDamage>{100f * 4.5f}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_GRENADELAUNCHER_NAME", "Fire");
            LanguageAPI.Add(prefix + "PRIMARY_GRENADELAUNCHER_DESCRIPTION", $"Launch a grenade for <style=cIsDamage>{100f * 5f}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_PLASMACANNON_NAME", "Fire");
            LanguageAPI.Add(prefix + "PRIMARY_PLASMACANNON_DESCRIPTION", $"Fire a burst of plasma for <style=cIsDamage>{100f * 14f}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_SNIPER_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_SNIPER_DESCRIPTION", $"Fire your rifle for <style=cIsDamage>{100f * SkillStates.Driver.SniperRifle.Shoot.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_LUNARRIFLE_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_LUNARRIFLE_DESCRIPTION", $"Fire a blast for <style=cIsDamage>{100f * SkillStates.Driver.LunarRifle.Shoot.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_LUNARHAMMER_NAME", "Crush");
            LanguageAPI.Add(prefix + "PRIMARY_LUNARHAMMER_DESCRIPTION", $"Swing your hammer for <style=cIsDamage>{100f * SkillStates.Driver.LunarHammer.SwingCombo._damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_NEMMANDO_NAME", "Shoot");
            LanguageAPI.Add(prefix + "PRIMARY_NEMMANDO_DESCRIPTION", $"Fire your pistol for <style=cIsDamage>{100f * 3.8f}% damage</style>.\n<style=cIsDamage>Critical hits shoot twice.</style>");

            LanguageAPI.Add(prefix + "PRIMARY_NEMMERC_NAME", "Splatter");
            LanguageAPI.Add(prefix + "PRIMARY_NEMMERC_DESCRIPTION", $"Fire a short-range blast for <style=cIsDamage>{SkillStates.Driver.Shotgun.Shoot.bulletCount}x{100f * SkillStates.Driver.Shotgun.Shoot.damageCoefficient}% damage</style>, and again when released.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_PISTOL_NAME", "Focus");
            LanguageAPI.Add(prefix + "SECONDARY_PISTOL_DESCRIPTION", $"Take aim and charge a shot for up to <style=cIsDamage>{100f * SkillStates.Driver.SteadyAim.damageCoefficient}% damage</style>. <style=cIsUtility>Boosts rate of fire and accuracy.</style>");

            LanguageAPI.Add(prefix + "SECONDARY_PYRITE_PISTOL_NAME", "Focus");
            LanguageAPI.Add(prefix + "SECONDARY_PYRITE_PISTOL_DESCRIPTION", $"Take aim and charge a shot for up to <style=cIsDamage>{100f * 6f}% damage</style>. <style=cIsUtility>Boosts rate of fire and accuracy.</style>");

            LanguageAPI.Add(prefix + "SECONDARY_LUNAR_PISTOL_NAME", "Focus");
            LanguageAPI.Add(prefix + "SECONDARY_LUNAR_PISTOL_DESCRIPTION", $"Take aim and charge a shot for up to <style=cIsDamage>{100f * 9f}% damage</style>. <style=cIsUtility>Boosts rate of fire and accuracy.</style>");

            LanguageAPI.Add(prefix + "SECONDARY_VOID_PISTOL_NAME", "Focus");
            LanguageAPI.Add(prefix + "SECONDARY_VOID_PISTOL_DESCRIPTION", $"Take aim and charge a shot for up to <style=cIsDamage>{100f * 9f}% damage</style>. <style=cIsUtility>Boosts rate of fire and accuracy.</style>");

            LanguageAPI.Add(prefix + "SECONDARY_BEETLESHIELD_NAME", "Block");
            LanguageAPI.Add(prefix + "SECONDARY_BEETLESHIELD_DESCRIPTION", $"Take aim and charge a shot for up to <style=cIsDamage>{100f * SkillStates.Driver.SteadyAim.damageCoefficient}% damage</style>. <style=cIsUtility>Blocks all damage from in front.</style>");

            LanguageAPI.Add(prefix + "SECONDARY_SHOTGUN_NAME", "Bash");
            LanguageAPI.Add(prefix + "SECONDARY_SHOTGUN_DESCRIPTION", $"<style=cIsDamage>Stun</style> and <style=cIsUtility>knock back</style> nearby enemies for <style=cIsDamage>{100f * SkillStates.Driver.Shotgun.Bash.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_SLUG_SHOTGUN_NAME", "Knife");
            LanguageAPI.Add(prefix + "SECONDARY_SLUG_SHOTGUN_DESCRIPTION", $"Throw a knife that gets stuck in the first enemy hit for <style=cIsDamage>{100f * SkillStates.Driver.Shotgun.Bash.damageCoefficient}% damage</style>. Shoot this knife to deal an additional <style=cIsDamage>{100f * SkillStates.Driver.Shotgun.Bash.damageCoefficient}% damage</style> and inflict <style=cIsHealth>Bleed</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_MACHINEGUN_NAME", "Zap");
            LanguageAPI.Add(prefix + "SECONDARY_MACHINEGUN_DESCRIPTION", $"<style=cIsDamage>Shocking.</style> Fire a quick laser for <style=cIsDamage>{100f * SkillStates.Driver.MachineGun.Zap.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_HEAVY_MACHINEGUN_NAME", "Grenade");
            LanguageAPI.Add(prefix + "SECONDARY_HEAVY_MACHINEGUN_DESCRIPTION", $"Launch a grenade that <style=cIsUtility>stuns</style> enemies for <style=cIsDamage>{100f * SkillStates.Driver.HeavyMachineGun.ShootGrenade.damageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_ROCKETLAUNCHER_NAME", "Hailstorm");
            LanguageAPI.Add(prefix + "SECONDARY_ROCKETLAUNCHER_DESCRIPTION", $"Fire a rapid barrage of rockets for <style=cIsDamage>{100f * SkillStates.Driver.RocketLauncher.Barrage.damageCoefficient}% damage</style> each.");

            LanguageAPI.Add(prefix + "SECONDARY_ROCKETLAUNCHER_ALT_NAME", "Hailstorm");
            LanguageAPI.Add(prefix + "SECONDARY_ROCKETLAUNCHER_ALT_DESCRIPTION", $"Fire a rapid barrage of rockets for <style=cIsDamage>{100f * 2.5f}% damage</style> each.");

            LanguageAPI.Add(prefix + "SECONDARY_PLASMACANNON_NAME", "Annihilation");
            LanguageAPI.Add(prefix + "SECONDARY_PLASMACANNON_DESCRIPTION", $"Fire a rapid barrage of plasma bursts for <style=cIsDamage>{100f * 10f}% damage</style> each.");

            LanguageAPI.Add(prefix + "SECONDARY_SNIPER_NAME", "Focus");
            LanguageAPI.Add(prefix + "SECONDARY_SNIPER_DESCRIPTION", $"Take aim and charge a devastating piercing shot for <style=cIsDamage>{100f * SkillStates.Driver.SniperRifle.SteadyAim.damageCoefficient}% damage</style>. <style=cIsUtility>Exposes enemy weak points.</style>");

            LanguageAPI.Add(prefix + "SECONDARY_GOLDENGUN_NAME", "Lights Out");
            LanguageAPI.Add(prefix + "SECONDARY_GOLDENGUN_DESCRIPTION", $"Take aim and fire a devastating shot for <style=cIsDamage>{100f * SkillStates.Driver.GoldenGun.LightsOut.damageCoefficient}% damage</style>. <style=cIsHealth>Consumes the gun on use.</style>");

            LanguageAPI.Add(prefix + "SECONDARY_LUNARHAMMER_NAME", "Shards");
            LanguageAPI.Add(prefix + "SECONDARY_LUNARHAMMER_DESCRIPTION", $"<style=cIsUtility>Agile.</style> Fire a volley of <style=cIsUtility>lunar shards</style>, dealing <style=cIsDamage>" + 100f * SkillStates.Driver.LunarHammer.FireShard.damageCoefficient + "% damage</style> each.");

            LanguageAPI.Add(prefix + "SECONDARY_NEMMANDO_NAME", "Submission");
            LanguageAPI.Add(prefix + "SECONDARY_NEMMANDO_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> Fire repeatedly for <style=cIsDamage>" + 100f * SkillStates.Driver.LunarHammer.FireShard.damageCoefficient + "% damage</style> per shot.");

            LanguageAPI.Add(prefix + "SECONDARY_NEMMERC_NAME", "Bash");
            LanguageAPI.Add(prefix + "SECONDARY_NEMMERC_DESCRIPTION", $"<style=cIsDamage>Stun</style> and <style=cIsUtility>knock back</style> nearby enemies for <style=cIsDamage>{100f * SkillStates.Driver.Shotgun.Bash.damageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_SLIDE_NAME", "Combat Slide");
            LanguageAPI.Add(prefix + "UTILITY_SLIDE_DESCRIPTION", "<style=cIsUtility>Slide</style> on the ground for a short distance. You can <style=cIsDamage>fire while sliding.</style>");

            LanguageAPI.Add(prefix + "UTILITY_SKATEBOARD_NAME", "Skateboard");
            LanguageAPI.Add(prefix + "UTILITY_SKATEBOARD_DESCRIPTION", "Ride your <style=cIsUtility>skateboard</style>. You can <style=cIsDamage>fire while skating.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_NAME", "Flashbang");
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_DESCRIPTION", $"Throw a grenade that <style=cIsUtility>stuns</style> enemies for <style=cIsDamage>{100f * SkillStates.Driver.ThrowGrenade.damageCoefficient}% damage</style>. <style=cIsUtility>You can hold up to two.</style>");

            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_SCEPTER_NAME", "Molotov");
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_SCEPTER_DESCRIPTION", $"Throw a grenade that <style=cIsUtility>stuns</style> enemies for <style=cIsDamage>{100f * SkillStates.Driver.ThrowGrenade.damageCoefficient}% damage</style>. <style=cIsUtility>You can hold up to two.</style>" + Helpers.ScepterDescription("Throw a molotov that bursts into flames instead."));

            LanguageAPI.Add(prefix + "SPECIAL_SUPPLY_DROP_NAME", "Supply Drop");
            LanguageAPI.Add(prefix + "SPECIAL_SUPPLY_DROP_DESCRIPTION", $"Call down a briefcase containing a <color=#{Helpers.yellowItemHex}>Prototype Rocket Launcher</color>. <style=cIsUtility>You can only request one per stage.</style>");

            LanguageAPI.Add(prefix + "SPECIAL_SUPPLY_DROP_SCEPTER_NAME", "Call of the Void");
            LanguageAPI.Add(prefix + "SPECIAL_SUPPLY_DROP_SCEPTER_DESCRIPTION", $"Call down a briefcase containing a <color=#{Helpers.yellowItemHex}>Prototype Rocket Launcher</color>. <style=cIsUtility>You can only request one per stage.</style>" + Helpers.ScepterDescription("Summon a <color=#" + Helpers.voidItemHex + ">voidborn weapon</color> instead."));
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "UNLOCKABLE_UNLOCKABLE_NAME", "A Real Hero");
            LanguageAPI.Add(prefix + "UNLOCKABLE_ACHIEVEMENT_NAME", "A Real Hero");
            LanguageAPI.Add(prefix + "UNLOCKABLE_ACHIEVEMENT_DESC", "Reach stage 3 in less than 15 minutes.");

            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_UNLOCKABLE_NAME", "Driver: Mastery");
            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_NAME", "Driver: Mastery");
            LanguageAPI.Add(prefix + "MONSOONUNLOCKABLE_ACHIEVEMENT_DESC", "As Driver, beat the game or obliterate on Monsoon.");

            LanguageAPI.Add(prefix + "TYPHOON_UNLOCKABLE_UNLOCKABLE_NAME", "Driver: Grand Mastery");
            LanguageAPI.Add(prefix + "TYPHOON_UNLOCKABLE_ACHIEVEMENT_NAME", "Driver: Grand Mastery");
            LanguageAPI.Add(prefix + "TYPHOON_UNLOCKABLE_ACHIEVEMENT_DESC", "As Driver, beat the game or obliterate on Typhoon or Eclipse.\n<color=#8888>(Counts any difficulty Typhoon or higher)</color>");

            LanguageAPI.Add(prefix + "SUPPLY_DROP_UNLOCKABLE_UNLOCKABLE_NAME", "Driver: Locked and Loaded");
            LanguageAPI.Add(prefix + "SUPPLY_DROP_UNLOCKABLE_ACHIEVEMENT_NAME", "Driver: Locked and Loaded");
            LanguageAPI.Add(prefix + "SUPPLY_DROP_UNLOCKABLE_ACHIEVEMENT_DESC", "As Driver, complete a teleporter event without letting any briefcases despawn.");

            LanguageAPI.Add(prefix + "SUIT_UNLOCKABLE_UNLOCKABLE_NAME", "Driver: Dressed to Kill");
            LanguageAPI.Add(prefix + "SUIT_UNLOCKABLE_ACHIEVEMENT_NAME", "Driver: Dressed to Kill");
            LanguageAPI.Add(prefix + "SUIT_UNLOCKABLE_ACHIEVEMENT_DESC", "As Driver, land the killing blow on a boss with a Sniper Rifle.");
            #endregion


            LanguageAPI.Add("ROB_DRIVER_JAMMED_POPUP", "JAMMED...");

            LanguageAPI.Add("ROB_DRIVER_UPGRADE_POPUP", "UPGRADE!");


            #region Gun shit
            LanguageAPI.Add("ROB_DRIVER_PISTOL_NAME", "Pistol");
            LanguageAPI.Add("ROB_DRIVER_PISTOL_DESC", "A reliable handgun that excels at nothing.");

            LanguageAPI.Add("ROB_DRIVER_LUNAR_PISTOL_NAME", "Perfected Blaster");
            LanguageAPI.Add("ROB_DRIVER_LUNAR_PISTOL_DESC", "A perfect weapon with no flaws. Speed is war.");

            LanguageAPI.Add("ROB_DRIVER_VOID_PISTOL_NAME", "Corrupted Blaster");
            LanguageAPI.Add("ROB_DRIVER_VOID_PISTOL_DESC", "A weapon corrupted by the void.");

            LanguageAPI.Add("ROB_DRIVER_NEEDLER_NAME", "Needler");
            LanguageAPI.Add("ROB_DRIVER_NEEDLER_DESC", "Risk of Rain 2");

            LanguageAPI.Add("ROB_DRIVER_GOLDENGUN_NAME", "Golden Gun");
            LanguageAPI.Add("ROB_DRIVER_GOLDENGUN_DESC", "A legendary gun that deals extraordinary damage.");

            LanguageAPI.Add("ROB_DRIVER_PYRITEGUN_NAME", "Pyrite Gun");
            LanguageAPI.Add("ROB_DRIVER_PYRITEGUN_DESC", "A mockery of the original.");

            LanguageAPI.Add("ROB_DRIVER_SHOTGUN_NAME", "Shotgun");
            LanguageAPI.Add("ROB_DRIVER_SHOTGUN_DESC", "A close-range powerhouse that excels at overwhelming damage.");

            LanguageAPI.Add("ROB_DRIVER_RIOT_SHOTGUN_NAME", "Riot Shotgun");
            LanguageAPI.Add("ROB_DRIVER_RIOT_SHOTGUN_DESC", "A piercing shotgun that excels at crowd control and heavy damage.");

            LanguageAPI.Add("ROB_DRIVER_SLUG_SHOTGUN_NAME", "Slug Shotgun");
            LanguageAPI.Add("ROB_DRIVER_SLUG_SHOTGUN_DESC", "A shotgun loaded with slugs that excels at powerful single hits.");

            LanguageAPI.Add("ROB_DRIVER_BADASS_SHOTGUN_NAME", "Badass Shotgun");
            LanguageAPI.Add("ROB_DRIVER_BADASS_SHOTGUN_DESC", "A six-barreled shotgun...!?");

            LanguageAPI.Add("ROB_DRIVER_MACHINEGUN_NAME", "Machine Gun");
            LanguageAPI.Add("ROB_DRIVER_MACHINEGUN_DESC", "A gun that excels at something, I'm not really sure what.");

            LanguageAPI.Add("ROB_DRIVER_HEAVY_MACHINEGUN_NAME", "Heavy Machine Gun");
            LanguageAPI.Add("ROB_DRIVER_HEAVY_MACHINEGUN_DESC", "A machine gun with slower shots, higher damage and more consistent aim.");

            LanguageAPI.Add("ROB_DRIVER_UZIS_NAME", "Dual Uzis");
            LanguageAPI.Add("ROB_DRIVER_UZIS_DESC", "A pair of uzis with high recoil but ridiculous damage.");

            LanguageAPI.Add("ROB_DRIVER_BAZOOKA_NAME", "Bazooka");
            LanguageAPI.Add("ROB_DRIVER_BAZOOKA_DESC", "A chargeable bazooka good for clearing out crowds.");

            LanguageAPI.Add("ROB_DRIVER_SNIPER_NAME", "Sniper Rifle");
            LanguageAPI.Add("ROB_DRIVER_SNIPER_DESC", "A rifle specializing in precise, fatal shots.");

            LanguageAPI.Add("ROB_DRIVER_ROCKETLAUNCHER_NAME", "Rocket Launcher");
            LanguageAPI.Add("ROB_DRIVER_ROCKETLAUNCHER_DESC", "KABOOOM");

            LanguageAPI.Add("ROB_DRIVER_ROCKETLAUNCHER_ALT_NAME", "Prototype Rocket Launcher");
            LanguageAPI.Add("ROB_DRIVER_ROCKETLAUNCHER_ALT_DESC", "KABLOOEY");

            LanguageAPI.Add("ROB_DRIVER_ARMCANNON_NAME", "Arm Cannon");
            LanguageAPI.Add("ROB_DRIVER_ARMCANNON_DESC", "Arm Cannon scavenged from a Steel Mechorilla.");

            LanguageAPI.Add("ROB_DRIVER_PLASMACANNON_NAME", "Super Plasma Cannon");
            LanguageAPI.Add("ROB_DRIVER_PLASMACANNON_DESC", "POWERRR!!!");

            LanguageAPI.Add("ROB_DRIVER_BEETLESHIELD_NAME", "Chitin Shield");
            LanguageAPI.Add("ROB_DRIVER_BEETLESHIELD_DESC", "An offhand shield to protect you while you use your pistol.");

            LanguageAPI.Add("ROB_DRIVER_BEHEMOTH_NAME", "Brilliant Behemoth");
            LanguageAPI.Add("ROB_DRIVER_BEHEMOTH_DESC", "huh?");

            LanguageAPI.Add("ROB_DRIVER_GRENADELAUNCHER_NAME", "Grenade Launcher");
            LanguageAPI.Add("ROB_DRIVER_GRENADELAUNCHER_DESC", "");

            LanguageAPI.Add("ROB_DRIVER_LUNARRIFLE_NAME", "Chimeric Cannon");
            LanguageAPI.Add("ROB_DRIVER_LUNARRIFLE_DESC", "");

            LanguageAPI.Add("ROB_DRIVER_LUNARMINIGUN_NAME", "Chimeric Gatling");
            LanguageAPI.Add("ROB_DRIVER_LUNARMINIGUN_DESC", "");

            LanguageAPI.Add("ROB_DRIVER_LUNARGRENADE_NAME", "Chimeric Launcher");
            LanguageAPI.Add("ROB_DRIVER_LUNARGRENADE_DESC", "");

            LanguageAPI.Add("ROB_DRIVER_LUNARHAMMER_NAME", "Hammer of the King");
            LanguageAPI.Add("ROB_DRIVER_LUNARHAMMER_DESC", "Wield supreme power in the palm of your hand.");

            LanguageAPI.Add("ROB_DRIVER_NEMMANDO_NAME", "Reclaimer");
            LanguageAPI.Add("ROB_DRIVER_NEMMANDO_DESC", "");

            LanguageAPI.Add("ROB_DRIVER_NEMMERC_NAME", "Carnage");
            LanguageAPI.Add("ROB_DRIVER_NEMMERC_DESC", "");

            LanguageAPI.Add("ROB_DRIVER_GOLEMGUN_NAME", "Stone Cannon");
            LanguageAPI.Add("ROB_DRIVER_GOLEMGUN_DESC", "");
            #endregion
        }
    }
}