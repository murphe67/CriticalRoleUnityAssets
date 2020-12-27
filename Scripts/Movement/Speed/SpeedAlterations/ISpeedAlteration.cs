using System;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedAlterationType
{
    BaseSpeed
}

public interface ISpeedAlteration
{
    SpeedAlterationSort MySpeedAlterationSort { get; }

    SpeedAlterationType MySpeedAlterationType { get; }

    int Alter(int alterableSpeed);

}


public class SpeedAlterationSort : IComparable
{
    public ISpeedAlteration SpeedAlteration;

    public SpeedAlterationSort(ISpeedAlteration speedAlteration)
    {
        SpeedAlteration = speedAlteration;
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        SpeedAlterationSort otherSort = obj as SpeedAlterationSort;
        if (otherSort != null)
            return SpeedAlterationSortCompare(this, otherSort);
        else
            throw new ArgumentException("Object is not a SpeedAlterationSort");
    }

    public int SpeedAlterationSortCompare(SpeedAlterationSort a, SpeedAlterationSort b)
    {
        if(a.SpeedAlteration.MySpeedAlterationType < b.SpeedAlteration.MySpeedAlterationType)
        {
            return -1;
        }
        if (a.SpeedAlteration.MySpeedAlterationType > b.SpeedAlteration.MySpeedAlterationType)
        {
            return 1;
        }
        else
        {
            int speed = 200;
            int speedA = a.SpeedAlteration.Alter(speed);
            int speedB = b.SpeedAlteration.Alter(speed);
            if(speedA < speedB)
            {
                return -1;
            }
            else if(speedA > speedB)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}