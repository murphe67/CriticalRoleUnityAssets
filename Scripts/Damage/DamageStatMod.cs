using System.Collections;
using System.Collections.Generic;
using CriticalRole.Attacking;
using UnityEngine;
using CriticalRole.Weapons;

namespace CriticalRole.Damage
{
    public class DamageStatMod : IDamageAlteration
    {
        public DamageAlterationType MyDamageAlterationType
        {
            get
            {
                return DamageAlterationType.Addition;
            }
        }

        public DamageStatMod(int mod)
        {
            Mod = mod;
        }

        public int Mod;

        public void Alter(IAttackData attackData)
        {
            WeaponObject weapon = attackData.MyHasAttack.MyWeapon;
            attackData.MyAttackRoll.MyDamageData.AddDamage(Mod, weapon.MyDamageType, DamageSource.Mod);
        }
    }

}
