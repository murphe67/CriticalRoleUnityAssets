using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Damage;

namespace CriticalRole.Attacking
{
    public class AttackRoll
    {
        public AttackRoll()
        {
            Roll = 0;
            DidHit = false;
            MyDamageData = new DamageData();
        }

        public int PureRoll;
        public int Roll;
        public bool DidHit;
        public DamageData MyDamageData;
    }
}
