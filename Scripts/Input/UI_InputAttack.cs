using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Attacking;
using CriticalRole.Map;


namespace CriticalRole.UI
{
    [RequireComponent(typeof(PlayerAttackController))]
    public class UI_InputAttack : MonoBehaviour, UI_InputSubclass
    {
        public UIManager MyUIManager;

        IPlayerAttackController MyPlayerAttackController;
        public bool SelectAttack = false;

        public void Initialise(UIManager myUI_Input)
        {
            MyUIManager = myUI_Input;
            MyPlayerAttackController = GetComponent<PlayerAttackController>();
        }


        public void AttackButton()
        {
            SelectAttack = true;
            MyPlayerAttackController.SelectAttackTarget(MyUIManager.CurrentIHasTurn);
            MyUIManager.ShowBackButton();
        }

        public void IHexagonClicked(IHexagon hexagon)
        {
            if(SelectAttack)
            {
                SelectAttack = false;
                MyPlayerAttackController.DoAttack(hexagon);
                MyUIManager.NoUI();
            }
            
        }

        public void IHexagonHovered(IHexagon hexagon)
        {
            if(SelectAttack)
            {
                MyPlayerAttackController.MouseHover(hexagon);
            }
        }

        public void IHexagonUnhovered(IHexagon hexagon)
        {
            if(SelectAttack)
            {
                MyPlayerAttackController.MouseUnhover(hexagon);
            }
        }

        

        public void BackButton()
        {
            if(SelectAttack)
            {
                SelectAttack = false;
                MyUIManager.ShowTurnUI();
                MyPlayerAttackController.BackFromAttack();
            }
        }
    }

}
