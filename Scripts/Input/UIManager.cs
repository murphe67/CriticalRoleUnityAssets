using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Turns;
using CriticalRole.Move;

namespace CriticalRole.UI
{

    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------

    // If it happens because the player clicked something, it should be handled
    // through this class
    //
    // Currently activates/deactives the player ui, and sends any inputs relating to 
    // movement to IPlayerMoveController

    public interface I_UIManager
    {
        void Initialise();

        UI_InputMove MyUI_InputMove { get; }

        UI_InputAttack MyUI_InputAttack { get; }

        void IHexagonClicked(IHexagon hexagon);

        void IHexagonHovered(IHexagon hexagon);

        void IHexagonUnhovered(IHexagon hexagon);
    }











    [RequireComponent(typeof(UI_InputMove))]
    [RequireComponent(typeof(UI_InputAttack))]
    public class UIManager : MonoBehaviour, I_UIManager, IStartTurnEvent
    {
        [Header("Set In Inspector")]
        public GameObject TurnUICanvas;
        public GameObject BackButtonCanvas;

        



        //----------------------------------------------------------------------------
        //                    Registration
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
            dependancyManager.RegisterUIManager(this);
        }

        #endregion



        #endregion





        //----------------------------------------------------------------------------
        //                    Initialise
        //----------------------------------------------------------------------------

        #region Initialise


        /// <summary>
        /// UI Input initialise is dependant on IHexagon being initialised correctly <para />
        /// Must be initialised after MapGeneration by the Dependancy Manager
        /// </summary>
        public void Initialise()
        {
            _SetUpSubClasses();
            _SetUpUICanvases();
            _PassRefToPlayerTurns();
            _PassRefToIHexagons();
            _PassRefToTurnController();
        }

        public UI_InputMove MyUI_InputMove { get; set; }
        public UI_InputAttack MyUI_InputAttack { get; set; }

        public List<UI_InputSubclass> Subclasses;
        public List<GameObject> UICanvases;



        #region Implmentation

        private void _SetUpSubClasses()
        {
            Subclasses = new List<UI_InputSubclass>();

            MyUI_InputMove = GetComponent<UI_InputMove>();
            MyUI_InputMove.Initialise(this);
            Subclasses.Add(MyUI_InputMove);


            MyUI_InputAttack = GetComponent<UI_InputAttack>();
            MyUI_InputAttack.Initialise(this);
            Subclasses.Add(MyUI_InputAttack);
        }

        private void _SetUpUICanvases()
        {
            UICanvases = new List<GameObject>();
            UICanvases.Add(BackButtonCanvas);
            UICanvases.Add(TurnUICanvas);
        }

        private void _PassRefToPlayerTurns()
        {
            PlayerTurn[] playerTurns = FindObjectsOfType<PlayerTurn>();
            foreach (PlayerTurn playerTurn in playerTurns)
            {
                playerTurn.SetUI_InputReference(this);
            }
        }

        private void _PassRefToIHexagons()
        {
            IHexagonMarker[] ihexagonMarkers = FindObjectsOfType<IHexagonMarker>();
            foreach (IHexagonMarker ihexagonMarker in ihexagonMarkers)
            {
                ihexagonMarker.gameObject.GetComponent<IHexagon>().Interaction.MyUIManager = this;
            }
        }

        private void _PassRefToTurnController()
        {
            TurnControllerMarker[] turnControllerMarkers = FindObjectsOfType<TurnControllerMarker>();
            foreach (TurnControllerMarker turnControllerMarker in turnControllerMarkers)
            {
                turnControllerMarker.GetComponent<ITurnController>().AddStartTurnEvent(this);
            }
        }

        #endregion


        #endregion




        //----------------------------------------------------------------------------
        //                   StartTurn
        //----------------------------------------------------------------------------

        #region StartTurn

        public IEnumerator StartTurn(IHasTurn currentIHasTurn)
        {
            CurrentIHasTurn = currentIHasTurn;
            ShowTurnUI();
            yield break;
        }

        public IHasTurn CurrentIHasTurn;

        #region Implementation

        public StartTurnType MyStartTurnType
        {
            get
            {
                return StartTurnType.UIEvent;
            }
        }

        #endregion

        #endregion




        //----------------------------------------------------------------------------
        //                   EndTurnButton
        //----------------------------------------------------------------------------

        #region EndTurnButton

        public void EndTurnButton()
        {
            NoUI();
            CurrentIHasTurn.EndTurn();
        }

        #endregion




        //----------------------------------------------------------------------------
        //                   BackButton
        //----------------------------------------------------------------------------

        public void BackButton()
        {
            foreach(UI_InputSubclass subclass in Subclasses)
            {
                subclass.BackButton();
            }
        }


        //----------------------------------------------------------------------------
        //                   Hexagon
        //----------------------------------------------------------------------------

        #region Hexagon

        public void IHexagonClicked(IHexagon hexagon)
        {
            foreach(UI_InputSubclass subclass in Subclasses)
            {
                subclass.IHexagonClicked(hexagon);
            }
        }

        public void IHexagonHovered(IHexagon hexagon)
        {
            foreach (UI_InputSubclass subclass in Subclasses)
            {
                subclass.IHexagonHovered(hexagon);
            }
        }

        public void IHexagonUnhovered(IHexagon hexagon)
        {
            foreach (UI_InputSubclass subclass in Subclasses)
            {
                subclass.IHexagonUnhovered(hexagon);
            }
        }

        #endregion




        //----------------------------------------------------------------------------
        //                   Show UI
        //----------------------------------------------------------------------------

        #region ShowUI

        public void NoUI()
        {
            foreach(GameObject canvas in UICanvases)
            {
                canvas.SetActive(false);
            }
        }

        public void ShowTurnUI()
        {
            NoUI();
            TurnUICanvas.SetActive(true);
        }

        public void ShowBackButton()
        {
            NoUI();
            BackButtonCanvas.SetActive(true);
        }

        #endregion



    }

}