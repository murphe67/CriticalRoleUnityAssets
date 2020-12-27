using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpeed : ISpeedAlteration
{
    public int Speed;
    public SpeedAlterationSort MySpeedAlterationSort { get; set; }
    public SpeedAlterationType MySpeedAlterationType { get; set; }

    public BaseSpeed(int speed)
    {
        Speed = speed;
        MySpeedAlterationType = SpeedAlterationType.BaseSpeed;
        MySpeedAlterationSort = new SpeedAlterationSort(this);
    }

    public int Alter(int alterableSpeed)
    {
        return Speed;
    }
}
