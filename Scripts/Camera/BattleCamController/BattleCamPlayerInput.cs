using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.BattleCamera
{
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

        public float RotationInput()
        {
            if (Input.GetMouseButtonDown(MouseKey))
            {
                MousePos = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(MouseKey))
            {
                float distance = (Input.mousePosition.x - MousePos);
                float factor = 0f;
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
                factor = Mathf.Clamp(factor, -1f, 1f); ;
                return factor;
            }
            return 0f;
        }

        /// <summary>
        /// Which mouse button to use to rotate <para />
        /// Originally set to 2 (middle mouse button)
        /// </summary>
        public int MouseKey = 2;

        /// <summary>
        /// Stores the x coord of where the mouse was when the mouse wheel was pressed
        /// </summary>
        public float MousePos = 0;

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