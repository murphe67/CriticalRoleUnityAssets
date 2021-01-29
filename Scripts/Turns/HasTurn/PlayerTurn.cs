using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.UI;
using CriticalRole.Contents;
using CriticalRole.Attacking;
using CriticalRole.Move;
using CriticalRole.Character;
using CriticalRole.Rolling;
using CriticalRole.Death;

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
    [RequireComponent(typeof(HasTurnHexContent))]
    [RequireComponent(typeof(HasSpeed))]
    [RequireComponent(typeof(HasAttack))]
    [RequireComponent(typeof(Equipment))]
    public class PlayerTurn : MonoBehaviour, IHasTurn
    {
        /// <summary>
        /// Set my turn controller- index in TurnList
        /// </summary>
        public int Index { get; set; }

        public string Name
        {
            get
            {
                return gameObject.name;
            }
        }

        //----------------------------------------------------------------------------
        //             Initialise
        //----------------------------------------------------------------------------
        public void Initialise(ITurnController turnController)
        {
            MyTurnController = turnController;
            MyHexContents = GetComponent<IContents>();
            MyHasSpeed = GetComponent<IHasSpeed>();
            MyHasAttack = GetComponent<IHasAttack>();
            MyHasStats = GetComponent<IHasStats>();
            MyIsVictim = GetComponent<IIsVictim>();

            _RollInitiative();

            GetComponent<ICanDie>().RegisterHasTurn(this);
        }

        /// <summary>
        /// Reference to contents as initiative order uses coords as fallback <para/>
        /// In perfectly matched initiative
        /// </summary>
        public IContents MyHexContents { get; set; }

        public IHasSpeed MyHasSpeed { get; set; }

        public IHasAttack MyHasAttack { get; set; }

        public IIsVictim MyIsVictim { get; set; }

        public IHasStats MyHasStats { get; set; }

        //----------------------------------------------------------------------------
        //             RollInitiative
        //----------------------------------------------------------------------------

        private void _RollInitiative()
        {
            AddDexModToInitiative dexModAlteration = new AddDexModToInitiative(MyHasStats.GetStatMod(StatsType.Dexterity));
            MyGeneralRoller.AddAlteration(RollType.Initiative, this, dexModAlteration);

            Initiative = new InitiativeRollData();
            MyGeneralRoller.Roll(RollType.Initiative, this, Initiative);
            
        }

        public IInitiativeRollData Initiative { get; set; }

        //----------------------------------------------------------------------------
        //             RegisterUIManager
        //----------------------------------------------------------------------------

        #region RegisterUIManager
        /// <summary>
        /// This isn't anything more complex than a set function <para/>
        /// However makes the reference injection more explicit
        /// </summary>
        public void RegisterUIManager(UIManager ui_input)
        {
            MyUIManager = ui_input;
        }

        public UIManager MyUIManager { get; private set; }

        #endregion



        //----------------------------------------------------------------------------
        //             RegisterGeneralRoller
        //----------------------------------------------------------------------------
        #region RegisterGeneralRoller

        public void RegisterGeneralRoller(IGeneralRoller generalRoller)
        {
            MyGeneralRoller = generalRoller;
        }

        IGeneralRoller MyGeneralRoller;

        #endregion



        //----------------------------------------------------------------------------
        //             Start Turn
        //----------------------------------------------------------------------------

        #region StartTurn
        /// <summary>
        /// For a PlayerTurn, nothing happens until the UI is interacted with <para/>
        /// So the only action taken is to inform the UI who's turn has started.
        /// </summary>
        public void StartTurn(ActionEnum action, BonusActionEnum bonusAction, ReactionEnum reaction)
        {
            MyUIManager.StartTurn(this);
            MyHasSpeed.ResetUsedMovement();
        }

        #endregion





        //----------------------------------------------------------------------------
        //             End Turn
        //----------------------------------------------------------------------------

        #region EndTurn

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

        #endregion



        //----------------------------------------------------------------------------
        //             End Move
        //----------------------------------------------------------------------------

        #region EndMove
        public void EndMove()
        {
            MyUIManager.ShowTurnUI();
        }

        #endregion

        public void EndAttack()
        {
            MyUIManager.ShowTurnUI();
        }

    }

}