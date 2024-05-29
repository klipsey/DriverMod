using RobDriver.SkillStates.BaseStates;
namespace RobDriver.SkillStates.Driver.Compat
{
    public class WallJumpSmall : BaseDriverState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            base.PlayAnimation("FullBody, Override Soft", "Jump");
            this.outer.SetNextStateToMain();
        }
    }
}