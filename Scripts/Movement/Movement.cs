using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------
//              Class Description
//----------------------------------------------------------------------------
//
// Here's the script that actually moves things once they know where they're going
//
// For now, theres no way for things to go wrong- once you've decided where you're going
// You're going there
//
// Later on interruptions like tripping or opportunity attacks will probably be implemented here
// So possibly this will also update the movement left of a character? so if it is interrupted
// its at the right value

public interface IMovement
{
    /// <summary>
    /// IMovement calls on the IMovement controller once movement has finished <para />
    /// This relationship is established here
    /// </summary>
    void Initialise(IMovementController movementController);

    /// <summary>
    /// Move this IHexContent to the end of this path
    /// </summary>
    void DoMove(IHasTurn hasTurn, MapPath path);

}

public class Movement : MonoBehaviour, IMovement
{
    //----------------------------------------------------------------------------
    //              Initialise
    //----------------------------------------------------------------------------

    #region Initialise
    /// <summary>
    /// IMovement calls on the IMovement controller once movement has finished <para />
    /// This relationship is established here
    /// </summary>
    public void Initialise(IMovementController movementController)
    {
        MyMovementController = movementController;
    }

    public IMovementController MyMovementController;



    #endregion


    //----------------------------------------------------------------------------
    //              DoMove
    //----------------------------------------------------------------------------

    #region DoMove
    /// <summary>
    /// Called by a movement controller to move the IHasTurn to the end of path
    /// </summary>
    public void DoMove(IHasTurn hasTurn, MapPath path)
    {
        MyHasTurn = hasTurn;
        Path = path;

        Moving = false;

        Path.PathStack.Pop();
        MoveToDestination();
    }

    /// <summary>
    /// The aIHasTurn being moved to a new hex
    /// </summary>
    IHasTurn MyHasTurn;

    /// <summary>
    /// The path to move Contents to the end of
    /// </summary>
    MapPath Path;

    /// <summary>
    /// Should the hex contents be in physical motion to a new position right now <para/>
    /// Should only be true once the new location has been put into NextHexTransform
    /// </summary>
    bool Moving;

    #endregion

    //----------------------------------------------------------------------------
    //              MoveToDestination
    //----------------------------------------------------------------------------

    /// <summary>
    /// The forking function- <para/>
    /// If the end of the path has not been reached, move a single hex forward <para/>
    /// Otherwise, end the movement <para/>
    /// Any kind of effect a hex will have on a hex content will probably be applied here
    /// </summary>
    public void MoveToDestination()
    {
        if (Path.PathStack.Count > 0)
        {
            StepDownPath();
        }
        else
        {
            MyMovementController.EndMove();
        }
    }


    //----------------------------------------------------------------------------
    //              StepDownpath
    //----------------------------------------------------------------------------

    #region StepDownPath

    public void StepDownPath()
    {
        IHexagon nextHex = Path.PathStack.Pop();

        //the hex you're moving out of should be marked empty
        MyHasTurn.MyHexContents.Location.Contents = null;

        //occupy new hex, and update location
        MyHasTurn.MyHexContents.Location = nextHex;
        nextHex.Contents = MyHasTurn.MyHexContents;

        MyHasTurn.MyHasSpeed.UseMove(nextHex.MovementCost);

        NextHexTransform = nextHex.HexTransform;
        Moving = true;
    }

    /// <summary>
    /// Each frame Moving is true, Contents moves towards this position
    /// </summary>
    Transform NextHexTransform;


    #endregion

    //----------------------------------------------------------------------------
    //              MoveTransform
    //----------------------------------------------------------------------------

    #region MoveTransform

    void Update()
    {
        MoveTransform();
    }

    /// <summary>
    /// Physically moves Contents toward NextHexTransform while moving is true <para/>
    /// Once it has reached NextHexTransform, moving is set to false, and the forking function is called
    /// </summary>
    public void MoveTransform()
    {
        if (Moving)
        {
            MyHasTurn.MyHexContents.ContentTransform.position = Vector3.MoveTowards(MyHasTurn.MyHexContents.ContentTransform.position, NextHexTransform.position, Time.deltaTime * PhysicalMoveSpeed);
            
            if (Vector3.Distance(MyHasTurn.MyHexContents.ContentTransform.position, NextHexTransform.position) < 0.01)
            {
                Moving = false;
                MoveToDestination();
            }
        }
    }

    //How far Contents should move per second
    public readonly float PhysicalMoveSpeed = 2.5f;

    #endregion
}
