using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.BattleCamera
{
    //----------------------------------------------------------------------------
    //                      Interface Description
    //----------------------------------------------------------------------------

    // Interface is implemented twice
    //
    // First for player input
    //
    // Second for control of camera through script

    public interface IBattleCamInput
    {

        float ZoomInput();

        float RotationInput();

        float TranslateForwardInput();

        float TranslateRightInput();
    }
}

