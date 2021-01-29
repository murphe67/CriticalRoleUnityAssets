using System;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Character;

namespace CriticalRole.Turns
{

    //----------------------------------------------------------------------------
    //              Class Description
    //----------------------------------------------------------------------------
    //
    // Any IHasTurn should generate their own ITurnSort when called to by the
    // TurnController. The TurnController adds it to the list.
    //
    // Once it has all the IHasTurn's, it sorts the list based on their TurnSort, 
    // giving the initiative order.
    //
    // The only thing accessible to the TurnController is the IHasTurn that generated it


    /// <summary>
    /// Generic initiative order sorting: <para/>
    /// First highest initiative wins, then on matching initative, highest dexterity wins <para/>
    /// In the very rare occasion both are the same, its done based on hex coords <para/>
    /// The hex coords ordering is also pretty random 
    /// </summary>
    public class TurnSort : IComparer<IHasTurn>
    { 
        public int Compare(IHasTurn x, IHasTurn y)
        {
            if (x.Initiative.RollValue < y.Initiative.RollValue)
            {
                return 1;
            }
            else if (x.Initiative.RollValue > y.Initiative.RollValue)
            {
                return -1;
            }
            else if (x.MyHasStats.GetStatMod(StatsType.Dexterity) < y.MyHasStats.GetStatMod(StatsType.Dexterity))
            {
                return 1;
            }
            else if (x.MyHasStats.GetStatMod(StatsType.Dexterity) > y.MyHasStats.GetStatMod(StatsType.Dexterity))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

    }
}