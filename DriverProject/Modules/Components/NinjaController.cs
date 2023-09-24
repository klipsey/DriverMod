using RoR2;
using UnityEngine;

namespace NinjaMod.Modules.Components
{
    public class NinjaController : MonoBehaviour
    {
        public int maxAirDashes = 2;
        public int currentAirDashes;
        private CharacterBody characterBody;
        private InputBankTest inputBank;

        public bool isSprinting;

        private void Awake()
        {
            this.characterBody = this.GetComponent<CharacterBody>();
            this.inputBank = this.GetComponent<InputBankTest>();
        }

        private void FixedUpdate()
        {
            if (this.inputBank) if (this.inputBank.skill3.down) this.isSprinting = true;
            else this.isSprinting = false;

            if (this.characterBody) this.characterBody.isSprinting = this.isSprinting;
        }

        private void Update()
        {
            if (this.characterBody) this.characterBody.isSprinting = this.isSprinting;
        }

        private void LateUpdate()
        {
            if (this.characterBody) this.characterBody.isSprinting = this.isSprinting;
        }
    }
}