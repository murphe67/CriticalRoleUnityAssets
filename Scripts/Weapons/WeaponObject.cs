using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Attacking;
using CriticalRole.Damage;
using CriticalRole.Character;

namespace CriticalRole.Weapons
{
    [CreateAssetMenu(fileName = "WeaponObject", menuName = "ScriptableObjects/WeaponObject", order = 1)]
    public class WeaponObject : ScriptableObject
    {
        public WeaponType MyWeaponType;

        public int DamageDie;
        public int NumDamageDie;

        public int RollDamage()
        {
            int damage = 0;
            for (int i = 0; i < NumDamageDie; i++)
            {
                damage += Random.Range(1, DamageDie + 1);
            }
            
            return damage;
        }
        
        public AttackRangeType MyAttackRangeType;
        public DamageType MyDamageType;
        public bool Finesse;
    }
}

