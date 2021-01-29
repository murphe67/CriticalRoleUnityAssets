using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Attacking;

namespace CriticalRole.Damage
{

    //----------------------------------------------------------------------------
    //            DamageAlterationType (Enum)
    //---------------------------------------------------------------------------

    #region DamageAlterationType (Enum)

    /// <summary>
    /// Order of operations for IDamageAlterations
    /// </summary>
    public enum DamageAlterationType
    {
        Addition,
        Multiplication
    }

    #endregion




    //----------------------------------------------------------------------------
    //            IDamageAlteration
    //---------------------------------------------------------------------------

    #region IDamageAlteration

    public interface IDamageAlteration
    {
        void Alter(IAttackData attackData);
        DamageAlterationType MyDamageAlterationType { get; }
    }

    #endregion





    //----------------------------------------------------------------------------
    //            DamageAlterationComparable
    //---------------------------------------------------------------------------

    #region DamageAlterationComparable

    /// <summary>
    /// Sorts IDamageAlteration by operation order
    /// </summary>
    public class DamageAlterationComparable : IComparer<IDamageAlteration>
    {
        public int Compare(IDamageAlteration x, IDamageAlteration y)
        {
            if(x.MyDamageAlterationType > y.MyDamageAlterationType)
            {
                return 1;
            }
            else if(x.MyDamageAlterationType < y.MyDamageAlterationType)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    #endregion

}

