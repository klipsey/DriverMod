using EntityStates;
using R2API;
using RobDriver.Modules.Survivors;
using RoR2.Skills;
using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace RobDriver.Modules.Weapons
{
    public abstract class BaseWeapon<T> : BaseWeapon where T : BaseWeapon<T>
    {
        public static T instance { get; private set; }

        public BaseWeapon()
        {
            if (instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting BaseWeapon was instantiated twice");
            instance = this as T;
        }
    }

    public abstract class BaseWeapon
    {
        public DriverWeaponDef weaponDef { get; set; }
        public abstract string weaponNameToken { get; }
        public abstract string weaponName { get; }
        public abstract string weaponDesc { get; }
        public abstract string iconName { get; }
        public abstract GameObject crosshairPrefab { get; }
        public abstract DriverWeaponTier tier { get; }
        public abstract int shotCount { get; }
        public abstract DriverWeaponDef.BuffType buffType { get; }
        public abstract SkillDef primarySkillDef { get; }
        public abstract SkillDef secondarySkillDef { get; }
        public abstract Mesh mesh { get; }
        public abstract Material material { get; }
        public abstract DriverWeaponDef.AnimationSet animationSet { get; }
        public abstract string calloutSoundString { get; }
        public abstract string configIdentifier { get; }
        public abstract float dropChance { get; }
        public abstract bool addToPool { get; }
        public abstract string uniqueDropBodyName { get; }

        public abstract void Init();

        protected void CreateLang()
        {
            LanguageAPI.Add("ROB_DRIVER_WEAPON_" + weaponNameToken + "_NAME", weaponName);
            LanguageAPI.Add("ROB_DRIVER_WEAPON_" + weaponNameToken + "_DESC", weaponDesc);
        }

        protected void CreateWeapon()
        {
            Texture icon = null;
            DriverWeaponTier changeTier = tier;
            float changeDropChance = dropChance;
            if (iconName != "") icon = Modules.Assets.mainAssetBundle.LoadAsset<Texture>(iconName);

            if(tier == DriverWeaponTier.Unique && Modules.Config.uniqueDropsAreLegendary.Value)
            {
                changeTier = DriverWeaponTier.Legendary;
                changeDropChance = 0f;
            }

            weaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = "ROB_DRIVER_WEAPON_" + weaponNameToken + "_NAME",
                descriptionToken = "ROB_DRIVER_WEAPON_" + weaponNameToken + "_DESC",
                icon = icon,
                crosshairPrefab = crosshairPrefab,
                tier = changeTier,
                shotCount = shotCount,
                primarySkillDef = primarySkillDef,
                secondarySkillDef = secondarySkillDef,
                mesh = mesh,
                material = material,
                animationSet = animationSet,
                calloutSoundString = calloutSoundString,
                configIdentifier = configIdentifier,
                dropChance = changeDropChance,
                buffType = buffType
            });
            DriverWeaponCatalog.AddWeapon(weaponDef);

            Skills.AddSkillToFamily(Driver.characterPrefab.GetComponent<Components.DriverArsenal>().weaponSkillSlot.skillFamily,
                Modules.Skills.CreateSkillDef(new SkillDefInfo(
                skillName: weaponDef.name,
                skillNameToken: weaponDef.nameToken,
                skillDescriptionToken: weaponDef.descriptionToken,
                skillIcon: Sprite.Create(weaponDef.icon as Texture2D, new Rect(0, 0, weaponDef.icon.width, weaponDef.icon.height), new Vector2(0.5f, 0.5f)),
                activationState: new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle)),
                activationStateMachineName: "",
                interruptPriority: InterruptPriority.Any,
                isCombatSkill: false,
                baseRechargeInterval: 0)));

            if (uniqueDropBodyName != "")
            {
                DriverWeaponCatalog.AddWeaponDrop(uniqueDropBodyName, weaponDef);
            }
        }
    }
}