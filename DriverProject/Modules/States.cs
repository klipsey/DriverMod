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
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.JammedGun));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.BaseDriverSkillState));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Slide));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SteadyAim));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.ThrowGrenade));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.ThrowMolotov));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SwingKnife));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.UseSyringe));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.ArmCannon.Shoot));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.BeetleShield.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.BeetleShield.SteadyAim));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.GrenadeLauncher.Shoot));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Bazooka.Charge));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Bazooka.Fire));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.GoldenGun.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.GoldenGun.AimLightsOut));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.GoldenGun.LightsOut));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Shotgun.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Shotgun.Bash));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Shotgun.Reload));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.RiotShotgun.Shoot));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SlugShotgun.Shoot));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.BadassShotgun.Shoot));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.HeavyMachineGun.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.HeavyMachineGun.ShootGrenade));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.LunarPistol.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.LunarPistol.SteadyAim));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.MachineGun.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.MachineGun.Zap));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.PlasmaCannon.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.PlasmaCannon.Barrage));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.RocketLauncher.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.RocketLauncher.Barrage));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.RocketLauncher.NerfedShoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.RocketLauncher.NerfedBarrage));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.PyriteGun.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.PyriteGun.SteadyAim));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SniperRifle.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SniperRifle.SteadyAim));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SniperRifle.Aim));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.VoidPistol.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.VoidPistol.SteadyAim));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.VoidRifle.Shoot));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.BadassShotgun.Shoot));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.LunarRifle.Shoot));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.LunarGrenade.Shoot));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.LunarHammer.SwingCombo));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.LunarHammer.FireShard));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Compat.NemmandoGun.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Compat.NemmandoGun.Submission));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Compat.NemmercGun.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Compat.NemmercGun.Shoot2));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.GolemGun.ChargeLaser));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.GolemGun.FireLaser));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.ArmBFG.Shoot));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.ArtiGauntlet.Shoot));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SMG.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SMG.PhaseRound));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SMG.SuppressiveFire));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Revolver.Shoot));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Revolver.AimLightsOut));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Revolver.AimLightsOutReset));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Revolver.LightsOut));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.Revolver.LightsOutReset));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SupplyDrop.AimSupplyDrop));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SupplyDrop.FireSupplyDrop));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SupplyDrop.CancelSupplyDrop));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SupplyDrop.Nerfed.AimCrapDrop));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SupplyDrop.Nerfed.FireCrapDrop));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SupplyDrop.Nerfed.CancelCrapDrop));

            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SupplyDrop.Scepter.AimVoidDrop));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SupplyDrop.Scepter.FireVoidDrop));
            entityStates.Add(typeof(RobDriver.SkillStates.Driver.SupplyDrop.Scepter.CancelVoidDrop));

            entityStates.Add(typeof(RobDriver.SkillStates.Emote.BaseEmote));
            entityStates.Add(typeof(RobDriver.SkillStates.Emote.Rest));
            entityStates.Add(typeof(RobDriver.SkillStates.Emote.Taunt));
            entityStates.Add(typeof(RobDriver.SkillStates.Emote.Dance));

            entityStates.Add(typeof(RobDriver.SkillStates.FuckMyAss));
        }
    }
}