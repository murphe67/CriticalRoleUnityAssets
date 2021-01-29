using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Turns;

namespace CriticalRole.Rolling
{
    public class RollDictID
    {
        public RollDictID(IHasTurn hasTurn, RollType rollType)
        {
            MyHasTurn = hasTurn;
            MyRollType = rollType;
        }

        public IHasTurn MyHasTurn;
        public RollType MyRollType;
    }

    public class RollDictIDComparer : IEqualityComparer<RollDictID>
    {
        public bool Equals(RollDictID x, RollDictID y)
        {
            if(x.MyRollType == y.MyRollType)
            {
                if(x.MyHasTurn == y.MyHasTurn)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetHashCode(RollDictID obj)
        {
            int hash1 = obj.MyRollType.GetHashCode();
            int hash2 = obj.MyHasTurn.GetHashCode();
            return (hash1 ^ hash2);
        }
    }

}
