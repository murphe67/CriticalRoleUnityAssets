using System;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Damage
{

    //----------------------------------------------------------------------------
    //            Class Description
    //---------------------------------------------------------------------------
    //
    // In order to alter damage based off of DamageSource and DamageType, you must be 
    // able to index into the dictionary. This provides a way to evaluate
    // whether the key matches.

    public class SingleDamageID
    {
        //----------------------------------------------------------------------------
        //            Constructor
        //---------------------------------------------------------------------------

        #region Constructor

        public SingleDamageID(DamageType damageType, DamageSource damageSource)
        {
            MyDamageType = damageType;
            MyDamageSource = damageSource;
        }

        public DamageType MyDamageType;
        public DamageSource MyDamageSource;

        #endregion
    
    }

    public class SingleDamageIDComparer : IEqualityComparer<SingleDamageID>
    {
        public bool Equals(SingleDamageID x, SingleDamageID y)
        {
                if (x.MyDamageSource == y.MyDamageSource)
                {
                    if (x.MyDamageType == y.MyDamageType)
                    {
                        return true;
                    }
                }
                return false;
        }

        public int GetHashCode(SingleDamageID obj)
        {
            int hash1 = obj.MyDamageSource.GetHashCode();
            int hash2 = obj.MyDamageType.GetHashCode();
            return (hash1 ^ hash2);
        }

        
    }

}
