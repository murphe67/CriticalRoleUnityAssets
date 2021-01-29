using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Dependancy;
using CriticalRole.Health;
using CriticalRole.Narration;
using CriticalRole.UI;
using CriticalRole.BattleCamera;
using CriticalRole.Death;

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
        /// Called by DependancyManager. Starts the TurnChangover process
        /// </summary>
        void BeginGame();

        void TurnChangeover();

        /// <summary>
        /// Wrapper for StartTurnList.Add(), just to protect the actual list
        /// </summary>
        void AddStartTurnEvent(IStartTurnEvent startTurnEvent);

        void RegisterNarrator(Narrator narrator);
        void RegisterBattleCamManager(IBattleCamManager camManager);
    }






    [RequireComponent(typeof(TurnControllerMarker))]
    public class TurnController : MonoBehaviour, ITurnController, IDeathEvent
    {

        //----------------------------------------------------------------------------
        //             Awake
        //----------------------------------------------------------------------------

        #region Awake

        private void Awake()
        {
            _RegisterWithDependancyManager();
            _AddDeathEvent();
        }

        #region Implementation
        private void _RegisterWithDependancyManager()
        {
            GameObject dependancyGO = _GetDependancyGO();
            IBattleDependancyManager dependancyManager = dependancyGO.GetComponent<IBattleDependancyManager>();
            dependancyManager.RegisterTurnController(this);
        }

        private void _AddDeathEvent()
        {
            DeathManagerMarker[] deathManagerMarkers = FindObjectsOfType<DeathManagerMarker>();
            foreach (DeathManagerMarker deathManagerMarker in deathManagerMarkers)
            {
                deathManagerMarker.gameObject.GetComponent<IDeathManager>().AddGlobalDeathEvent(this);
            }
        }


        private GameObject _GetDependancyGO()
        {
            DependancyManagerMarker dependancy = FindObjectOfType<DependancyManagerMarker>();
            if (dependancy == null)
            {
                Debug.LogError("No dependancy manager in scene");
            }
            return dependancy.gameObject;
        }


        #endregion

        #endregion



        //----------------------------------------------------------------------------
        //            DependancyInjections
        //----------------------------------------------------------------------------

        #region DependancyInjections


        //----------------------------------------------------------------------------
        //             RegisterNarrator
        //----------------------------------------------------------------------------

        #region RegisterNarrator
        public void RegisterNarrator(Narrator narrator)
        {
            MyNarrator = narrator;
        }

        public Narrator MyNarrator;

        #endregion



        //----------------------------------------------------------------------------
        //             RegisterBattleCamManager
        //----------------------------------------------------------------------------

        #region RegisterBattleCamManager
        public void RegisterBattleCamManager(IBattleCamManager camManager)
        {
            MyCamManager = camManager;
        }

        IBattleCamManager MyCamManager;

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

        private List<IStartTurnEvent> _StartTurnEvents;
        public List<IStartTurnEvent> StartTurnEvents
        {
            get
            {
                if (_StartTurnEvents == null)
                {
                    _StartTurnEvents = new List<IStartTurnEvent>();
                }
                return _StartTurnEvents;
            }
        }

        #endregion


        //----------------------------------------------------------------------------
        //             BeginGame
        //----------------------------------------------------------------------------

        #region BeginGame
        public void BeginGame()
        {
            _BuildTurnList();
            StartTurnEvents.Sort(new StartTurnSort());
            TurnChangeover();
        }

        #region Implementation
        private void _BuildTurnList()
        {
            HasTurnMarker[] TurnMarkerArray = FindObjectsOfType<HasTurnMarker>();

            HasTurnMaxIndex = -1;
            foreach (HasTurnMarker turnMarker in TurnMarkerArray)
            {
                HasTurnMaxIndex++;
                IHasTurn hasTurn = turnMarker.gameObject.GetComponent<IHasTurn>();
                hasTurn.Initialise(this);
                TurnList.Add(turnMarker.gameObject.GetComponent<IHasTurn>());
                hasTurn.Index = HasTurnMaxIndex;
            }

            TurnList.Sort(new TurnSort());
            CurrentTurn = -1;
        }

        private List<IHasTurn> _TurnList;

        /// <summary>
        /// Sorted Initiative Order
        /// </summary>
        public List<IHasTurn> TurnList
        {
            get
            {
                if(_TurnList == null)
                {
                    _TurnList = new List<IHasTurn>();
                }
                return _TurnList;
            }
        }

        #endregion

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
            IHasTurn hasTurn = TurnList[CurrentTurn];


            Coroutine camCoroutine = StartCoroutine(MyCamManager.StartTurnScript(hasTurn));
            yield return StartCoroutine(MyNarrator.StartTurn(hasTurn));
            StopCoroutine(camCoroutine);
            yield return StartCoroutine(MyCamManager.StartTurnPlayer(hasTurn));

            
            foreach (IStartTurnEvent startTurnEvent in StartTurnEvents)
            {
                yield return StartCoroutine(startTurnEvent.StartTurn(hasTurn));
            }

            //hasTurn.StartTurn();
        }


        #endregion


        //----------------------------------------------------------------------------
        //             ReactToDeath
        //----------------------------------------------------------------------------

        #region  ReactToDeath

        public IEnumerator ReactToDeath(IHasTurn hasTurn)
        {
            TurnList.Remove(hasTurn);
            HasTurnMaxIndex--;
            if(CurrentTurn >= hasTurn.Index)
            {
                CurrentTurn--;
                TurnChangeover();
            }
            yield break;
        }
        #endregion

    }

}
