using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Turns;

namespace CriticalRole.Attacking
{
    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------
    //
    // Class for the attack manager to get all of the effects that alter 
    // an attack roll
    //
    // Wrapper classes for 2 dictionaries, each containing lists of
    // AttackDataAlterations as their values.
    //
    // At high level, there could be a huge amount of attack alterations on a
    // battlefield.
    //
    // It would be very easy to add code to the alteration to only alter
    // appropriate attacks, as they know everything about the attack
    //
    // However I'm worried checking every alteration every attack would be slow
    //
    // So instead an alteration has to tie itself to an attacker making an attack
    // or to a victim being attacked.
    //
    // Attack alterations form a solid like 
    // 20%? of the rules of this game
    // and this systems could expand to include a lot of different effects
    // or the attack manager could possibly have a second class seperate to this
    // 
    // Leaving this one to specifically focus on attack roll alterations


    public interface IAttackDataAlterer
    {
        void Alter(IAttackData attackData);

        void AddAttackAlteration(IHasAttack hasAttack, IAttackDataAlteration attackDataAlteration);


        void AddAttackAlteration(IIsVictim isVictim, IAttackDataAlteration attackDataAlteration);
    }



    public class AttackDataAlterer : IAttackDataAlterer
    {


        private Dictionary<IHasAttack, List<IAttackDataAlteration>> _HasAttackAlterationsDict;
        public Dictionary<IHasAttack, List<IAttackDataAlteration>> HasAttackAlterationsDict
        {
            get
            {
                if(_HasAttackAlterationsDict == null)
                {
                    _HasAttackAlterationsDict = new Dictionary<IHasAttack, List<IAttackDataAlteration>>();
                }
                return _HasAttackAlterationsDict;
            }
        }

        private Dictionary<IIsVictim, List<IAttackDataAlteration>> _IsVictimAlterationsDict;
        public Dictionary<IIsVictim, List<IAttackDataAlteration>> IsVictimAlterationsDict
        {
            get
            {
                if(_IsVictimAlterationsDict == null)
                {
                    _IsVictimAlterationsDict = new Dictionary<IIsVictim, List<IAttackDataAlteration>>();
                }
                return _IsVictimAlterationsDict;
            }
        }

  





        //----------------------------------------------------------------------------
        //                    Alter
        //----------------------------------------------------------------------------

        #region Alter

        public void Alter(IAttackData attackData)
        {
            if(HasAttackAlterationsDict.ContainsKey(attackData.MyHasAttack))
            {
                foreach(IAttackDataAlteration attackDataAlteration in HasAttackAlterationsDict[attackData.MyHasAttack])
                {
                    attackDataAlteration.Alter(attackData);
                }
                
            }
            
            if(IsVictimAlterationsDict.ContainsKey(attackData.MyIsVictim))
            {
                foreach (IAttackDataAlteration attackDataAlteration in IsVictimAlterationsDict[attackData.MyIsVictim])
                {
                    attackDataAlteration.Alter(attackData);
                }
            }
        }

        #endregion





        //----------------------------------------------------------------------------
        //                   Add Alterations
        //----------------------------------------------------------------------------

        #region Add Alterations

        // Attack alterations have to specify who they apply to
        // Trade off: you have to add an alteration for each character
        // so you have quicker implementation but more memory is used
        //
        // Honestly I don't think either would impact performance, but
        // it might be a good idea to add a mix of both

        public void AddAttackAlteration(IHasAttack hasAttack, IAttackDataAlteration attackDataAlteration)
        {
            if(!HasAttackAlterationsDict.ContainsKey(hasAttack))
            {
                HasAttackAlterationsDict[hasAttack] = new List<IAttackDataAlteration>();
            }
            HasAttackAlterationsDict[hasAttack].Add(attackDataAlteration);
        }

        public void AddAttackAlteration(IIsVictim isVictim, IAttackDataAlteration attackDataAlteration)
        {
            if (!IsVictimAlterationsDict.ContainsKey(isVictim))
            {
                IsVictimAlterationsDict[isVictim] = new List<IAttackDataAlteration>();
            }

            IsVictimAlterationsDict[isVictim].Add(attackDataAlteration);
        }

        #endregion
    }



}