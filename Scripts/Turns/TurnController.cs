using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------
//             Class Description
//----------------------------------------------------------------------------
//
// Manages the turn based nature of gameplay.
// Generates an initiative order, then calls StartTurn on the first IHasTurn
// Calls StartTurn on the next one once TurnChangeover is called

public interface ITurnController
{
    void TurnChangeover();
}


public class TurnController : MonoBehaviour, ITurnController
{

    private void Awake()
    {
        GameObject dependancyGO = FindObjectOfType<DependancyManagerMarker>().gameObject;
        IBattleDependancyManager dependancyManager = dependancyGO.GetComponent<IBattleDependancyManager>();
        dependancyManager.RegisterTurnController(this);
    }

    /// <summary>
    /// Sorted Initiative Order
    /// </summary>
    public List<ITurnSort> TurnList;

    /// <summary>
    /// Last initiative order index
    /// </summary>
    [HideInInspector]
    public int HasTurnMaxIndex;

    /// <summary>
    /// current initiative order index
    /// </summary>
    [HideInInspector]
    public int CurrentTurn;


    //----------------------------------------------------------------------------
    //             Initialise
    //----------------------------------------------------------------------------

    #region Initialise

    /// <summary>
    /// BeginGame starts the first turn <para />
    /// Must be called last by the dependancy manager
    /// </summary>
    public void BeginGame()
    {
        HasTurnMarker[] TurnMarkerArray = GameObject.FindObjectsOfType<HasTurnMarker>();
        TurnList = new List<ITurnSort>();

        HasTurnMaxIndex = -1;
        foreach (HasTurnMarker turnMarker in TurnMarkerArray)
        {
            HasTurnMaxIndex++;
            IHasTurn hasTurn = turnMarker.gameObject.GetComponent<IHasTurn>();
            hasTurn.Initialise(this);
            TurnList.Add(turnMarker.gameObject.GetComponent<IHasTurn>().TurnSort);
        }

        TurnList.Sort();

        CurrentTurn = -1;
        TurnChangeover();
    }

    #endregion

    //----------------------------------------------------------------------------
    //             Turn Changeover
    //----------------------------------------------------------------------------

    #region Turn Changeover

    /// <summary>
    /// Update the index and call StartTurn
    /// </summary>
    public void TurnChangeover()
    {
        StartCoroutine(TurnChangeoverCoroutine());
    }

    /// <summary>
    /// Increase the initiative order index. If it is out of bounds, go back to the beginning
    /// </summary>
    public void UpdateCurrentTurn()
    {
        CurrentTurn++;
        if(CurrentTurn > HasTurnMaxIndex)
        {
            CurrentTurn = 0;
        }
    }

    #endregion

    //----------------------------------------------------------------------------
    //             Camera Transition
    //----------------------------------------------------------------------------

    public GameObject BattleCam;
    public TurnTransitionCamera OtherCam;

    public IEnumerator TurnChangeoverCoroutine()
    {
        UpdateCurrentTurn();
        yield return new WaitForSeconds(0.1f);
        BattleCam.SetActive(false);

        yield return StartCoroutine(OtherCam.TurnTransition(TurnList[CurrentTurn].HasTurn));

        BattleCam.SetActive(true);
        TurnList[CurrentTurn].HasTurn.StartTurn();
    }
}
