using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Attacking
{
    public class ToHitBonus : IAttackDataAlteration
    {
        int Bonus;
        public ToHitBonus(int bonus)
        {
            Bonus = bonus;
        }

        public void Alter(IAttackData attackData)
        {
            attackData.AddModifier(Bonus);
        }
    }

}

