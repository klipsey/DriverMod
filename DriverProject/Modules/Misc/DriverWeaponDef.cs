using UnityEngine;
using RoR2;
using RoR2.Skills;

[CreateAssetMenu(fileName = "wpn", menuName = "ScriptableObjects/WeaponDef", order = 1)]
public class DriverWeaponDef : ScriptableObject
{
    [Header("General")]
    public string nameToken = "";
    public string descriptionToken = "";
    public Texture icon = null;
    public GameObject crosshairPrefab = null;
    public DriverWeaponTier tier = DriverWeaponTier.Common;
    public float baseDuration = 8f;

    [Header("Skills")]
    public SkillDef primarySkillDef;
    public SkillDef secondarySkillDef;

    [Header("Visuals")]
    public Mesh mesh;
    public Material material;
    public string animLayer;

    [HideInInspector]
    public ushort index; // assigned at runtime

    public static DriverWeaponDef CreateWeaponDefFromInfo(DriverWeaponDefInfo weaponDefInfo)
    {
        DriverWeaponDef weaponDef = new DriverWeaponDef();

        weaponDef.nameToken = weaponDefInfo.nameToken;
        weaponDef.descriptionToken = weaponDefInfo.descriptionToken;
        weaponDef.icon = weaponDefInfo.icon;
        weaponDef.crosshairPrefab = weaponDefInfo.crosshairPrefab;
        weaponDef.tier = weaponDefInfo.tier;
        weaponDef.baseDuration = weaponDefInfo.baseDuration;

        weaponDef.primarySkillDef = weaponDefInfo.primarySkillDef;
        weaponDef.secondarySkillDef = weaponDefInfo.secondarySkillDef;

        weaponDef.mesh = weaponDefInfo.mesh;
        weaponDef.material = weaponDefInfo.material;
        weaponDef.animLayer = weaponDefInfo.animLayer;

        return weaponDef;
    }
}

[System.Serializable]
public struct DriverWeaponDefInfo
{
    public string nameToken;
    public string descriptionToken;
    public Texture icon;
    public GameObject crosshairPrefab;
    public DriverWeaponTier tier;
    public float baseDuration;

    public SkillDef primarySkillDef;
    public SkillDef secondarySkillDef;

    public Mesh mesh;
    public Material material;
    public string animLayer;
}

public enum DriverWeaponTier
{
    Common,
    Uncommon,
    Legendary,
    Unique
}