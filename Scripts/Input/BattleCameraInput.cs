using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------
//                      Interface Description
//----------------------------------------------------------------------------

// This interface is probably overkill  
// If I eventually have multiple ways to move the camera, it'll probably be a whole new class
// rather than remapping inputs
//
// but it makes the parameterisation of the input cleaner to have it through an interface

public interface IBattleCameraInput
{
    /// <summary>
    /// Generic zoom input, default: mouse wheel
    /// </summary>
    /// <returns></returns>
    float ZoomInput();

    /// <summary>
    /// Generic Rotation Input <para/>
    /// Default: hold mouse wheel down, and move right or left
    /// </summary>
    /// <returns></returns>
    float RotationInput();

    float TranslateForwardInput();

    float TranslateRightInput();
}

public class BattleCameraInput : IBattleCameraInput
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
                factor += distance * RotationDefaultSpeedIncrement;
            }
            else if (distance < -RotationMinDelta)
            {
                factor = -RotationDefaultSpeed;
                factor += distance * RotationDefaultSpeedIncrement;
            }
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
    public float RotationDefaultSpeed = 25f;

    /// <summary>
    /// How much the rotation speed should increase as the mouse moves further from when it started <para/>
    /// 
    /// Orignally set to 0.15
    /// </summary>
    public float RotationDefaultSpeedIncrement = 0.15f;

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
