using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Move
{
    public class BaseSpeed : ISpeedAlteration
    {
        public int Speed;
        public SpeedAlterationType MySpeedAlterationType { get; set; }

        public BaseSpeed(int speed)
        {
            Speed = speed;
            MySpeedAlterationType = SpeedAlterationType.BaseSpeed;
        }

        public float Alter(float alterableSpeed)
        {
            return Speed;
        }
    }

}
