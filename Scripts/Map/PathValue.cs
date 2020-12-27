using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------
//              Class Description
//----------------------------------------------------------------------------
//
// The A* algorithm needs the set of hexagons to be sorted based on f score
// This class abstract that away from the hexagon, and also stores a reference to the hexagon
// 
// Also stores other data for A*, like the heuristic and the previous PathValue
//
// Has an additional factor to the comparison: distance from the centre line
// Hex pathfinding in wavy lines produces this like cone shape of equivalent paths
//
// Without the centre comparison, it did it based on coords, which produced a lot of right angles
// so if the f score is equal, it picks the hex physically (euclideanly) closest to the centre line
//
// This might produce weird behaviour on longer paths around obstacles? Keep an eye out for when
// I get to that stage


public interface IPathValue : IComparable
{
    /// <summary>
    /// Instead of the default Score_g of infinity, set the first node score_g to 0
    /// </summary>
    void SetFirstPathDistance();

    /// <summary>
    /// If a shorter path to a hex is found <para/>
    /// You should update the path to it (previous), and the new cost to this node (score_g)
    /// </summary>
    /// <param name="score_g"></param>
    /// <param name="previous"></param>
    void UpdateValues(int score_g, IPathValue previous);

    /// <summary>
    /// The path values have the path stored as a reverse linked list <para />
    /// This reverses it, and constructs a MapPath <para />
    /// MapPath is currently unimportant, but will hopefully allow ai to make good decisions with movement
    /// </summary>
    MapPath ReconstructPath();

    IHexagon Hex { get; }
    int Score_g { get; }
    int Score_f { get; }
}


/// <summary>
/// The A* pathfinding uses a sorted set of hexagons. 
/// This abstracts the sorting data away from the Hexagon class
/// </summary>
public class PathValue : IPathValue
{
    //----------------------------------------------------------------------------
    //              Variables
    //----------------------------------------------------------------------------

    #region Variables

    public int Heuristic;

    /// <summary>
    /// Centre point between start and end, used to straighten wavy paths
    /// </summary>
    Vector3 Centre;

    public int Score_g { get; set; }
    public int Score_f { get { return Score_g + Heuristic; } }


    public IHexagon Hex { get; set; }

    public PathValue Previous;

    #endregion

    //----------------------------------------------------------------------------
    //              Constructor
    //----------------------------------------------------------------------------

    public PathValue(int score_g, int heuristic, Vector3 centre, IHexagon hex, PathValue previous)
    {
        Score_g = score_g;
        Heuristic = heuristic;
        Centre = centre;
        Hex = hex;
        Previous = previous;
    }

    //----------------------------------------------------------------------------
    //              Interface Methods
    //----------------------------------------------------------------------------

    #region Interface Methods

    /// <summary>
    /// Instead of the default Score_g of infinity, set the first node score_g to 0
    /// </summary>
    public void SetFirstPathDistance()
    {
        Score_g = 0;
    }

    /// <summary>
    /// If a shorter path to a hex is found <para/>
    /// You should update the path to it (previous), and the new cost to this node (score_g)
    /// </summary>
    /// <param name="score_g"></param>
    /// <param name="previous"></param>
    public void UpdateValues(int score_g, IPathValue previous)
    {
        Score_g = score_g;
        Previous = (PathValue)previous;
    }

    /// <summary>
    /// The path values have the path stored as a reverse linked list <para />
    /// This reverses it, and constructs a MapPath <para />
    /// MapPath is currently unimportant, but will hopefully allow ai to make good decisions with movement
    /// </summary>
    public MapPath ReconstructPath()
    {
        MapPath myMapPath = new MapPath();
        myMapPath.cost = Score_g;

        PathValue currentPath = this;
        while (currentPath != null)
        {
            myMapPath.PathStack.Push(currentPath.Hex);
            currentPath = currentPath.Previous;
        }

        return myMapPath;
    }

    #endregion

    //----------------------------------------------------------------------------
    //              IComparable
    //----------------------------------------------------------------------------

    #region IComparable
    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        PathValue otherPathValue = (PathValue)obj;
        if (otherPathValue != null)
        {
            return PathCompare(this, otherPathValue);
        }
        else throw new ArgumentException("Object is not a PathValue");
    }

    private int PathCompare(PathValue a, PathValue b)
    {
        if (a.Score_f < b.Score_f)
        {
            return -1;
        }
        else if (a.Score_f > b.Score_f)
        {
            return 1;
        }
        //here's where the euclidean distance comparison kicks in
        else if(Vector3.Distance(a.Hex.HexTransform.position, Centre) < Vector3.Distance(b.Hex.HexTransform.position, Centre))
        {
            return -1;
        }
        else if (Vector3.Distance(a.Hex.HexTransform.position, Centre) > Vector3.Distance(b.Hex.HexTransform.position, Centre))
        {
            return 1;
        }
        else
        {
            return Vec3IntExt.Vec3IntCompare(a.Hex.CoOrds, b.Hex.CoOrds);
        }
    }
    #endregion
}