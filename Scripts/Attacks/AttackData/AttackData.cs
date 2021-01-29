using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Weapons;


namespace CriticalRole.Attacking
{
    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------
    //
    // 5e's modification of attack rolls is computationally very difficult,
    // and also easily understood by people.
    //
    // Any effect that 'alters' an attack needs to do everything and anything about
    // the attack made.
    //
    // This AttackData both stores the unchangeable data to facilitate
    // decision making, and also functions for adding advantage, disadvantage and 
    // and a pure delta to the roll.
    //
    // The implementation here of editing variables that form a control system,
    // rather than editing the actual number,
    //
    // is more elegant than in the movement calculation. Possibly redo movement based 
    // on this?


    public interface IAttackData
    {
        IHasAttack MyHasAttack { get; }
        IIsVictim MyIsVictim { get; }

        void SetIsVictim(IIsVictim victim);

        bool Advantage { get; }
        bool Disadvantage { get; }

        void AddAdvantage();

        void AddDisadvantage();


        int SumOfModifiers { get; }

        void AddModifier(int mod);

        IEnumerator RollForAttack();

        AttackRoll MyAttackRoll { get; }
    }

    public class AttackData : IAttackData
    {
        //----------------------------------------------------------------------------
        //                    Constructor
        //----------------------------------------------------------------------------

        #region Constructor

        public AttackData(IHasAttack hasAttack, AttackRangeType rangeType, 
                        AttackType attackType, AttackStat attackStat)
        {
            MyHasAttack = hasAttack;
            MyIsVictim = null;

            MyRangeType = rangeType;
            MyAttackType = attackType;
            MyAttackStat = attackStat;

            Advantage = false;
            Disadvantage = false;
            SumOfModifiers = 0;
        }

        #region Implementation

        #region Attacker and Victim

        public IHasAttack MyHasAttack { get; set; }

        public IIsVictim MyIsVictim { get; set; }

        public void SetIsVictim(IIsVictim victim)
        {
            MyIsVictim = victim;
        }

        #endregion


        #region Attack Enums

        public AttackRangeType MyRangeType { get; set; }

        public AttackType MyAttackType { get; set; }

        public AttackStat MyAttackStat { get; set; }

        #endregion


        #endregion

        #endregion






        //----------------------------------------------------------------------------
        //                    Advantage and Disadvantage
        //----------------------------------------------------------------------------

        #region Advantage and Disadvantage

        public bool Advantage { get; set; }

        public bool Disadvantage { get; set; }

        public void AddAdvantage()
        {
            Advantage = true;
        }

        public void AddDisadvantage()
        {
            Disadvantage = true;
        }


        public int RollWithAdvantage()
        {
            int rollA = Random.Range(1, 21);
            int rollB = Random.Range(1, 21);

            if (rollA > rollB)
            {
                return rollA;
            }
            return rollB;
        }

        public int RollWithDisadvantage()
        {
            int rollA = Random.Range(1, 21);
            int rollB = Random.Range(1, 21);

            if (rollA < rollB)
            {
                return rollA;
            }
            return rollB;
        }

        #endregion





        //----------------------------------------------------------------------------
        //                    Modifiers
        //----------------------------------------------------------------------------

        #region Modifiers

        public int SumOfModifiers { get; set; }

        public void AddModifier(int mod)
        {
            SumOfModifiers += mod;
        }

        #endregion





        //----------------------------------------------------------------------------
        //                    RollForAttack
        //----------------------------------------------------------------------------

        #region RollForAttack

        public IEnumerator RollForAttack()
        {
            MyAttackRoll = new AttackRoll();
            if(Advantage && !Disadvantage)
            {
                MyAttackRoll.Roll = RollWithAdvantage();
            }
            else if(Disadvantage && !Advantage)
            {
                MyAttackRoll.Roll = RollWithDisadvantage();
            }
            else
            {
                MyAttackRoll.Roll = Random.Range(1, 21);
            }

            MyAttackRoll.PureRoll = MyAttackRoll.Roll;
            MyAttackRoll.Roll += SumOfModifiers;
            yield break;
        }

        public AttackRoll MyAttackRoll { get; set; }

        #endregion

    }

}