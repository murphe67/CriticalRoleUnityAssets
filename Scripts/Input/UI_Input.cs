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


    public class UI_Input : MonoBehaviour, IStartTurnEvent
    {
        public GameObject TurnUICanvas;
        public GameObject MoveUICanvas;

        public IHasTurn CurrentIHasTurn;
        public IPlayerMoveController MyPlayerMoveController;

        //----------------------------------------------------------------------------
        //                    Initialise
        //----------------------------------------------------------------------------

        #region Initialise

        private void Awake()
        {
            GameObject dependancyGO = FindObjectOfType<DependancyManagerMarker>().gameObject;
            IBattleDependancyManager dependancyManager = dependancyGO.GetComponent<IBattleDependancyManager>();
            dependancyManager.RegisterUI_Input(this);
        }

        /// <summary>
        /// UI Input initialise is dependant on IHexagon being initialised correctly <para />
        /// Must be initialised after MapGeneration by the Dependancy Manager
        /// </summary>
        public void Initialise()
        {
            PlayerTurn[] playerTurns = FindObjectsOfType<PlayerTurn>();
            foreach (PlayerTurn playerTurn in playerTurns)
            {
                playerTurn.SetUI_InputReference(this);
            }

            MyPlayerMoveController = new PlayerMoveController();


            IHexagonMarker[] ihexagonMarkers = FindObjectsOfType<IHexagonMarker>();
            foreach (IHexagonMarker ihexagonMarker in ihexagonMarkers)
            {
                ihexagonMarker.gameObject.GetComponent<IHexagon>().Interaction.MyUI_Input = this;
            }

            TurnControllerMarker[] turnControllerMarkers = FindObjectsOfType<TurnControllerMarker>();
            foreach(TurnControllerMarker turnControllerMarker in turnControllerMarkers)
            {
                turnControllerMarker.GetComponent<ITurnController>().AddStartTurnEvent(this);
            }

        }

        #endregion




        //----------------------------------------------------------------------------
        //                   StartTurn
        //----------------------------------------------------------------------------

        public IEnumerator StartTurn(IHasTurn currentIHasTurn)
        {
            CurrentIHasTurn = currentIHasTurn;
            ShowTurnUI();
            yield break;
        }

        public StartTurnType MyStartTurnType
        {
            get
            {
                return StartTurnType.UIEvent;
            }
        }



        //----------------------------------------------------------------------------
        //                   Moving
        //----------------------------------------------------------------------------

        #region Moving

        public void MoveButton()
        {
            MyPlayerMoveController.SelectMoveDestination(CurrentIHasTurn);
            SelectMove = true;

            ShowMoveUI();
        }



        public void BackFromMove()
        {
            ShowTurnUI();
            MyPlayerMoveController.BackFromMove();
        }


        public void IHexagonClicked(IHexagon ihexagon)
        {
            if (SelectMove)
            {
                MyPlayerMoveController.DoMove(ihexagon);
                SelectMove = false;
                NoUI();
            }
        }

        public void IHexagonHovered(IHexagon ihexagon)
        {
            if (SelectMove)
            {
                MyPlayerMoveController.HighlightPath(ihexagon);
            }
        }

        /// <summary>
        /// Should a hexagon click be interpreted as a move selection?
        /// </summary>
        public bool SelectMove = false;

        #endregion

        //----------------------------------------------------------------------------
        //                   End Turn
        //----------------------------------------------------------------------------

        public void EndTurnButton()
        {
            NoUI();
            CurrentIHasTurn.EndTurn();
        }

        //----------------------------------------------------------------------------
        //                   UI Show
        //----------------------------------------------------------------------------

        #region UI Show

        public void NoUI()
        {
            TurnUICanvas.SetActive(false);
            MoveUICanvas.SetActive(false);
        }

        public void ShowTurnUI()
        {
            TurnUICanvas.SetActive(true);
            MoveUICanvas.SetActive(false);
        }

        public void ShowMoveUI()
        {
            TurnUICanvas.SetActive(false);
            MoveUICanvas.SetActive(true);
        }

        #endregion

    }

}
