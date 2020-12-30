using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.BattleCamera;

namespace CriticalRole.Turns
{

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

        void AddStartTurnEvent(IStartTurnEvent startTurnEvent);
    }






    [RequireComponent(typeof(TurnControllerMarker))]
    public class TurnController : MonoBehaviour, ITurnController
    {

        //----------------------------------------------------------------------------
        //             Registration
        //----------------------------------------------------------------------------
        
        #region Registration

        private void Awake()
        {
            GameObject dependancyGO = FindObjectOfType<DependancyManagerMarker>().gameObject;
            IBattleDependancyManager dependancyManager = dependancyGO.GetComponent<IBattleDependancyManager>();
            dependancyManager.RegisterTurnController(this);
        }

        #endregion





        //----------------------------------------------------------------------------
        //             Initialise
        //----------------------------------------------------------------------------

        #region Initialise

        /// <summary>
        /// BeginGame starts the first turn <para />
        /// Must be called last by the dependancy manager
        /// </summary>
        public void Initialise()
        {
            BuildTurnList();
            StartTurnEvents = new List<IStartTurnEvent>();

            Hideable[] hideables = FindObjectsOfType<Hideable>();
            foreach(Hideable hideable in hideables)
            {
                hideable.Initialise();
            }
        }

        /// <summary>
        /// Sorted Initiative Order
        /// </summary>
        public List<ITurnSort> TurnList;

        public List<IStartTurnEvent> StartTurnEvents;

        #endregion
        //----------------------------------------------------------------------------
        //             AddStartTurnEvent
        //----------------------------------------------------------------------------

        public void AddStartTurnEvent(IStartTurnEvent startTurnEvent)
        {
            StartTurnEvents.Add(startTurnEvent);
        }

        //----------------------------------------------------------------------------
        //             BeginGame
        //----------------------------------------------------------------------------

        public void BeginGame()
        {
            StartTurnEvents.Sort(new StartTurnSort());
            TurnChangeover();
        }


        //----------------------------------------------------------------------------
        //             BuildTurnList
        //---------------------------------------------------------------------------

        public void BuildTurnList()
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
        }

        //----------------------------------------------------------------------------
        //             UpdateCameraTurn
        //----------------------------------------------------------------------------

        #region UpdateCurrentTurn
        public void UpdateCurrentTurn()
        {
            CurrentTurn++;
            if (CurrentTurn > HasTurnMaxIndex)
            {
                CurrentTurn = 0;
            }
        }

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

        public IEnumerator TurnChangeoverCoroutine()
        {
            UpdateCurrentTurn();
            foreach (IStartTurnEvent startTurnEvent in StartTurnEvents)
            {
                yield return StartCoroutine(startTurnEvent.StartTurn(TurnList[CurrentTurn].HasTurn));
            }
            TurnList[CurrentTurn].HasTurn.StartTurn();
        }


        #endregion
        /// <summary>
        /// Increase the initiative order index. If it is out of bounds, go back to the beginning
        /// </summary>
        /// 






    }

}
