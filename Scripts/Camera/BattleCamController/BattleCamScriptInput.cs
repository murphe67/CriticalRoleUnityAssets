using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.BattleCamera
{

    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------
    //
    // Moves the camera around based on 4 floats.
    // Fairly rudamentry but I didn't want to learn how the entire animation system 
    // works when this was so simple to implement

    public interface IBattleCamScriptInput
    {
        void SetInputs(float rotation, float forward, float right, float zoom);
    }


    public class BattleCamScriptInput : IBattleCamInput, IBattleCamScriptInput
    {
        public void SetInputs(float rotation, float forward, float right, float zoom)
        {
            RotationFloat = rotation;
            TranslateForwardFloat = forward;
            TranslateRightFloat = right;
            ZoomFloat = zoom;
        }

        public float RotationFloat = 0f;
        public float TranslateForwardFloat = 0f;
        public float TranslateRightFloat = 0f;
        public float ZoomFloat = 0f;

        public float RotationInput()
        {
            return RotationFloat;
        }

        public float TranslateForwardInput()
        {
            return TranslateForwardFloat;
        }

        public float TranslateRightInput()
        {
            return TranslateRightFloat;
        }

        public float ZoomInput()
        {
            return ZoomFloat;
        }
    }
}