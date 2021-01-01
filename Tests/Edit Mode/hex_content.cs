using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class hex_content
    {
        [Test]
        public void hex_content_moved_to_hex_at_startup()
        {
            GameObject gameObject = new GameObject();
            MapGeneration mapGeneration = gameObject.AddComponent<MapGeneration>();

            GameObject character = new GameObject();
            character.transform.position = new Vector3(5, 0, 3);
            character.AddComponent<IContentMarker>();
            BaseHexContent moveableCharacter = character.AddComponent<BaseHexContent>();

            mapGeneration.MapRadius = 10;
            mapGeneration.HexagonPrefab = new GameObject();
            mapGeneration.HexDict = new Dictionary<Vector3Int, IHexagon>();
            mapGeneration.GenerateMap();
            mapGeneration.UpdateHexNeighbours();
            mapGeneration.ConnectContentsToHex();

            Assert.AreEqual(new Vector3(5.25f, 0, 3.031f), character.transform.position); 
        }

        [Test]
        public void hex_content_links_to_hex()
        {
            GameObject gameObject = new GameObject();
            MapGeneration mapGeneration = gameObject.AddComponent<MapGeneration>();

            GameObject character = new GameObject();
            character.transform.position = new Vector3(5, 0, 3);
            character.AddComponent<IContentMarker>();
            BaseHexContent moveableCharacter = character.AddComponent<BaseHexContent>();

            mapGeneration.MapRadius = 10;
            mapGeneration.HexagonPrefab = new GameObject();
            mapGeneration.HexDict = new Dictionary<Vector3Int, IHexagon>();
            mapGeneration.GenerateMap();
            mapGeneration.UpdateHexNeighbours();
            mapGeneration.ConnectContentsToHex();

            Assert.AreEqual(mapGeneration.HexDict[new Vector3Int(7, 0, -7)], moveableCharacter.Location);
        }
    }
}

