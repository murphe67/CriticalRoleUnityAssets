using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Weapons;
using CriticalRole.Damage;
using CriticalRole.Character;
using CriticalRole.Alterations;
using CriticalRole.Health;


namespace CriticalRole.Attacking
{

    //----------------------------------------------------------------------------
    //             Class Description
    //----------------------------------------------------------------------------
    //
    // HasAttack is still a major grey area to me of what it actually is
    //
    // I think it's going to be the interface between a characters stats
    // and the AttackManager.
    //
    // Currently the AttackManager looks for the AttackData from it
    // So wherever the attack gets configured will have to send it to the HasAttack
    //
    // Also AttackAlterations get hooked to it as the ID of the attacker

    public interface IHasAttack
    {
        void Initialise();
        WeaponObject MyWeapon { get; }
        void RegisterAlterationManager(IAlterationManager alterationManager);

        /// <summary>
        /// Get all the information the attacker has on the attack
        /// </summary>
        /// <returns></returns>
        IAttackData GetAttackData();
        IEnumerator RollDamage(AttackRoll attackRoll);

        string MyName { get; }
    }

    public class HasAttack : MonoBehaviour, IHasAttack
    {

        //----------------------------------------------------------------------------
        //             Initialise
        //----------------------------------------------------------------------------

        #region Initialise
        public void Initialise()
        {
            Equipment equipment = GetComponent<Equipment>();
            MyWeapon = equipment.MyWeapon;

            IHasStats hasStats = GetComponent<IHasStats>();
            int hitBonusInt = 0;
            if(hasStats.IsProficient(MyWeapon.MyWeaponType))
            {
                hitBonusInt += hasStats.ProficiencyBonus;
            }

            hitBonusInt += hasStats.GetAttackMod(MyWeapon);
            

            ToHitBonus toHitBonus = new ToHitBonus(hitBonusInt);
            MyAlterationManager.MyAttackDataAlterer.AddAttackAlteration(this, toHitBonus);

            int damageStatModInt = hasStats.GetAttackMod(MyWeapon);
            DamageStatMod damageStatMod = new DamageStatMod(damageStatModInt);
            MyAlterationManager.MyDamageDataAlterer.RegisterDamageAlteration(this, damageStatMod);
        }

        public WeaponObject MyWeapon { get; set; }

        #endregion


        //----------------------------------------------------------------------------
        //             RegisterAlterationManager
        //----------------------------------------------------------------------------

        #region RegisterAlterationManager
        public void RegisterAlterationManager(IAlterationManager alterationManager)
        {
            MyAlterationManager = alterationManager;
        }

        public IAlterationManager MyAlterationManager;


        #endregion
        
        
        



        //----------------------------------------------------------------------------
        //            GetAttackData
        //----------------------------------------------------------------------------

        #region GetAttackData

        public IAttackData GetAttackData()
        {
            return new AttackData(this, AttackRangeType.Melee, AttackType.Weapon, AttackStat.Strength);
        }

        #endregion





        //----------------------------------------------------------------------------
        //             RollDamage
        //----------------------------------------------------------------------------

        #region RollDamage

        public IEnumerator RollDamage(AttackRoll attackRoll)
        {
            attackRoll.MyDamageData.AddDamage(MyWeapon.RollDamage(), MyWeapon.MyDamageType, DamageSource.Roll); 
            yield break;
        }

        #endregion




        //----------------------------------------------------------------------------
        //             MyName
        //----------------------------------------------------------------------------

        #region MyName

        public string MyName
        {
            get
            {
                return gameObject.name;
            }
        }

        #endregion
    }

}
