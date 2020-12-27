using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------
//              Class Description
//----------------------------------------------------------------------------
//
// This class allows the player to move a character
//
// SelectMoveDestination shows the player all the IHexagons they can move to
// Clicking a possibility moves the character to the new location
//
// Currently no way not to move once you have clicked move
//
// IMovementController interface for the Movement script to return control
//
// While the IPlayerMoveController interface is for UI_Input, so that it can pass
// decisions here, where they are implemented

/// <summary>
/// Interface the movement script accesses the MoveController through. <para/>
/// Only communication (currently) is that movement has ended at the final destination
/// </summary>
public interface IMovementController
{
    /// <summary>
    /// Called by Movement <para/>
    /// The IHexContent has reached where you sent it
    /// </summary>
    void EndMove();
}




public interface IPlayerMoveController
{
    /// <summary>
    /// Show all IHexagons a character can move to and make them clickable
    /// </summary>
    /// <param name="currentIHasTurn"></param>
    void SelectMoveDestination(IHasTurn currentIHasTurn);

    void HighlightPath(IHexagon ihexagon);

    /// <summary>
    /// Called when a hexagon has been clicked, to move the character there
    /// </summary>
    void DoMove(IHexagon endLocation);

    void BackFromMove();

}






public class PlayerMoveController : IMovementController, IPlayerMoveController
{
    //----------------------------------------------------------------------------
    //              Construction
    //----------------------------------------------------------------------------

    #region Construction
    public PlayerMoveController()
    {
        MyMapPathfinding = new MapPathfinding();

        GameObject go = new GameObject("Player Movement GO");
        MyMovement = go.AddComponent<Movement>();
        MyMovement.Initialise(this);
    }

    public IMapPathfinding MyMapPathfinding;
    public IMovement MyMovement;

    #endregion

    //----------------------------------------------------------------------------
    //              SelectMoveDestination
    //----------------------------------------------------------------------------

    #region SelectMoveDestination
    /// <summary>
    /// Show all IHexagons a character can move to and make them clickable
    /// </summary>
    /// <param name="currentIHasTurn"></param>
    public void SelectMoveDestination(IHasTurn currentIHasTurn)
    {
        HexesInRange = MyMapPathfinding.GetHexesInRange(currentIHasTurn.MyHexContents.Location, currentIHasTurn.MyHasSpeed.CurrentMovement());
        HexesInRange.Remove(currentIHasTurn.MyHexContents.Location);

        CurrentIHasTurn = currentIHasTurn;

        foreach (IHexagon hex in HexesInRange)
        {
            hex.Interaction.MakeSelectable();
        }
    }

    /// <summary>
    /// Set in SelectMoveDestination, which shows all the hexes this can go to <para/>
    /// The DoMove function then moves it
    /// </summary>
    public IHasTurn CurrentIHasTurn;

    List<IHexagon> HexesInRange;

    public void HighlightPath(IHexagon ihexagon)
    {
        MapPath path = MyMapPathfinding.GeneratePath(CurrentIHasTurn.MyHexContents.Location, ihexagon);
        path.PathStack.Pop();
        foreach(IHexagon hex in HexesInRange)
        {
            hex.Interaction.Unhighlight();
        }
        while(path.PathStack.Count != 0)
        {
            IHexagon toHighlight = path.PathStack.Pop();
            toHighlight.Interaction.Highlight();
        }
    }

    #endregion
    
    //----------------------------------------------------------------------------
    //              DoMove
    //----------------------------------------------------------------------------

    /// <summary>
    /// Revert the hexagons to inert <para/>
    /// Then find a path for CurrentIHasTurn to endLocation, and pass that path to Movement
    /// </summary>
    public void DoMove(IHexagon endLocation)
    {
        foreach (IHexagon hex in HexesInRange)
        {
            hex.Interaction.MakeUnselectable();
        }


        MapPath myMapPath = MyMapPathfinding.GeneratePath(CurrentIHasTurn.MyHexContents.Location, endLocation);
        MyMovement.DoMove(CurrentIHasTurn, myMapPath);
    }



    //----------------------------------------------------------------------------
    //              EndMove
    //----------------------------------------------------------------------------

    /// <summary>
    /// This wrapping seems bad at the minute, but eventually it'll will call and EndMove function <para/>
    /// Which will allow the CurrentIHasTurn to make further decisions
    /// </summary>
    public void EndMove()
    {
        CurrentIHasTurn.EndMove();
    }

    //----------------------------------------------------------------------------
    //              BackFromMove
    //----------------------------------------------------------------------------

    public void BackFromMove()
    {
        foreach (IHexagon hex in HexesInRange)
        {
            hex.Interaction.MakeUnselectable();
        }
    }

}
