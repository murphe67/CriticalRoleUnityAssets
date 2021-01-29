using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using CriticalRole.Turns;

namespace Tests
{
    /*
    public class turn_sort
    {
        [Test]
        public void turn_sort_initiative_sort()
        {
            List<TurnSort> TurnList = new List<TurnSort>();
            List<TurnSort> ExpectedList = new List<TurnSort>();

            TurnSort[] TurnArray = new TurnSort[7];


            for (int i = 6; i >= 0; i--)
            {
                IHasTurn hasTurn = Substitute.For<IHasTurn>();
                TurnSort turnSort = new TurnSort(i, 0, new Vector3Int(0, 0, 0), hasTurn);
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
            List<TurnSort> TurnList = new List<TurnSort>();
            List<TurnSort> ExpectedList = new List<TurnSort>();

            TurnSort[] TurnArray = new TurnSort[7];


            for (int i = 6; i >= 0; i--)
            {
                IHasTurn hasTurn = Substitute.For<IHasTurn>();
                TurnSort turnSort = new TurnSort(0, i, new Vector3Int(0, 0, 0), hasTurn);
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
            List<TurnSort> TurnList = new List<TurnSort>();
            List<TurnSort> ExpectedList = new List<TurnSort>();

            TurnSort[] TurnArray = new TurnSort[2];


            for (int i = 1; i >= 0; i--)
            {
                IHasTurn hasTurn = Substitute.For<IHasTurn>();
                TurnSort turnSort = new TurnSort(0, 0, new Vector3Int(i, 0, -i), hasTurn);
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
    */
}