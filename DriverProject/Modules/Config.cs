using BepInEx.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace RobDriver.Modules
{
    internal static class Config
    {

        internal static void ReadConfig()
        {
        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return DriverPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }

        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return DriverPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this character"));
        }
    }


    public class StageSpawnInfo {
        private string stageName;
        private int minStages;

        public StageSpawnInfo(string stageName, int minStages) {
            this.stageName = stageName;
            this.minStages = minStages;
        }

        public string GetStageName() { return stageName; }
        public int GetMinStages() { return minStages; }
    }
}