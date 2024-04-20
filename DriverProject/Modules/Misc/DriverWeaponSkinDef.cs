using UnityEngine;
using RoR2;
using DriverMod.Modules;
using RoR2.Skills;
using RobDriver;

[CreateAssetMenu(fileName = "rsd", menuName = "ScriptableObjects/DriverWeaponSkinDef", order = 1)]
public class DriverWeaponSkinDef : ScriptableObject
{
    [Header("General")]
    public string nameToken = "";
    public ushort weaponDefIndex = DriverWeaponCatalog.Pistol.index;
    public Mesh weaponSkinMesh = DriverWeaponCatalog.Pistol.mesh;
    public Material weaponSkinMaterial = DriverWeaponCatalog.Pistol.material;

    public static DriverWeaponSkinDef CreateWeaponSkinDefFromInfo(DriverWeaponSkinDefInfo skinDefInfo)
    {
        DriverWeaponSkinDef weaponSkinDef = (DriverWeaponSkinDef)ScriptableObject.CreateInstance(typeof(DriverWeaponSkinDef));
        weaponSkinDef.nameToken = skinDefInfo.nameToken;
        weaponSkinDef.weaponDefIndex = skinDefInfo.weaponDefIndex;
        weaponSkinDef.weaponSkinMesh = skinDefInfo.weaponSkinMesh;
        weaponSkinDef.weaponSkinMaterial = skinDefInfo.weaponSkinMaterial;
        return weaponSkinDef;
    }
    [System.Serializable]
    public struct DriverWeaponSkinDefInfo
    {
        public string nameToken;
        public ushort weaponDefIndex;
        public Mesh weaponSkinMesh;
        public Material weaponSkinMaterial;
    }
}