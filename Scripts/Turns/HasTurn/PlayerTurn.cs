using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.UI;

namespace CriticalRole.Turns
{

    //----------------------------------------------------------------------------
    //             Class Description
    //----------------------------------------------------------------------------

    // Standard implementation following from IHasTurn
    // Stores a reference to the TurnController, generates a totally random initiative
    // and dexterity to sort it in the TurnController Initiative order
    // 


    [RequireComponent(typeof(HasTurnMarker))]
    [RequireComponent(typeof(BaseHexContent))]
    [RequireComponent(typeof(HasSpeed))]
    public class PlayerTurn : MonoBehaviour, IHasTurn
    {
        //----------------------------------------------------------------------------
        //             Initialise
        //----------------------------------------------------------------------------
        public void Initialise(ITurnController turnController)
        {
            MyTurnController = turnController;
            MyHexContents = GetComponent<IHexContents>();
            MyHasSpeed = GetComponent<IHasSpeed>();

            TurnSort = new BaseTurnSort(Random.Range(0, 10), Random.Range(0, 10), MyHexContents.Location.CoOrds, this);
        }


        /// <summary>
        /// All IHasTurn should generate a TurnSort so it can be correctly placed in the initiative order
        /// </summary>
        public ITurnSort TurnSort { get; set; }

        /// <summary>
        /// Reference to contents as initiative order uses coords as fallback <para/>
        /// In perfectly matched initiative
        /// </summary>
        public IHexContents MyHexContents { get; set; }

        public IHasSpeed MyHasSpeed { get; set; }

        //----------------------------------------------------------------------------
        //             UI_Input reference
        //----------------------------------------------------------------------------

        /// <summary>
        /// This isn't anything more complex than a set function <para/>
        /// However makes the reference injection more explicit
        /// </summary>
        public void SetUI_InputReference(UI_Input ui_input)
        {
            MyUI_Input = ui_input;
        }

        public UI_Input MyUI_Input { get; private set; }

        //----------------------------------------------------------------------------
        //             Start Turn
        //----------------------------------------------------------------------------

        /// <summary>
        /// For a PlayerTurn, nothing happens until the UI is interacted with <para/>
        /// So the only action taken is to inform the UI who's turn has started.
        /// </summary>
        public void StartTurn()
        {
            MyUI_Input.StartTurn(this);
            Debug.Log("Start Turn: " + gameObject.name);
            MyHasSpeed.RefreshMovement();
        }




        //----------------------------------------------------------------------------
        //             End Turn
        //----------------------------------------------------------------------------

        /// <summary>
        /// End turn and pass control back to the turn controller
        /// </summary>
        public void EndTurn()
        {
            MyTurnController.TurnChangeover();
        }

        /// <summary>
        /// reference to the turn controller so that the PlayerTurn can indicate turn is finished
        /// </summary>
        public ITurnController MyTurnController;

        //----------------------------------------------------------------------------
        //             End Move
        //----------------------------------------------------------------------------
        public void EndMove()
        {
            MyUI_Input.ShowTurnUI();
        }

    }

}