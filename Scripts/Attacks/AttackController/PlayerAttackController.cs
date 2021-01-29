using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Map;
using CriticalRole.Turns;

//----------------------------------------------------------------------------
//                    Class Description
//----------------------------------------------------------------------------
//
//  Based on the Player Move Controller
//
//  Implements the UI menu attack button functions
//
// Now that its implemented its pretty much only a UI class?
//
// Like in theory it where the multi purpose attack base class gets called?
//
// Possibly once reacting to attacks and adding stuff like divine smite will run
// through here


namespace CriticalRole.Attacking
{

    //
    // Interface currently only exposes functions
    // Single implementation
    //
    public interface IPlayerAttackController : IAttackController
    {
        /// <summary>
        /// Called by the UI when the player clicks attack. <para/>
        /// Sets up systems necessary for the player to see who they can attack and make that decision.
        /// </summary>
        void SelectAttackTarget(IHasTurn currentIHasTurn);

        /// <summary>
        /// Called when the player has pressed back after pressing the attack button
        /// </summary>
        void BackFromAttack();

        /// <summary>
        /// Called by the UI when the player decides who to attack. <para/>
        /// </summary>
        void DoAttack(IHexagon hexagon);

        /// <summary>
        /// Called when the mouse hovers over a selectable hex
        /// </summary>
        void MouseHover(IHexagon hexagon);

        /// <summary>
        /// Called when a mouse is no longer over a selectable hex
        /// </summary>
        void MouseUnhover(IHexagon hexagon);
    }


    [RequireComponent(typeof(IAttackControllerMarker))]
    public class PlayerAttackController : MonoBehaviour, IPlayerAttackController
    {


        //----------------------------------------------------------------------------
        //                   Initialise
        //----------------------------------------------------------------------------

        #region Initialise
        /// <summary>
        /// Inherited from IAttackController. <para/>
        /// Includes dependancy injection from AttackManager
        /// </summary>
        public void Initialise(IAttackManager attackManager)
        {
            MyAttackManager = attackManager;
            MyAttackRanges = new AttackRanges();
        }

        /// <summary>
        /// Boilerplate class for getting targets from the map
        /// </summary>
        AttackRanges MyAttackRanges;

        /// <summary>
        /// Boilerplate class that implements all attacks
        /// </summary>
        IAttackManager MyAttackManager;

        #endregion





        //----------------------------------------------------------------------------
        //                   SelectAttackTarget
        //----------------------------------------------------------------------------

        #region SelectAttackTarget

        /// <summary>
        /// Called when the move button is pressed. <para/>
        /// Updates the CurrentHasMove, then makes all attackable hexes in range clickable and green.
        /// </summary>
        public void SelectAttackTarget(IHasTurn currentIHasTurn)
        {
            CurrentHasTurn = currentIHasTurn;
            TargetsInRange = MyAttackRanges.GetTargetsInRange(currentIHasTurn.MyHexContents.Location, 1);
            foreach (IHexagon target in TargetsInRange)
            {
                target.Interaction.ChangeColor(ClickableColor);
                target.Interaction.MakeSelectable();
            }
        }

        /// <summary>
        /// Set by SelectAttackTarget <para/>
        /// Is passed to the AttackManager as the attacker by DoAttack <para/>
        /// </summary>
        IHasTurn CurrentHasTurn;

        /// <summary>
        /// All targets within the attack range of CurrentHasTurn. <para/>
        /// Set by SelectAttackTarget.
        /// </summary>
        HashSet<IHexagon> TargetsInRange;



        /// <summary>
        /// Attackable hex color. <para/>
        /// SelectAttackTarget sets TargetsInRange to this color. <para/>
        /// They are reset when the back button is pressed or an attack happens.
        /// </summary>
        Color ClickableColor
        {
            get
            {
                return Color.green;
            }
        }

        #endregion





        //----------------------------------------------------------------------------
        //                  BackFromAttack
        //----------------------------------------------------------------------------

        #region BackFromAttack
        /// <summary>
        /// Makes the hexes unselectable
        /// </summary>
        public void BackFromAttack()
        {
            foreach (IHexagon target in TargetsInRange)
            {
                target.Interaction.MakeUnselectable();
            }
        }

        #endregion





        //----------------------------------------------------------------------------
        //                   DoAttack
        //----------------------------------------------------------------------------

        #region DoAttack

        /// <summary>
        /// Make all the attackable hexes unselectable again. <para/>
        /// Then passes the CurrentHasTurn and the clicked hex to the AttackManager
        /// </summary>
        public void DoAttack(IHexagon hexagon)
        {
            foreach (IHexagon target in TargetsInRange)
            {
                target.Interaction.MakeUnselectable();
            }
            MyAttackManager.Attack(CurrentHasTurn, hexagon.Contents);
        }

        #endregion





        //----------------------------------------------------------------------------
        //                   Mouse Hovering
        //----------------------------------------------------------------------------

        #region Mouse Hovering

        /// <summary>
        /// Turn the hexes dark green when the mouse is over them
        /// </summary>
        public void MouseHover(IHexagon hexagon)
        {
            hexagon.Interaction.ChangeColor(HoverColor);
        }

        /// <summary>
        /// Turn the hexes light green. <para/>
        /// Which is the color they normally are when not hovered.
        /// </summary>
        public void MouseUnhover(IHexagon hexagon)
        {
            hexagon.Interaction.ChangeColor(ClickableColor);
        }

        /// <summary>
        /// HighlightHover sets the hex to this color when called <para/>
        /// Happens when the mouse is over the hexagon
        /// </summary>
        Color HoverColor
        {
            get
            {
                return new Color(0, 0.2f, 0);
            }
        }


        #endregion





    }

}