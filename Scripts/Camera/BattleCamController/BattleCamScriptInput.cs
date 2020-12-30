using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.BattleCamera
{

    public class BattleCamScriptInput : IBattleCamInput
    {
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