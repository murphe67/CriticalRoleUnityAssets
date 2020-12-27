using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class hex
    {
        [Test]
        public void add_neighbours()
        {
            //ARRANGE
            GameObject gameObject = new GameObject("base");
            Hexagon hex = gameObject.AddComponent<Hexagon>();

            List<IHexagon> neighbours = new List<IHexagon>();
            for(int i = 0; i < 6; i++)
            {
                neighbours.Add(Substitute.For<IHexagon>());
            }

            //ACT
            hex.AddNeighbours(neighbours);

            //ASSERT
            Assert.AreEqual(hex.Neighbours, neighbours);
        }

        [Test]
        public void create_hex()
        {
            //ARRANGE
            GameObject gameObject = new GameObject("base");
            Vector3Int coords = new Vector3Int(0, 1, -1);

            //ACT
            Hexagon hex = gameObject.AddComponent<Hexagon>();
            hex.Initialise(coords, 1);

            //ASSERT
            Assert.AreEqual(hex.CoOrds, coords);
        }
    }
}
