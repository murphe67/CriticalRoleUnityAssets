using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//----------------------------------------------------------------------------
//                    Class Description
//----------------------------------------------------------------------------
//
// This class forms the tile system the map is based off
// 
// Is currently 2D, but eventually should be extended to 3D
//
// It is the main access point for anything map related, including any of the
// pathfinding functions or location.
//
// Different interfaces depending on what access is needed
//
// Due to the size of the class, the way the IHexagon interacts with the UI_Input
// is in a different class HexInteract




public interface IHexagon
{
    //--------------------------------------
    //           Pathfinding
    //--------------------------------------

    /// <summary>
    /// Location in HexCoords
    /// </summary>
    Vector3Int CoOrds { get; }

    /// <summary>
    /// Movement cost in units of 5ft
    /// </summary>
    int MovementCost { get; }

    List<IHexagon> Neighbours { get;}

    void KeepHexagon();

    bool RemoveUnused();

    bool IsOccupied();


    //--------------------------------------
    //           Content
    //--------------------------------------

    IHexContents Contents { get; set; }


    //--------------------------------------
    //           Movement
    //--------------------------------------

    /// <summary>
    /// The hexagon's transform <para/>
    /// Used by Movement to physically move a mesh onto the hex
    /// </summary>
    Transform HexTransform { get; }

    //--------------------------------------
    //           Interaction
    //--------------------------------------

    /// <summary>
    /// Functions related to the hexagon reacting to actions are in Interaction
    /// </summary>
    IHexInteract Interaction { get; }
}

/// <summary>
/// This interface is only for MapGeneration
/// Allows the neighbours to be edited
/// </summary>
public interface IHexagonNeighbours : IHexagon
{
    //--------------------------------------
    //           Setup
    //--------------------------------------

    /// <summary>
    /// Set the internal neighbours list
    /// </summary>
    void AddNeighbours(List<IHexagon> neighbours);
}









[RequireComponent(typeof(IHexagonMarker))]
public class Hexagon : MonoBehaviour, IHexagonNeighbours
{
    //----------------------------------------------------------------------------
    //                    Initialisation
    //----------------------------------------------------------------------------


    #region Initialisation
    /// <summary>
    /// Initialise variables, and verify coords
    /// </summary>
    public void Initialise(Vector3Int coords, int movementCost)
    {
        Debug.Assert((coords.x + coords.y + coords.z) == 0);

        CoOrds = coords;
        MovementCost = 1;

        Interaction = gameObject.AddComponent<HexInteract>();
        Interaction.hex = this;
    }

    #endregion





    //----------------------------------------------------------------------------
    //                    Neighbours
    //----------------------------------------------------------------------------

    /// <summary>
    /// Set function only accessible to MapGeneration
    /// </summary>
    public void AddNeighbours(List<IHexagon> neighbours)
    {
        Neighbours = neighbours;
    }





    //----------------------------------------------------------------------------
    //                    Pathfinding
    //--------------------------------------------------------------------------

    #region Pathfinding
    public Vector3Int CoOrds { get; set; }

    public int MovementCost { get; set; }

    public List<IHexagon> Neighbours { get; set; }

    public bool IsOccupied()
    {
        if(Contents != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void KeepHexagon()
    {
        Keep = true;
    }

    public bool Keep = false;

    public bool RemoveUnused()
    {
        if(!Keep)
        {
            Destroy(gameObject);
        }
        return !Keep;
    }

    #endregion




    //----------------------------------------------------------------------------
    //                    Content
    //--------------------------------------------------------------------------

    public IHexContents Contents { get; set; }




    //----------------------------------------------------------------------------
    //                    Movement
    //--------------------------------------------------------------------------

    public Transform HexTransform
    {
        get
        {
            return gameObject.transform;
        }
    }




    //----------------------------------------------------------------------------
    //                    Interaction
    //----------------------------------------------------------------------------

    public IHexInteract Interaction { get; set; }
}
