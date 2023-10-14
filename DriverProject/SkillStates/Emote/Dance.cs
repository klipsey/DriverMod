namespace RobDriver.SkillStates.Emote
{
    public class Dance : BaseEmote
    {
        public override void OnEnter()
        {
            base.OnEnter();

            // hardcoded, fix at some point
            if (this.characterBody.skinIndex == 1) this.PlayEmote("Dance", "sfx_jacket_dance");
            else this.PlayEmote("Dance", "sfx_driver_dance");

            this.FindModelChild("Boombox").gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            this.FindModelChild("Boombox").gameObject.SetActive(false);
        }
    }
}