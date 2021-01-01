using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRanges
{
    public HashSet<IHexagon> GetTargetsInRange(IHexagon start, int range)
    {
        Queue<IHexagon> hexQueue = new Queue<IHexagon>();
        HashSet<IHexagon> visited = new HashSet<IHexagon>();
        HashSet<IHexagon> targetsInRange = new HashSet<IHexagon>();

        
        hexQueue.Enqueue(start);
        int i = 0;
        while(hexQueue.Count != 0 && i < 10)
        {
            i++;
            IHexagon currentHex = hexQueue.Dequeue();
            if(currentHex.MyHexMap.IsOccupied())
            {
                targetsInRange.Add(currentHex);
            }

            foreach(IHexagon neighbour in currentHex.MyHexMap.Neighbours)
            {
                if(HexMath.FindDistance(start, neighbour) <= range && !visited.Contains(neighbour))
                {
                    hexQueue.Enqueue(neighbour);
                    visited.Add(neighbour);
                }
            }
        }

        targetsInRange.Remove(start);
        return targetsInRange;
    }
}
