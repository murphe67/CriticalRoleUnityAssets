using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class turn_sort
    {
        [Test]
        public void turn_sort_initiative_sort()
        {
            List<ITurnSort> TurnList = new List<ITurnSort>();
            List<ITurnSort> ExpectedList = new List<ITurnSort>();

            ITurnSort[] TurnArray = new ITurnSort[7];


            for (int i = 6; i >= 0; i--)
            {
                IHasTurn hasTurn = Substitute.For<IHasTurn>();
                ITurnSort turnSort = new BaseTurnSort(i, 0, new Vector3Int(0, 0, 0), hasTurn);
                ExpectedList.Add(turnSort);
                TurnArray[i] = turnSort;
            }

            for (int i = 0; i < 3; i++)
            {
                TurnList.Add(TurnArray[i]);
            }
            for (int i = 6; i >= 3; i--)
            {
                TurnList.Add(TurnArray[i]);
            }
            TurnList.Sort();

            Assert.AreEqual(ExpectedList, TurnList);
        }

        [Test]
        public void turn_sort_dexterity_sort()
        {
            List<ITurnSort> TurnList = new List<ITurnSort>();
            List<ITurnSort> ExpectedList = new List<ITurnSort>();

            ITurnSort[] TurnArray = new ITurnSort[7];


            for (int i = 6; i >= 0; i--)
            {
                IHasTurn hasTurn = Substitute.For<IHasTurn>();
                ITurnSort turnSort = new BaseTurnSort(0, i, new Vector3Int(0, 0, 0), hasTurn);
                ExpectedList.Add(turnSort);
                TurnArray[i] = turnSort;
            }

            for (int i = 0; i < 3; i++)
            {
                TurnList.Add(TurnArray[i]);
            }
            for (int i = 6; i >= 3; i--)
            {
                TurnList.Add(TurnArray[i]);
            }
            TurnList.Sort();

            Assert.AreEqual(ExpectedList, TurnList);
        }

        [Test]
        public void turn_sort_location_sort()
        {
            List<ITurnSort> TurnList = new List<ITurnSort>();
            List<ITurnSort> ExpectedList = new List<ITurnSort>();

            ITurnSort[] TurnArray = new ITurnSort[2];


            for (int i = 1; i >= 0; i--)
            {
                IHasTurn hasTurn = Substitute.For<IHasTurn>();
                ITurnSort turnSort = new BaseTurnSort(0, 0, new Vector3Int(i, 0, -i), hasTurn);
                ExpectedList.Add(turnSort);
                TurnArray[i] = turnSort;
            }

            for (int i = 0; i <= 1; i++)
            {
                TurnList.Add(TurnArray[i]);
            }

            TurnList.Sort();

            Assert.AreEqual(ExpectedList, TurnList);
        }
    }
}