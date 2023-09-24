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
            //AddSkill(typeof(BaseRegiSkillState));
        }
    }
}