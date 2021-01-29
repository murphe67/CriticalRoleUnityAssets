using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Move
{

    //----------------------------------------------------------------------------
    //              Class Description
    //----------------------------------------------------------------------------
    //
    // IHasSpeed provides the hook for registering speed alterations.
    // It also provides the access point for changing and using the amount of movement
    // actually left this turn


    /// <summary>
    /// Moveable character's current movement speed in tiles <para/>
    /// Grappling/Spells add a SpeedAlteration to influence this
    /// </summary>
    public interface IHasSpeed
    {
        void RegisterHasSpeedAlterer(IHasSpeedAlterer hasSpeedAlterer);

        /// <summary>
        /// Get the final result of this character's movement speed <para/>
        /// After all effects take place
        /// </summary>
        int CurrentMovement();

        /// <summary>
        /// Reduces the amount of movement left by movementCost
        /// <param name="movementCost"></param>
        void UseMove(int movementCost);

        /// <summary>
        /// Resets CurrentMovement to the HasSpeeds total movement per turn
        /// </summary>
        void ResetUsedMovement();

        IHasSpeedAlterer MyHasSpeedAlterer { get; set; }
    }

    public class HasSpeed : MonoBehaviour, IHasSpeed
    {
        //----------------------------------------------------------------------------
        //             RegisterSpeedAlterer
        //----------------------------------------------------------------------------

        public void RegisterHasSpeedAlterer(IHasSpeedAlterer hasSpeedAlterer)
        {
            MyHasSpeedAlterer = hasSpeedAlterer;
        }

        public IHasSpeedAlterer MyHasSpeedAlterer { get; set; }


        //----------------------------------------------------------------------------
        //            FinalSpeed
        //----------------------------------------------------------------------------

        /// <summary>
        /// Get the final result of this character's movement speed <para/>
        /// after all effects take place <para/>
        /// (Hopefully) applies all effects reversibly and in correct order <para/>
        /// Is the amount of moving left, rather than the final total movement
        /// </summary>
        public int CurrentMovement()
        {
            return MyHasSpeedAlterer.GetTotalMoveSpeed(this) - UsedMovement;
        }

        [HideInInspector]
        public int UsedMovement;

        public void UseMove(int movementCost)
        {
            UsedMovement += movementCost;
        }

        public void ResetUsedMovement()
        {
            UsedMovement = 0;
        }


    }
}
