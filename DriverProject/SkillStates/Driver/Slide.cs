using UnityEngine;
using RoR2;
using EntityStates;

namespace RobDriver.SkillStates.Driver
{
	public class Slide : BaseState
	{
		private Vector3 forwardDirection;
		private GameObject slideEffectInstance;
		private bool startedStateGrounded;

		public override void OnEnter()
		{
			base.OnEnter();
			Util.PlaySound(EntityStates.Commando.SlideState.soundString, base.gameObject);

			if (base.inputBank && base.characterDirection)
			{
				base.characterDirection.forward = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
			}

			if (base.characterMotor)
			{
				this.startedStateGrounded = base.characterMotor.isGrounded;
			}

			base.characterBody.SetSpreadBloom(0f, false);

			if (!this.startedStateGrounded)
			{
				this.PlayAnimation("Body", "SprintJump");
				Vector3 velocity = base.characterMotor.velocity;
				velocity.y = base.characterBody.jumpPower;
				base.characterMotor.velocity = velocity;
				return;
			}

			base.PlayAnimation("FullBody, Override", "Slide", "Slide.playbackRate", EntityStates.Commando.SlideState.slideDuration);

			if (EntityStates.Commando.SlideState.slideEffectPrefab)
			{
				Transform parent = base.FindModelChild("Root");
				this.slideEffectInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Commando.SlideState.slideEffectPrefab, parent);
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority)
			{
				float num = this.startedStateGrounded ? EntityStates.Commando.SlideState.slideDuration : EntityStates.Commando.SlideState.jumpDuration;

				if (base.inputBank && base.characterDirection)
				{
					base.characterDirection.moveVector = base.inputBank.moveVector;
					this.forwardDirection = base.characterDirection.forward;
				}

				if (base.characterMotor)
				{
					float num2;

					if (this.startedStateGrounded)
					{
						num2 = EntityStates.Commando.SlideState.forwardSpeedCoefficientCurve.Evaluate(base.fixedAge / num);
					}
					else
					{
						num2 = EntityStates.Commando.SlideState.jumpforwardSpeedCoefficientCurve.Evaluate(base.fixedAge / num);
					}

					base.characterMotor.rootMotion += num2 * this.moveSpeedStat * this.forwardDirection * Time.fixedDeltaTime;
				}
				if (base.fixedAge >= num)
				{
					this.outer.SetNextStateToMain();
				}
			}
		}

		public override void OnExit()
		{
			this.PlayImpactAnimation();
			if (this.slideEffectInstance) EntityState.Destroy(this.slideEffectInstance);

			base.OnExit();
		}

		private void PlayImpactAnimation()
		{
			Animator modelAnimator = base.GetModelAnimator();
			int layerIndex = modelAnimator.GetLayerIndex("Impact");
			if (layerIndex >= 0)
			{
				modelAnimator.SetLayerWeight(layerIndex, 1f);
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}