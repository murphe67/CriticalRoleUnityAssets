using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------
//              Class Description
//----------------------------------------------------------------------------

// Fairly boring bog-standard A* pathfinding class
// 
// Can be instanced wherever, passed 2 hexes and will return a MapPath that has
// all the info about that path to allow UI and AI to easily understand specifics
//
// Interfaced through IMapPathFinding, two accessible functions
//
// Using MapPathfinding is reliant on MapGeneration, but is only going to be called
// once the game has actually begun, so in practice is independant
//
// Can be instanced as many times as needed, wherever is needed

public interface IMapPathfinding
{
    MapPath GeneratePath(IHexagon hex_a, IHexagon hex_b);
    List<IHexagon> GetHexesInRange(IHexagon start, int range);
}


public class MapPathfinding : IMapPathfinding
{

    //----------------------------------------------------------------------------
    //              Generate Path
    //----------------------------------------------------------------------------

    #region GeneratePath

    /// <summary>
    /// Generic A* pathfinding, hex distance as heuristic
    /// </summary>
    public MapPath GeneratePath(IHexagon hex_a, IHexagon hex_b)
    {
        SortedSet<IPathValue> openSet = new SortedSet<IPathValue>();
        PathValueDict = new Dictionary<IHexagon, IPathValue>();

        Start = hex_a;
        Destination = hex_b;

        IPathValue firstPath = GetPathValue(hex_a);
        firstPath.SetFirstPathDistance();

        openSet.Add(firstPath);
        while (openSet.Count != 0)
        {
            IPathValue currentPath = openSet.Min;
            openSet.Remove(currentPath);

            IHexagon currentHex = currentPath.Hex;


            if (currentHex.CoOrds == hex_b.CoOrds)
            {
                return currentPath.ReconstructPath();
            }


            foreach (IHexagon neighbour in currentHex.Neighbours)
            {
                if(!neighbour.IsOccupied())
                {
                    IPathValue neighbourPath = GetPathValue(neighbour);
                    int newGScore = currentPath.Score_g + neighbour.MovementCost;
                    if (newGScore < neighbourPath.Score_g)
                    {
                        if (openSet.Contains(neighbourPath))
                        {
                            openSet.Remove(neighbourPath);
                        }

                        neighbourPath.UpdateValues(newGScore, currentPath);
                        openSet.Add(neighbourPath);
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Dictionary allows you to access pathvalue based on hexagon
    /// instead of through the priority queue
    /// </summary>
    public Dictionary<IHexagon, IPathValue> PathValueDict;

    /// <summary>
    /// Class variable allows heuristic to be calculated
    /// for all nodes
    /// </summary>
    public IHexagon Destination;

    /// <summary>
    /// Used to calculate the centre of the path, meaning paths are straighter
    /// </summary>
    public IHexagon Start;

    #endregion

    //----------------------------------------------------------------------------
    //              Hexes in Range
    //----------------------------------------------------------------------------

    /// <summary>
    /// This is an almost direct copy of above, but it sets the heuristic distance to
    /// to the start location <para/>
    /// 
    /// So in practice it is Dijkstra's out until range
    /// </summary>
    public List<IHexagon> GetHexesInRange(IHexagon start, int range)
    {
        SortedSet<IPathValue> openSet = new SortedSet<IPathValue>();
        List<IHexagon> hexesInRange = new List<IHexagon>();
        PathValueDict = new Dictionary<IHexagon, IPathValue>();

        Destination = start;
        Start = start;

        IPathValue firstPath = GetPathValue(start);
        firstPath.SetFirstPathDistance();

        openSet.Add(firstPath);
        while (openSet.Count != 0)
        {
            IPathValue currentPath = openSet.Min;
            openSet.Remove(currentPath);

            IHexagon currentHex = currentPath.Hex;
            hexesInRange.Add(currentHex);

            foreach (IHexagon neighbour in currentHex.Neighbours)
            {
                if (!neighbour.IsOccupied())
                {
                    IPathValue neighbourPath = GetPathValue(neighbour);
                    int newGScore = currentPath.Score_g + neighbour.MovementCost;
                    if (newGScore < neighbourPath.Score_g && newGScore <= range)
                    {
                        if (openSet.Contains(neighbourPath))
                        {
                            openSet.Remove(neighbourPath);
                        }

                        neighbourPath.UpdateValues(newGScore, currentPath);
                        openSet.Add(neighbourPath);
                    }
                }
            }          
        }
        return hexesInRange;


    }

    //----------------------------------------------------------------------------
    //              Get Path Value
    //----------------------------------------------------------------------------

    /// <summary>
    /// GetPathValue checks if a hex has been visited before. <para/>
    /// If it has, return the original PathValue <para/>
    /// Otherwise, construct a new PathValue
    /// 
    /// </summary>
    public IPathValue GetPathValue(IHexagon hex)
    {
        if(PathValueDict.ContainsKey(hex))
        {
            return PathValueDict[hex];
        }
        else
        {
            Vector3 centre = (Destination.HexTransform.position + Start.HexTransform.position) / 2;
            IPathValue myPath = new PathValue(400000, HexMath.FindDistance(hex, Destination), centre, hex, null);
            PathValueDict.Add(hex, myPath);
            return myPath;
        }
    }
}
