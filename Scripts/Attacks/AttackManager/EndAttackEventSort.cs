using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Attacking
{
    /// <summary>
    /// Sorts the EndAttacks by enum declaration order
    /// </summary>
    public class EndAttackEventSort : IComparer<IEndAttackEvent>
    {
        public int Compare(IEndAttackEvent x, IEndAttackEvent y)
        {
            if(x.MyEndAttackEventType > y.MyEndAttackEventType)
            {
                return 1;
            }
            else if(x.MyEndAttackEventType < y.MyEndAttackEventType)
            {
                return -1;
            }
            return 0;
        }
    }

}
