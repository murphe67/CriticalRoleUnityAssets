using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//----------------------------------------------------------------------------
//             ClassDescription
//----------------------------------------------------------------------------
//
// Armour scriptable objects for hardcoding their behaviour
//
// Still unsure how features are going to work but I think it will be
// an entirely seperate system. 
//
// So that all features from random sources are handled the same way.

namespace CriticalRole.Armored
{
    public enum ArmorType
    {
        Light, 
        Medium,
        Heavy,
        Shield
    }

    [CreateAssetMenu(fileName = "New Armor", menuName = "ScriptableObjects/ArmorObject", order = 1)]
    public class ArmorObject : ScriptableObject
    {
        public ArmorType MyArmorType;
        public int BaseAC;
        public int MinStrength;
        public bool StealthDisadvantage;
        public bool HasMaxDexMod;
        public int MaxDexMod;
    }
}

