using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Map;

//----------------------------------------------------------------------------
//             Class Description
//----------------------------------------------------------------------------
//
// There are a couple different ways to consider 'attack range'
//
// That include visibility and blindness and walls
// 
// Currently it only 'traverses the graph' of the map.
// Stopping if the straight line distance between the two hexes exceeds the range

namespace CriticalRole.Attacking
{
    public class AttackRanges
    {
        public HashSet<IHexagon> GetTargetsInRange(IHexagon start, int range)
        {
            Queue<IHexagon> hexQueue = new Queue<IHexagon>();
            HashSet<IHexagon> visited = new HashSet<IHexagon>();
            HashSet<IHexagon> targetsInRange = new HashSet<IHexagon>();


            hexQueue.Enqueue(start);
            while (hexQueue.Count != 0)
            {
                IHexagon currentHex = hexQueue.Dequeue();
                
                if (currentHex.MyHexMap.IsOccupied())
                {
                    if (currentHex.Contents.MyHasTurn != null)
                    {
                        targetsInRange.Add(currentHex);
                    }
                }

                foreach (IHexagon neighbour in currentHex.MyHexMap.Neighbours)
                {
                    if (HexMath.FindDistance(start, neighbour) <= range && !visited.Contains(neighbour))
                    {
                        hexQueue.Enqueue(neighbour);
                        visited.Add(neighbour);
                    }
                }
            }

            //algorithm would return the attacker as a target otherwise
            targetsInRange.Remove(start);

            return targetsInRange;
        }
    }


}