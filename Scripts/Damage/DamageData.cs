using System;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Damage
{

    //----------------------------------------------------------------------------
    //             Damage Enums
    //----------------------------------------------------------------------------
    #region Damage Enums

    public enum DamageSource
    {
        Roll,
        Mod
    }

    public enum DamageType
    {
        Acid,
        Bludgeoning,
        Cold,
        Fire,
        Force,
        Lightning,
        Nectoric,
        Piercing,
        Poison,
        Psychic,
        Radiant,
        Slashing,
        Thunder
    }

    #endregion



    //----------------------------------------------------------------------------
    //             Class Description
    //----------------------------------------------------------------------------
    //
    // This system is a lot more complicated than other numerical alterations
    // Damage in 5e gets multiplied based off of DamageType and DamageSource
    // and needs to be quickly accessible based off of the combinations of both 
    // type and source.
    //
    // DamageData therefore keeps the numerical value of the damage in a dictionary
    // with the key as a SingleDamageID, so that damage alterations can mess with 
    // the values properly.
    //
    // Theyre stored as floats so that division can be undone without rounding errors.
    // The damage is converted to a (floored) int on access.

    public class DamageData
    {
        //----------------------------------------------------------------------------
        //             Constructor
        //----------------------------------------------------------------------------

        #region Constructor
        public DamageData()
        {
            MyDamageDict = new Dictionary<SingleDamageID, float>(new SingleDamageIDComparer());
        }

        public Dictionary<SingleDamageID, float> MyDamageDict;

        #endregion


        //----------------------------------------------------------------------------
        //             AddDamage
        //----------------------------------------------------------------------------

        #region AddDamage
        /// <summary>
        /// Wrapper function for adding an element to the damage dictionary
        /// </summary>
        public void AddDamage(float value, DamageType damageType, DamageSource damageSource)
        {
            SingleDamageID damageID = new SingleDamageID(damageType, damageSource);


            if(!MyDamageDict.ContainsKey(damageID))
            {
                MyDamageDict[damageID] = value;
            }
            else
            {
                MyDamageDict[damageID] += value;
            }
        }


        #endregion




        //----------------------------------------------------------------------------
        //             GetDamage
        //---------------------------------------------------------------------------

        #region GetDamage

        public int GetDamage()
        {
            float damage = 0;

            foreach(KeyValuePair<SingleDamageID, float> keyValuePair in MyDamageDict)
            {
                damage += keyValuePair.Value;
            }

            return (int)damage;
        }

        #endregion
    
    
    }



}

