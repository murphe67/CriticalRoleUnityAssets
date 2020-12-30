using System;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------
//              Class Description
//----------------------------------------------------------------------------
//
// Any IHasTurn should generate their own ITurnSort when called to by the
// TurnController. The TurnController adds it to the list.
//
// Once it has all the IHasTurn's, it sorts the list based on their TurnSort, 
// giving the initiative order.
//
// The only thing accessible to the TurnController is the IHasTurn that generated it

/// <summary>
/// Abstracts the sorting of IHasTurns for initiative order away from the IHasTurn
/// </summary>
public interface ITurnSort : IComparable
{
    IHasTurn HasTurn { get; }
}

/// <summary>
/// Generic initiative order sorting: <para/>
/// First highest initiative wins, then on matching initative, highest dexterity wins <para/>
/// In the very rare occasion both are the same, its done based on hex coords <para/>
/// The hex coords ordering is also pretty random 
/// </summary>
public class BaseTurnSort : ITurnSort
{
    public int Initiative;

    public int Dexterity;

    public Vector3Int Coords;

    public IHasTurn HasTurn { get; set; }

    public BaseTurnSort(int initiative, int dexterity, Vector3Int coords, IHasTurn hasTurn)
    {
        Initiative = initiative;
        Dexterity = dexterity;
        Coords = coords;
        HasTurn = hasTurn;
    }

    //----------------------------------------------------------------------------
    //              IComparable
    //----------------------------------------------------------------------------

    #region IComparable


    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        BaseTurnSort otherTurnSort = (BaseTurnSort)obj;

        if (otherTurnSort != null)
        {
            return TurnCompare(this, otherTurnSort);
        }
        else throw new ArgumentException("Object is not a TurnSort");
    }

    private int TurnCompare(BaseTurnSort a, BaseTurnSort b)
    {
        if (a.Initiative < b.Initiative)
        {
            return 1;
        }
        else if (a.Initiative > b.Initiative)
        {
            return -1;
        }
        else if (a.Dexterity < b.Dexterity)
        {
            return 1;
        }
        else if (a.Dexterity > b.Dexterity)
        {
            return -1;
        }
        else
        {
            return -(Vec3IntExt.Vec3IntCompare(a.Coords, b.Coords));
        }
    }

    #endregion

}