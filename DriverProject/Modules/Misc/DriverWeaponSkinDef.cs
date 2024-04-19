using UnityEngine;
using RoR2;
using RoR2.Skills;

[CreateAssetMenu(fileName = "rsd", menuName = "ScriptableObjects/DriverWeaponSkinDef", order = 1)]
public class DriverWeaponSkinDef : ScriptableObject
{
    [Header("General")]
    public string nameToken = "";
    public DriverWeaponDef weaponDef;
    public Mesh weaponSkinMesh;
    public Material weaponSkinMaterial;

    public static DriverWeaponSkinDef CreateWeaponSkinDefFromInfo(DriverWeaponSkinDefInfo skinDefInfo)
    {
        DriverWeaponSkinDef skinDef = (DriverWeaponSkinDef)ScriptableObject.CreateInstance(typeof(DriverWeaponSkinDef));
        skinDef.nameToken = skinDefInfo.nameToken;
        skinDef.weaponDef = skinDefInfo.weaponDef;
        skinDef.weaponSkinMesh = skinDefInfo.weaponSkinMesh;
        skinDef.weaponSkinMaterial = skinDefInfo.weaponSkinMaterial;
        return skinDef;
    }
    [System.Serializable]
    public struct DriverWeaponSkinDefInfo
    {
        public string nameToken;
        public DriverWeaponDef weaponDef;
        public Mesh weaponSkinMesh;
        public Material weaponSkinMaterial;
    }
}