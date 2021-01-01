using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class path_value
    {
        [Test]
        public void path_value_equals()
        {
            GameObject gameObject = new GameObject();
            IHexagon hex1 = Substitute.For<IHexagon>();
            Vector3Int coords1 = new Vector3Int(1, -1, 0);
            hex1.MyHexMap.CoOrds.Returns(coords1);
            hex1.HexTransform.Returns(gameObject.transform);

            IHexagon hex2 = Substitute.For<IHexagon>();
            Vector3Int coords2 = new Vector3Int(1, -1, 0);
            hex2.MyHexMap.CoOrds.Returns(coords2);
            hex2.HexTransform.Returns(gameObject.transform);

            Vector3 centre = new Vector3(0, 0, 0);
            PathValue path1 = new PathValue(4, 4, centre, hex1, null);
            PathValue path2 = new PathValue(4, 4, centre, hex2, null);

            
            Assert.AreEqual(path1.CompareTo(path2), 0);
        }

        
        [Test]
        public void path_value_id_is_more_than()
        {
            GameObject gameObject = new GameObject();
            IHexagon hex1 = Substitute.For<IHexagon>();
            Vector3Int coords1 = new Vector3Int(1, -1, 0);
            hex1.MyHexMap.CoOrds.Returns(coords1);
            hex1.HexTransform.Returns(gameObject.transform);

            IHexagon hex2 = Substitute.For<IHexagon>();
            Vector3Int coords2 = new Vector3Int(0, 1, -1);
            hex2.MyHexMap.CoOrds.Returns(coords2);
            hex2.HexTransform.Returns(gameObject.transform);

            Vector3 centre = new Vector3(0, 0, 0);
            PathValue path1 = new PathValue(4, 4, centre, hex1, null);
            PathValue path2 = new PathValue(4, 4, centre, hex2, null);


            Assert.AreEqual(path1.CompareTo(path2), 1);
        }

        
        [Test]
        public void path_value_id_is_less_than()
        {
            GameObject gameObject = new GameObject();
            IHexagon hex1 = Substitute.For<IHexagon>();
            Vector3Int coords1 = new Vector3Int(0, -1, -1);
            hex1.MyHexMap.CoOrds.Returns(coords1);
            hex1.HexTransform.Returns(gameObject.transform);

            IHexagon hex2 = Substitute.For<IHexagon>();
            Vector3Int coords2 = new Vector3Int(1, 0, -1);
            hex2.MyHexMap.CoOrds.Returns(coords2);
            hex2.HexTransform.Returns(gameObject.transform);

            Vector3 centre = new Vector3(0, 0, 0);
            PathValue path1 = new PathValue(4, 4, centre, hex1, null);
            PathValue path2 = new PathValue(4, 4, centre, hex2, null);


            Assert.AreEqual(path1.CompareTo(path2), -1);
        }

        [Test]
        public void path_value_f_is_more_than()
        {
            GameObject gameObject = new GameObject();
            IHexagon hex1 = Substitute.For<IHexagon>();
            Vector3Int coords1 = new Vector3Int(0, -1, -1);
            hex1.MyHexMap.CoOrds.Returns(coords1);
            hex1.HexTransform.Returns(gameObject.transform);

            IHexagon hex2 = Substitute.For<IHexagon>();
            Vector3Int coords2 = new Vector3Int(1, 0, -1);
            hex2.MyHexMap.CoOrds.Returns(coords2);
            hex2.HexTransform.Returns(gameObject.transform);


            Vector3 centre = new Vector3(0, 0, 0);
            PathValue path1 = new PathValue(4, 5, centre, hex1, null);
            PathValue path2 = new PathValue(4, 4, centre, hex2, null);


            Assert.AreEqual(path1.CompareTo(path2), 1);
        }

        [Test]
        public void path_value_f_is_less_than()
        {
            GameObject gameObject = new GameObject();
            IHexagon hex1 = Substitute.For<IHexagon>();
            Vector3Int coords1 = new Vector3Int(0, -1, -1);
            hex1.MyHexMap.CoOrds.Returns(coords1);
            hex1.HexTransform.Returns(gameObject.transform);

            IHexagon hex2 = Substitute.For<IHexagon>();
            Vector3Int coords2 = new Vector3Int(1, 0, -1);
            hex2.MyHexMap.CoOrds.Returns(coords2);
            hex2.HexTransform.Returns(gameObject.transform);


            Vector3 centre = new Vector3(0, 0, 0);
            PathValue path1 = new PathValue(4, 3, centre, hex1, null);
            PathValue path2 = new PathValue(4, 4, centre,  hex2, null);


            Assert.AreEqual(path1.CompareTo(path2), -1);
        }
    }
}