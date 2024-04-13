namespace RobDriver.SkillStates.Driver
{
    public class WaitForReload : BaseDriverSkillState
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= 1f && base.isAuthority)
            {
                if (this.iDrive.weaponTimer == this.iDrive.maxWeaponTimer || this.iDrive.passive.isBullets || this.iDrive.passive.isRyan)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }

                if (this.iDrive.weaponTimer <= 0f)
                {
                    this.outer.SetNextState(new ReloadPistol());
                }
                else
                {
                    this.outer.SetNextState(new ReloadPistol
                    {
                        interruptPriority = EntityStates.InterruptPriority.Any
                    });
                }
            }
        }
    }
}