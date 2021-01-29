using System;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Move
{
    public enum SpeedAlterationType
    {
        BaseSpeed
    }

    public interface ISpeedAlteration
    {
        SpeedAlterationType MySpeedAlterationType { get; }

        float Alter(float alterableSpeed);
    }


    public class SpeedAlterationSort : IComparer<ISpeedAlteration>
    {
        public int Compare(ISpeedAlteration x, ISpeedAlteration y)
        {
            if (x.MySpeedAlterationType < y.MySpeedAlterationType)
            {
                return -1;
            }
            if (x.MySpeedAlterationType > y.MySpeedAlterationType)
            {
                return 1;
            }
            else
            {
                //this is probably overkill
                //but it only returns equal if the alterations do the exact same thing

                //this specific calculation will only confuse very few alterations for each other
                //eg +200 and x2 will return equal from this.
                //and none of them should occur in practice

                int speed = 200;
                float speedA = x.Alter(speed);
                float speedB = y.Alter(speed);
                if (speedA < speedB)
                {
                    return -1;
                }
                else if (speedA > speedB)
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
}
