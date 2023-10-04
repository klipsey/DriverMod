namespace RobDriver.SkillStates.Emote
{
    public class Dance : BaseEmote
    {
        public override void OnEnter()
        {
            base.OnEnter();

            this.PlayEmote("Dance", "sfx_driver_dance");
            this.FindModelChild("Boombox").gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            this.FindModelChild("Boombox").gameObject.SetActive(false);
        }
    }
}