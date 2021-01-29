using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Attacking;
using System.Linq;

namespace CriticalRole.Damage
{
    //----------------------------------------------------------------------------
    //             Class Description
    //----------------------------------------------------------------------------

    //
    // All damage alterations go through here, registered to an attacker or 
    // a victim.
    //
    // Two dictionaries plus a list wrapper of the alterations. Alterations get given
    // attack roll data to make decisions from.
    //
    // Damage alterations are sorted based on order of operations, so the two alteration
    // lists must be concatenated before sorting.

    public interface IDamageDataAlterer
    {
        void AlterDamage(IHasAttack hasAttack, IIsVictim isVictim, IAttackData attackRoll);

        void RegisterDamageAlteration(IHasAttack hasAttack, IDamageAlteration damageAlteration);
        void RegisterDamageAlteration(IIsVictim isVictim, IDamageAlteration damageAlteration);
    }


    public class DamageDataAlterer : IDamageDataAlterer
    {

        //----------------------------------------------------------------------------
        //            AlterDamage
        //----------------------------------------------------------------------------

        #region AlterDamage

        /// <summary>
        /// Gets all alterations registered to the hasAttack and isVictim. <para/>
        /// It then concantenates them, sorts them by operation order, and applies them.
        /// </summary>
        public void AlterDamage(IHasAttack hasAttack, IIsVictim isVictim, IAttackData attackData)
        {
            _InitialiseDamageAlterationsEnumerable();

            _GetHasAttackAlterations(hasAttack);

            _GetIsVictimAlterations(isVictim);

            _SortAlterations();

            _ApplyAlterations(attackData);
        }

        #region Implementation

        private IEnumerable<IDamageAlteration> _DamageAlterationsEnumerable;

        private List<IDamageAlteration> _DamageAlterations;

        private void _InitialiseDamageAlterationsEnumerable()
        {
            _DamageAlterationsEnumerable = new List<IDamageAlteration>();
        }

        private void _GetHasAttackAlterations(IHasAttack hasAttack)
        {
            if (HasAttackAlterationDict.ContainsKey(hasAttack))
            {
                _DamageAlterationsEnumerable = _DamageAlterationsEnumerable.Concat(HasAttackAlterationDict[hasAttack]);
            }
        }

        private void _GetIsVictimAlterations(IIsVictim isVictim)
        {
            if (IsVictimAlterationDict.ContainsKey(isVictim))
            {
                _DamageAlterationsEnumerable = _DamageAlterationsEnumerable.Concat(IsVictimAlterationDict[isVictim]);
            }
        }

        private void _SortAlterations()
        {
            _DamageAlterations = _DamageAlterationsEnumerable.ToList();
            _DamageAlterations.Sort(new DamageAlterationComparable());
        }

        private void _ApplyAlterations(IAttackData attackData)
        {
            foreach (IDamageAlteration damageAlteration in _DamageAlterations)
            {
                damageAlteration.Alter(attackData);
            }
        }

        #endregion


        #endregion






        //----------------------------------------------------------------------------
        //            RegisterDamageAlteration
        //----------------------------------------------------------------------------

        #region Register Damage Alteration

        #region HasAttack Alterations

        /// <summary>
        /// Register a damage alteration to a character's attack
        /// </summary>
        public void RegisterDamageAlteration(IHasAttack hasAttack, IDamageAlteration damageAlteration)
        {
            _CheckDictInit(hasAttack);

            HasAttackAlterationDict[hasAttack].Add(damageAlteration);
        }


        #region Implementation

        private Dictionary<IHasAttack, List<IDamageAlteration>> _HasAttackAlterationDict;
        public Dictionary<IHasAttack, List<IDamageAlteration>> HasAttackAlterationDict
        {
            get
            {
                if(_HasAttackAlterationDict == null)
                {
                    _HasAttackAlterationDict = new Dictionary<IHasAttack, List<IDamageAlteration>>();
                }
                return _HasAttackAlterationDict;
            }
        }

        /// <summary>
        /// Make sure dictionary entry exists. <para/>
        /// If it doesn't, initialise it.
        /// </summary>
        private void _CheckDictInit(IHasAttack hasAttack)
        {
            if (!HasAttackAlterationDict.ContainsKey(hasAttack))
            {
                HasAttackAlterationDict[hasAttack] = new List<IDamageAlteration>();
            }
        }

        #endregion


        #endregion




        #region IsVictim Alterations


        /// <summary>
        /// Register a damage alteration for when a character gets attacked.
        /// </summary>
        public void RegisterDamageAlteration(IIsVictim isVictim, IDamageAlteration damageAlteration)
        {
            _CheckDictInit(isVictim);

            IsVictimAlterationDict[isVictim].Add(damageAlteration);
        }

        #region Implementation

        private Dictionary<IIsVictim, List<IDamageAlteration>> _IsVictimAlterationDict;
        public Dictionary<IIsVictim, List<IDamageAlteration>> IsVictimAlterationDict
        {
            get
            {
                if(_IsVictimAlterationDict == null)
                {
                    _IsVictimAlterationDict = new Dictionary<IIsVictim, List<IDamageAlteration>>();
                }
                return _IsVictimAlterationDict;
            }
        }

        /// <summary>
        /// Make sure dictionary entry exists. <para/>
        /// If it doesn't, initialise it.
        /// </summary>
        private void _CheckDictInit(IIsVictim isVictim)
        {
            if (!IsVictimAlterationDict.ContainsKey(isVictim))
            {
                IsVictimAlterationDict[isVictim] = new List<IDamageAlteration>();
            }
        }

        #endregion

        #endregion



        #endregion


    }
}


