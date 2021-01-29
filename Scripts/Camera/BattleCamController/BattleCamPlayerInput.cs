using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.BattleCamera
{

    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------
    //
    //  Takes input from the mouse and keyboard to move the camera
    //

    public class BattleCamPlayerInput : IBattleCamInput
    {
        /// <summary>
        /// When zoom input is requested, return mouse wheel delta
        /// </summary>
        /// <returns></returns>
        public float ZoomInput()
        {
            return -Input.mouseScrollDelta.y;
        }





        #region Rotation

        /// <summary>
        /// Needs to return a value between -1 and 1 for rotation speed <para/>
        /// </summary>
        /// <returns></returns>
        public float RotationInput()
        {
            _GetMouseDownPosition();

            float distance = Input.mousePosition.x - MouseDownPos;
            float factor = _CalcFactorFromDistance(distance);

            return Mathf.Clamp(factor, -1f, 1f);
        }

        #region Implementation

        #region Variables

        /// <summary>
        /// Which mouse button to use to rotate <para />
        /// Originally set to 2 (middle mouse button)
        /// </summary>
        public int MouseKey = 2;

        /// <summary>
        /// Stores the x coord of where the mouse was when the mouse wheel was pressed
        /// </summary>
        public float MouseDownPos = 0;

        /// <summary>
        /// How far the mouse must move before rotation begins <para />
        /// Originally set to 15
        /// </summary>
        public float RotationMinDelta = 15f;

        /// <summary>
        /// The slowest you can rotate <para/>
        /// Originally set to 25
        /// </summary>
        public float RotationDefaultSpeed = 0.1f;

        #endregion

        private void _GetMouseDownPosition()
        {
            if (Input.GetMouseButtonDown(MouseKey))
            {
                MouseDownPos = Input.mousePosition.x;
            }
        }

        private float _CalcFactorFromDistance(float distance)
        {
            float factor = 0;

            if (Input.GetMouseButton(MouseKey))
            {
                if (distance > RotationMinDelta)
                {
                    factor = RotationDefaultSpeed;
                    factor += (3 * distance / Camera.main.pixelWidth) * (1 - RotationDefaultSpeed);
                }
                else if (distance < -RotationMinDelta)
                {
                    factor = -RotationDefaultSpeed;
                    factor += (3 * distance / Camera.main.pixelWidth) * (1 - RotationDefaultSpeed);
                }
            }

            return factor;
        }


        #endregion
        
        #endregion






        public float TranslateForwardInput()
        {
            return Input.GetAxis("Vertical");
        }






        public float TranslateRightInput()
        {
            return Input.GetAxis("Horizontal");
        }

    }
}