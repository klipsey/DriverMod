namespace RobDriver.SkillStates.Driver
{
    public class WaitForReload : BaseDriverSkillState
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!base.isAuthority) return;

            if (this.iDrive.weaponTimer == this.iDrive.maxWeaponTimer ||
                this.iDrive.weaponDef != this.iDrive.defaultWeaponDef ||
                iDrive.HasSpecialBullets)
            {
                this.outer.SetNextStateToMain();
                return;
            }

            if (base.fixedAge >= 1f)
            {
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