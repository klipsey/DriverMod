using R2API;
using RoR2;
using RobDriver.Modules;
using UnityEngine;

[CreateAssetMenu(fileName = "blt", menuName = "ScriptableObjects/BulletDef", order = 2)]
public class DriverBulletDef : ScriptableObject
{
    [Header("General")]
    public string nameToken = "";
    public DamageType bulletType = DamageType.Generic;
    public DamageAPI.ModdedDamageType moddedBulletType = DamageTypes.Generic;
    public DriverWeaponTier tier = DriverWeaponTier.Common;

    [Header("Visuals")]
    public Sprite icon = null;
    public Color trailColor = Color.black;

    [HideInInspector]
    public ushort index; // assigned at runtime

    public static DriverBulletDef CreateBulletDefFromInfo(DriverBulletDefInfo bulletDefInfo)
    {
        DriverBulletDef bulletDef = (DriverBulletDef)ScriptableObject.CreateInstance(typeof(DriverBulletDef));
        bulletDef.name = bulletDefInfo.nameToken;
        bulletDef.nameToken = bulletDefInfo.nameToken;
        bulletDef.bulletType = bulletDefInfo.bulletType;
        bulletDef.moddedBulletType = bulletDefInfo.moddedBulletType;
        bulletDef.tier = bulletDefInfo.tier;
        bulletDef.icon = bulletDefInfo.icon;
        bulletDef.trailColor = bulletDefInfo.trailColor;
        return bulletDef;
    }
}

[System.Serializable]
public struct DriverBulletDefInfo
{
    public string nameToken;
    public DamageType bulletType;
    public DamageAPI.ModdedDamageType moddedBulletType;
    public DriverWeaponTier tier;

    public Sprite icon;
    public Color trailColor;
}
