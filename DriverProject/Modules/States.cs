using System.Collections.Generic;
using System;

namespace RobDriver.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void AddSkill(Type t)
        {
            entityStates.Add(t);
        }

        public static void RegisterStates()
        {
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.MainState));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Slide));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SteadyAim));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.ThrowGrenade));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.JammedGun));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Revolver.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Revolver.SteadyAim));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Shotgun.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Shotgun.Bash));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Shotgun.Reload));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.MachineGun.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.MachineGun.Zap));

            entityStates.Add(typeof(RobDriver.SkillStates.Emote.BaseEmote));
            entityStates.Add(typeof(RobDriver.SkillStates.Emote.Rest));
            entityStates.Add(typeof(RobDriver.SkillStates.Emote.Taunt));
            entityStates.Add(typeof(RobDriver.SkillStates.Emote.Dance));

            entityStates.Add(typeof(RobDriver.SkillStates.FuckMyAss));
        }
    }
}