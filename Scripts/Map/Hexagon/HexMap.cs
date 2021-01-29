using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Map
{

    public interface IHexMap
    {

        /// <summary>
        /// Location in HexCoords
        /// </summary>
        Vector3Int CoOrds { get; }

        /// <summary>
        /// Movement cost in units of 5ft
        /// </summary>
        int MovementCost { get; }

        HashSet<IHexagon> Neighbours { get; }

        bool IsOccupied();
    }

    public interface IHexMapRestricted : IHexMap
    {
        void SetCoords(Vector3Int coords);
        void SetMovementCost(int movementCost);
        void SetNeighbours(HashSet<IHexagon> neighbours);
    }


    public class HexMap : IHexMapRestricted
    {
        public HexMap(IHexagon hexagon)
        {
            MyHexagon = hexagon;
        }

        public IHexagon MyHexagon;

        public Vector3Int CoOrds { get; set; }

        public int MovementCost { get; set; }

        public HashSet<IHexagon> Neighbours { get; set; }

        public bool IsOccupied()
        {
            if (MyHexagon.Contents != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetCoords(Vector3Int coords)
        {
            CoOrds = coords;
        }

        public void SetMovementCost(int movementCost)
        {
            MovementCost = movementCost;
        }

        public void SetNeighbours(HashSet<IHexagon> neighbours)
        {
            Neighbours = neighbours;
        }
    }

}
