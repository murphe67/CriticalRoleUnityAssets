using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Armored;

namespace CriticalRole.Character
{
    public class ClericClass : MonoBehaviour, IClassAbilities
    {
        public void Initialise()
        {
            IHasStats hasStats = GetComponent<IHasStats>();
            hasStats.AddSimpleWeaponsProficiency();
            hasStats.AddArmorProficiency(ArmorType.Light);
            hasStats.AddArmorProficiency(ArmorType.Medium);
            hasStats.AddArmorProficiency(ArmorType.Shield);
        }
    }

}

