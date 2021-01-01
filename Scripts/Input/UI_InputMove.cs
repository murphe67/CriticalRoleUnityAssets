using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Move;

namespace CriticalRole.UI
{
    public class UI_InputMove : MonoBehaviour, UI_InputSubclass
    {
        public UIManager MyUIManager;
        public IPlayerMoveController MyPlayerMoveController;

        public void Initialise(UIManager myUI_Input)
        {
            MyUIManager = myUI_Input;
            MyPlayerMoveController = new PlayerMoveController();
        }

        public void MoveButton()
        {
            MyPlayerMoveController.SelectMoveDestination(MyUIManager.CurrentIHasTurn);
            SelectMove = true;

            MyUIManager.ShowBackButton();
        }

        public void BackButton()
        {
            if(SelectMove)
            {
                SelectMove = false;
                MyUIManager.ShowTurnUI();
                MyPlayerMoveController.BackFromMove();
            }
        }


        public void IHexagonClicked(IHexagon hexagon)
        {
            if (SelectMove)
            {
                MyPlayerMoveController.DoMove(hexagon);
                SelectMove = false;
                MyUIManager.NoUI();
            }
        }

        public void IHexagonHovered(IHexagon hexagon)
        {
            if (SelectMove)
            {
                MyPlayerMoveController.HighlightPath(hexagon);
            }
        }

        public void IHexagonUnhovered(IHexagon hexagon)
        {

        }

        /// <summary>
        /// Should a hexagon click be interpreted as a move selection?
        /// </summary>
        public bool SelectMove = false;
    }
}
