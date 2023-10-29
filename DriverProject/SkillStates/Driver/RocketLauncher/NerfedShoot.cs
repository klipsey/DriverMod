namespace RobDriver.SkillStates.Driver.RocketLauncher
{
    public class NerfedShoot : Shoot
    {
        protected override float _damageCoefficient => 4.5f;
    }
}