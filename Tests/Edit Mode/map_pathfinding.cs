using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class map_pathfinding
    {
        [Test]
        public void pathfinding()
        {
            GameObject gameObject = new GameObject();
            MapGeneration mapGeneration = gameObject.AddComponent<MapGeneration>();
            mapGeneration.HexagonPrefab = new GameObject();
            mapGeneration.MapRadius = 10;

            mapGeneration.Initialise();

            IHexagon start_hex = mapGeneration.HexDict[new Vector3Int(0, 0, 0)];
            IHexagon end_hex = mapGeneration.HexDict[new Vector3Int(0, -4, 4)];

            MapPathfinding mapPathfinding = new MapPathfinding();
            MapPath path = mapPathfinding.GeneratePath(start_hex, end_hex);

            MapPath arbitrary_path = new MapPath();
            for (int i = 4; i >= 0; i--)
            {
                arbitrary_path.PathStack.Push(mapGeneration.HexDict[new Vector3Int(0, -i, i)]);
            }
            arbitrary_path.cost = 4;

            Assert.AreEqual(arbitrary_path.PathStack, path.PathStack);
            Assert.AreEqual(arbitrary_path.cost, path.cost);
        }

        [Test]
        public void hexes_in_range()
        {
            GameObject gameObject = new GameObject();
            MapGeneration mapGeneration = gameObject.AddComponent<MapGeneration>();
            mapGeneration.HexagonPrefab = new GameObject();
            mapGeneration.MapRadius = 10;

            mapGeneration.Initialise();

            IHexagon startHex = mapGeneration.HexDict[new Vector3Int(0, 0, 0)];

            MapPathfinding mapPathfinding = new MapPathfinding();
            List<IHexagon> hexesInRange = mapPathfinding.GetHexesInRange(startHex, 1);

            List<IHexagon> neighboursAndStart = startHex.Neighbours;
            neighboursAndStart.Add(startHex);
            foreach(IHexagon neighbour in startHex.Neighbours)
            {
                Assert.IsTrue(hexesInRange.Contains(neighbour));
            }
            foreach (IHexagon hex in hexesInRange)
            {
                Assert.IsTrue(neighboursAndStart.Contains(hex));
            }

        }
    }
}
