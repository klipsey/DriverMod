using UnityEngine;
using RoR2;
using EntityStates;
using UnityEngine.AddressableAssets;

namespace RobDriver.SkillStates.Driver.Revolver
{
    public class AimLightsOut : BaseDriverSkillState
    {
        public static float baseDuration = 0.6f;

        private float duration;
        private GameObject effectInstance;
        private uint spinPlayID;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = AimLightsOut.baseDuration / this.attackSpeedStat;

            this.effectInstance = GameObject.Instantiate(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoReloadFX.prefab").WaitForCompletion());
            this.effectInstance.transform.parent = this.FindModelChild("Pistol");
            this.effectInstance.transform.localRotation = Quaternion.Euler(new Vector3(0f, 80f, 0f));
            this.effectInstance.transform.localPosition = Vector3.zero;

            this.spinPlayID = Util.PlaySound("sfx_driver_pistol_spin", this.gameObject);

            base.PlayAnimation("Gesture, Override", "AimLightsOut", "Action.playbackRate", this.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.StartAimMode(0.5f);

            if (base.fixedAge >= (0.9f * this.duration))
            {
                if (this.effectInstance)
                {
                    if (this.spinPlayID != 0u) AkSoundEngine.StopPlayingID(this.spinPlayID);
                    EntityState.Destroy(this.effectInstance);

                    Util.PlaySound("sfx_driver_pistol_ready", this.gameObject);
                }
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                if (!this.inputBank.skill2.down)
                {
                    this.outer.SetNextState(new LightsOut());
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}