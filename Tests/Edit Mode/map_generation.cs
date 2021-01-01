using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class map_generation
    {
        [Test]
        public void initial_generation()
        {
            //ARRANGE
            GameObject hexGO = new GameObject();
            GameObject gameObject = new GameObject();
            MapGeneration myMapGeneration = gameObject.AddComponent<MapGeneration>();
            myMapGeneration.HexagonPrefab = hexGO;
            myMapGeneration.MapRadius = 10;


            myMapGeneration.HexDict = new Dictionary<Vector3Int, IHexagon>();

            //ACT
            myMapGeneration.GenerateMap();

            //ASSERT
            Assert.AreEqual(331, myMapGeneration.HexDict.Count);
        }

        [Test]
        public void get_directions()
        {
            List<Vector3Int> directions = new List<Vector3Int>();
            directions.Add(new Vector3Int(1, -1, 0));
            directions.Add(new Vector3Int(1, 0, -1));
            directions.Add(new Vector3Int(0, 1, -1));
            directions.Add(new Vector3Int(-1, 1, 0));
            directions.Add(new Vector3Int(-1, 0, 1));
            directions.Add(new Vector3Int(0, -1, 1));

            GameObject gameObject = new GameObject();
            MapGeneration myMapGeneration = gameObject.AddComponent<MapGeneration>();
            Assert.AreEqual(directions, myMapGeneration.GetDirections());
        }

        [Test]
        public void update_neighbours()
        {
            //ARRANGE
            GameObject hexGO = new GameObject();
            GameObject gameObject = new GameObject();
            MapGeneration mapGeneration = gameObject.AddComponent<MapGeneration>();
            mapGeneration.HexagonPrefab = hexGO;
            mapGeneration.MapRadius = 10;

            //generate map
            mapGeneration.HexDict = new Dictionary<Vector3Int, IHexagon>();
            mapGeneration.GenerateMap();
            mapGeneration.UpdateHexNeighbours();
            mapGeneration.ConnectContentsToHex();

            //get random hex
            Vector3Int coords = new Vector3Int(-3, 2, 1);
            IHexagon hex = (IHexagon)mapGeneration.HexDict[coords];

            //manually calculate its neighbours
            List<Vector3Int> directions = mapGeneration.GetDirections();
            List<IHexagon> neighbours = new List<IHexagon>();
            foreach(Vector3Int direction in directions)
            {
                neighbours.Add(mapGeneration.HexDict[coords + direction]);
            }

            //ASSERT
            Assert.AreEqual(neighbours, hex.MyHexMap.Neighbours);
        }
    }
}