using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Map
{

    //----------------------------------------------------------------------------
    //              Class Description
    //----------------------------------------------------------------------------

    // Container class for path information
    // Allowing AI to make decisions after a pathfinding call
    //
    // Is a class instead of a struct due to parameterless constructor

    public class MapPath
    {
        public MapPath()
        {
            PathStack = new Stack<IHexagon>();
            HasOpportunityAttack = false;
            cost = 0;
        }
        public Stack<IHexagon> PathStack;
        public bool HasOpportunityAttack;
        public int cost;
    }

}
