using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class map_ranges
    {
        [Test]
        public void find_straight_distance()
        {
            GameObject gameObject = new GameObject();
            MapGeneration mapGeneration = gameObject.AddComponent<MapGeneration>();
            mapGeneration.HexagonPrefab = new GameObject();
            mapGeneration.MapRadius = 10;

            mapGeneration.HexDict = new Dictionary<Vector3Int, IHexagon>();
            mapGeneration.GenerateMap();
            mapGeneration.UpdateHexNeighbours();

            IHexagon start_hex = mapGeneration.HexDict[new Vector3Int(0, 0, 0)];
            IHexagon end_hex = mapGeneration.HexDict[new Vector3Int(0, -4, 4)];

            
            int distance = 4;

            Assert.AreEqual(distance, HexMath.FindDistance(start_hex, end_hex));
        }
    }
}
