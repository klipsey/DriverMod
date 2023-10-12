using RoR2;
using EntityStates;

namespace RobDriver.SkillStates.Driver
{
    public class SwingKnife : EntityStates.Bandit2.Weapon.SlashBlade
    {
        // i don't know if this will work lmao
        public override void OnEnter()
        {
            base.OnEnter();
            this.FindModelChild("KnifeModel").gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            this.FindModelChild("KnifeModel").gameObject.SetActive(false);
        }

        public override void PlayAnimation()
        {
            this.PlayCrossfade("Gesture, Override", "SwingKnife", "Knife.playbackRate", this.duration, 0.1f);
        }
    }
}