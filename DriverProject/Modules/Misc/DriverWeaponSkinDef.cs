using UnityEngine;
using RoR2;
using DriverMod.Modules;
using RoR2.Skills;
using RobDriver;

[CreateAssetMenu(fileName = "rsd", menuName = "ScriptableObjects/DriverWeaponSkinDef", order = 3)]
public class DriverWeaponSkinDef : ScriptableObject
{
    [Header("General")]
    public string nameToken = "";
    public string skinIndex = "";
    public ushort weaponDefIndex;
    public Mesh weaponSkinMesh;
    public Material weaponSkinMaterial;

    public static DriverWeaponSkinDef CreateWeaponSkinDefFromInfo(DriverWeaponSkinDefInfo skinDefInfo)
    {
        DriverWeaponSkinDef weaponSkinDef = (DriverWeaponSkinDef)ScriptableObject.CreateInstance(typeof(DriverWeaponSkinDef));
        weaponSkinDef.nameToken = skinDefInfo.nameToken;
        weaponSkinDef.skinIndex = skinDefInfo.mainSkinName;
        weaponSkinDef.weaponDefIndex = skinDefInfo.weaponDefIndex;
        weaponSkinDef.weaponSkinMesh = skinDefInfo.weaponSkinMesh;
        weaponSkinDef.weaponSkinMaterial = skinDefInfo.weaponSkinMaterial;
        return weaponSkinDef;
    }
    [System.Serializable]
    public struct DriverWeaponSkinDefInfo
    {
        public string nameToken;
        public string mainSkinName;
        public ushort weaponDefIndex;
        public Mesh weaponSkinMesh;
        public Material weaponSkinMaterial;
    }
}