using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Weapons;
using CriticalRole.Armored;

namespace CriticalRole.Character
{
    public enum StatsType
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    }

    public interface IHasStats
    {
        int GetStatMod(StatsType stat);

        int GetAttackMod(WeaponObject weapon);

        void AddWeaponProficiency(WeaponType proficiency);

        void AddSimpleWeaponsProficiency();

        void AddMartialWeaponsProficiency();

        bool IsProficient(WeaponType weapon);

        void AddArmorProficiency(ArmorType proficiency);

        bool IsProficient(ArmorType armorType);

     
        int MaxHealth { get; }

        int Level { get; }

        int ProficiencyBonus { get; }
    }



    public class HasStats : MonoBehaviour, IHasStats
    {

        //----------------------------------------------------------------------------
        //                    Health
        //----------------------------------------------------------------------------

        #region Health

        [Header("Health")]
        public int _MaxHealth;

        public int MaxHealth => _MaxHealth;

        #endregion



        //----------------------------------------------------------------------------
        //                    Core Stats
        //----------------------------------------------------------------------------

        #region Core Stats
        [Header("Core Stats")]
        public int Strength;
        public int Dexterity;
        public int Constitution;
        public int Intelligence;
        public int Wisdom;
        public int Charisma;

        public int GetStatMod(StatsType stat)
        {
            int score;
            switch (stat)
            {
                case StatsType.Strength:
                    score = Strength;
                    break;
                case StatsType.Dexterity:
                    score = Dexterity;
                    break;
                case StatsType.Constitution:
                    score = Constitution;
                    break;
                case StatsType.Intelligence:
                    score = Intelligence;
                    break;
                case StatsType.Wisdom:
                    score = Wisdom;
                    break;
                case StatsType.Charisma:
                    score = Charisma;
                    break;
                default:
                    score = 0;
                    break;
            }

            return Mathf.FloorToInt((score - 10) / 2.0f);
        }

        #endregion





        //----------------------------------------------------------------------------
        //                    Level
        //----------------------------------------------------------------------------

        #region Level

        [Header("Level")]
        public int _Level;
        public int Level
        {
            get
            {
                return _Level;
            }
        }

        #endregion



        //----------------------------------------------------------------------------
        //                    GetAttackMod
        //----------------------------------------------------------------------------

        #region GetAttackMod

        public int GetAttackMod(WeaponObject weapon)
        {
            int strMod = GetStatMod(StatsType.Strength);
            int dexMod = GetStatMod(StatsType.Dexterity);

            if (weapon.Finesse && (dexMod > strMod))
            {
                return dexMod;
            }
            return strMod;
        }

        #endregion




        //----------------------------------------------------------------------------
        //                    Proficiency Bonus
        //----------------------------------------------------------------------------

        #region Proficiency Bonus

        public int ProficiencyBonus
        {
            get
            {
                return ((Level - 1) / 4) + 2;
            }
        }


        #endregion




        //----------------------------------------------------------------------------
        //                    Weapon Proficiencies
        //----------------------------------------------------------------------------

        #region Weapon Proficiencies

        public HashSet<WeaponType> _WeaponProficienciesSet;
        public HashSet<WeaponType> WeaponProficienciesSet
        {
            get
            {
                if(_WeaponProficienciesSet == null)
                {
                    _WeaponProficienciesSet = new HashSet<WeaponType>();
                }
                return _WeaponProficienciesSet;
            }
        }

        public bool IsProficient(WeaponType weapon)
        {
            return WeaponProficienciesSet.Contains(weapon);
        }

        public void AddWeaponProficiency(WeaponType proficiency)
        {
            WeaponProficienciesSet.Add(proficiency);
        }

        public void AddSimpleWeaponsProficiency()
        {
            AddWeaponProficiency(WeaponType.Club);
            AddWeaponProficiency(WeaponType.Dagger);
            AddWeaponProficiency(WeaponType.Greatclub);
            AddWeaponProficiency(WeaponType.Handaxe);
            AddWeaponProficiency(WeaponType.Javelin);
            AddWeaponProficiency(WeaponType.HammerLight);
            AddWeaponProficiency(WeaponType.Mace);
            AddWeaponProficiency(WeaponType.Quarterstaff);
            AddWeaponProficiency(WeaponType.Sickle);
            AddWeaponProficiency(WeaponType.Spear);
            AddWeaponProficiency(WeaponType.CrossbowLight);
            AddWeaponProficiency(WeaponType.Dart);
            AddWeaponProficiency(WeaponType.Shortbow);
            AddWeaponProficiency(WeaponType.Sling);
        }

        public void AddMartialWeaponsProficiency()
        {
            AddWeaponProficiency(WeaponType.Battleaxe);
            AddWeaponProficiency(WeaponType.Flail);
            AddWeaponProficiency(WeaponType.Glaive);
            AddWeaponProficiency(WeaponType.Greataxe);
            AddWeaponProficiency(WeaponType.Greatsword);
            AddWeaponProficiency(WeaponType.Halberd);
            AddWeaponProficiency(WeaponType.Lance);
            AddWeaponProficiency(WeaponType.Longsword);
            AddWeaponProficiency(WeaponType.Maul);
            AddWeaponProficiency(WeaponType.Morningstar);
            AddWeaponProficiency(WeaponType.Pike);
            AddWeaponProficiency(WeaponType.Rapier);
            AddWeaponProficiency(WeaponType.Scimitar);
            AddWeaponProficiency(WeaponType.Shortsword);
            AddWeaponProficiency(WeaponType.Trident);
            AddWeaponProficiency(WeaponType.WarPick);
            AddWeaponProficiency(WeaponType.Warhammer);
            AddWeaponProficiency(WeaponType.Blowgun);
            AddWeaponProficiency(WeaponType.CrossbowHand);
            AddWeaponProficiency(WeaponType.CrossbowHeavy);
            AddWeaponProficiency(WeaponType.Longbow);
            AddWeaponProficiency(WeaponType.Net);
        }

        #endregion



        //----------------------------------------------------------------------------
        //                    Armor Proficiencies
        //----------------------------------------------------------------------------

        #region Armor Proficiency

        public HashSet<ArmorType> _ArmorProficiencySet;
        public HashSet<ArmorType> ArmorProficiencySet
        {
            get
            {
                if(_ArmorProficiencySet == null)
                {
                    _ArmorProficiencySet = new HashSet<ArmorType>();
                }
                return _ArmorProficiencySet;
            }
        }

        public bool IsProficient(ArmorType armorType)
        {
            return ArmorProficiencySet.Contains(armorType);
        }

        public void AddArmorProficiency(ArmorType proficiency)
        {
            ArmorProficiencySet.Add(proficiency);
        }

        #endregion
    
    }






}
