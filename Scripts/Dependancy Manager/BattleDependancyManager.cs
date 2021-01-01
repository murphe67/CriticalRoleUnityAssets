using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.BattleCamera;
using CriticalRole.Turns;
using CriticalRole.UI;

//----------------------------------------------------------------------------
//             Class Description
//----------------------------------------------------------------------------
//
// Any system which is reliant on an different independant system should register
// itself to the Dependancy Manager to allow them to initialise in the correct order
//
// This is only to be used as a last resort- if the dependant system can be initialised
// by the other, it should
//
// But for core systems, they should not be aware of each other in that way
//
// Currently 2 dependant systems:
// the IHexagon spawned by the MapGeneration needs to communicate with UI_Input
// when it is clicked. 
// However, MapGeneration should not initialise UI_Input.
// 
// TurnController starts the turn based gameplay, and so it dependant on every other system
//
// However TurnController does not have any configurable set-up, and so is spawned by the
// Dependancy Manager




public interface IBattleDependancyManager
{
    void RegisterMapGeneration(IMapGeneration mapGeneration);
    void RegisterUIManager(I_UIManager uiManager);
    void RegisterTurnController(ITurnController turnController);
    void RegisterCameraManager(IBattleCamManager myBattleCameraManager);
}





[RequireComponent(typeof(DependancyManagerMarker))]
public class BattleDependancyManager : MonoBehaviour, IBattleDependancyManager
{
    //----------------------------------------------------------------------------
    //             Registration
    //----------------------------------------------------------------------------

    #region Registration

    /// <summary>
    /// Called by MapGeneration to register itself as THE MapGeneration <para />
    /// If there are two, the first is completely ignored
    /// </summary>
    public void RegisterMapGeneration(IMapGeneration mapGeneration)
    {
        MyMapGeneration = mapGeneration;
    }

    IMapGeneration MyMapGeneration;

    /// <summary>
    /// Called by UI_Input to register itself as THE UI_Input <para />
    /// If there are two, the first is completely ignored
    /// </summary>
    public void RegisterUIManager(I_UIManager uiManager)
    {
        MyUIManager = uiManager; 
    }

    I_UIManager MyUIManager;

    /// <summary>
    /// Called by TurnController to register itself as THE TurnController <para />
    /// If there are two, the first is completely ignored
    /// </summary>
    public void RegisterTurnController(ITurnController turnController)
    {
        MyTurnController = turnController;
    }

    ITurnController MyTurnController;

    public void RegisterCameraManager(IBattleCamManager myBattleCameraManager)
    {
        MyBattleCameraManager = myBattleCameraManager;
    }

    IBattleCamManager MyBattleCameraManager;

    #endregion




    //----------------------------------------------------------------------------
    //             Initialisation
    //----------------------------------------------------------------------------


    #region Initialisation

    // Assert that the necessary components of a battlefield exist
    // One approach would be to have the dependancy manager spawn everything
    // but that would lose the benefit of seperating out the settings -they'd all be on one
    // monobehaviour, which gets messy
    //
    // UI_Input works better as placed in the scene, as the UI needs to register button functions
    // to it, so it should exist before runtime

   
    private void Start()
    {
        MyMapGeneration.Initialise();
        MyTurnController.Initialise();
        MyUIManager.Initialise();
        MyBattleCameraManager.Initialise();

        MyTurnController.BeginGame();
    }
  


    #endregion




    //----------------------------------------------------------------------------
    //             Singleton Assertion
    //----------------------------------------------------------------------------

    #region Singleton Assertion

    // The dependancy manager is the only 'singleton-esque' manager
    // If you have more than one of the others, one is completely ignored
    //
    // But multiple dependancy managers break the registration process
    private void Awake()
    {
        DependancyManagerMarker[] dependancyManagers = FindObjectsOfType<DependancyManagerMarker>();
        Debug.Assert(dependancyManagers.Length == 1);
    }

    #endregion
}
