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
        /// <summary>
        /// DependancyManager only. Initialises the StartTurn List. <para/>
        /// So other systems can add themselves to it
        /// </summary>
        void Initialise();

        /// <summary>
        /// Called by DependancyManager. Starts the TurnChangover process
        /// </summary>
        void BeginGame();

        void TurnChangeover();

        /// <summary>
        /// Wrapper for StartTurnList.Add(), just to protect the actual list
        /// </summary>
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
            _RegisterWithDependancyManager();
        }

        #region Implementation
        private void _RegisterWithDependancyManager()
        {
            GameObject dependancyGO = FindObjectOfType<DependancyManagerMarker>().gameObject;
            IBattleDependancyManager dependancyManager = dependancyGO.GetComponent<IBattleDependancyManager>();
            dependancyManager.RegisterTurnController(this);
        }
        #endregion

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
            _BuildTurnList();
            StartTurnEvents = new List<IStartTurnEvent>();
        }

        /// <summary>
        /// Sorted Initiative Order
        /// </summary>
        public List<ITurnSort> TurnList;
        public List<IStartTurnEvent> StartTurnEvents;

        #region Implementation
        private void _BuildTurnList()
        {
            HasTurnMarker[] TurnMarkerArray = FindObjectsOfType<HasTurnMarker>();
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
        #endregion

        #endregion





        //----------------------------------------------------------------------------
        //             AddStartTurnEvent
        //----------------------------------------------------------------------------

        #region AddStartTurnEvent
        public void AddStartTurnEvent(IStartTurnEvent startTurnEvent)
        {
            StartTurnEvents.Add(startTurnEvent);
        }

        #endregion



        //----------------------------------------------------------------------------
        //             BeginGame
        //----------------------------------------------------------------------------

        #region BeginGame
        public void BeginGame()
        {
            StartTurnEvents.Sort(new StartTurnSort());
            TurnChangeover();
        }

        #endregion




        //----------------------------------------------------------------------------
        //             UpdateCurrentTurn
        //----------------------------------------------------------------------------

        #region UpdateCurrentTurn

        /// <summary>
        /// Incrememnt CurrentTurn, if out of bounds, set to 0
        /// </summary>
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
        public int HasTurnMaxIndex;

        /// <summary>
        /// current initiative order index
        /// </summary>
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






    }

}
