using System.Collections.Generic;
using System;

namespace RobDriver.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void AddSkill(Type t)
        {
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.MainState));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Slide));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SteadyAim));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.ThrowGrenade));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Revolver.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Revolver.SteadyAim));
        }

        public static void RegisterStates()
        {
            //AddSkill(typeof(BaseRegiSkillState));
        }
    }
}