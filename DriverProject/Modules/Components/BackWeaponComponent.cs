using UnityEngine;
using RoR2;
using UnityEngine.AddressableAssets;

namespace RobDriver.Modules.Components
{
    public class BackWeaponComponent : MonoBehaviour
    {
        public SkinnedMeshRenderer targetRenderer { get; set; }

        private void Awake()
        {
            if (!this.targetRenderer) this.targetRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        public void Init(DriverWeaponDef weaponDef)
        {
            if (this.targetRenderer)
            {
                this.targetRenderer.sharedMesh = weaponDef.mesh;
                this.targetRenderer.material = weaponDef.material;
            }
        }
    }
}