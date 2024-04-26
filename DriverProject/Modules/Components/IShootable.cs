using RoR2;

namespace RobDriver.Modules.Components
{
    public interface IShootable
    {
        void OnShot(DamageInfo damageInfo);

        bool CanBeShot();

        RicochetUtils.RicochetPriority GetRicochetPriority();
    }
}