using UnityEngine;
using RoR2;
using UnityEngine.AddressableAssets;

namespace RobDriver.Modules.Components
{
    public class DiscardedWeaponComponent : MonoBehaviour
    {
        public SkinnedMeshRenderer targetRenderer { get; set; }
        public Rigidbody rb { get; set; }

        private Transform targetTransform;
        private float lifetime = 60f;
        public float rotateSpeedX = 0f;
        public float rotateSpeedZ = -1200f;
        private bool spinning = false;
        private GameObject effectInstance;
        private DriverWeaponDef weaponDef;
        private float stopwatch;

        private void Awake()
        {
            if (!this.targetRenderer) this.targetRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
            if (!this.rb) this.rb = this.GetComponent<Rigidbody>();
            if (!this.targetTransform) this.targetTransform = this.transform.GetChild(1);

            Destroy(this.gameObject, this.lifetime);
        }

        private void FixedUpdate()
        {
            if (this.targetTransform && this.spinning)
            {
                this.stopwatch += Time.fixedDeltaTime;

                this.targetTransform.RotateAround(this.transform.position, this.transform.forward, this.rotateSpeedX * Time.fixedDeltaTime);
                this.targetTransform.RotateAround(this.transform.position, this.transform.right, this.rotateSpeedZ * Time.fixedDeltaTime);
                //this.targetTransform.Rotate(new Vector3(Time.fixedDeltaTime * this.rotateSpeed), this.targetTransform.localRotation.eulerAngles.y + (Time.fixedDeltaTime * this.rotateSpeedY), this.targetTransform.localRotation.eulerAngles.z + (Time.fixedDeltaTime * this.rotateSpeedZ)));
                //this.targetTransform.localRotation = Quaternion.Euler(;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (this.spinning && this.stopwatch >= 0.25f)
            {
                this.spinning = false;
                if (this.effectInstance) Destroy(this.effectInstance);

                if (this.rb) this.rb.collisionDetectionMode = CollisionDetectionMode.Discrete; // optimization

                Util.PlaySound("sfx_driver_gun_drop", this.gameObject);
            }
        }

        private void StartSpin()
        {
            this.spinning = true;

            if (this.weaponDef && this.weaponDef.animationSet == DriverWeaponDef.AnimationSet.TwoHanded)
            {
                //this.effectInstance.transform.localPosition = new Vector3(-0.3f, 0f, 0f);
                //this.effectInstance.transform.localScale = new Vector3(3.5f, 2.5f, -32f);
            }
            else
            {
                this.effectInstance = GameObject.Instantiate(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoReloadFX.prefab").WaitForCompletion());
                this.effectInstance.transform.parent = this.transform;
                this.effectInstance.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                this.effectInstance.transform.localPosition = Vector3.zero;
            }

            Util.PlaySound("sfx_driver_gun_throw", this.gameObject);
        }

        public void Init(DriverWeaponDef weaponDef, Vector3 force)
        {
            if (this.targetRenderer)
            {
                this.targetRenderer.sharedMesh = weaponDef.mesh;
                this.targetRenderer.material = weaponDef.material;
            }

            if (this.rb) this.rb.velocity = force;

            this.weaponDef = weaponDef;
            this.StartSpin();
        }
    }
}