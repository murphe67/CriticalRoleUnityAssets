using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vec3IntExt
{
    public static int Vec3IntCompare(Vector3Int a, Vector3Int b)
    {
        if (a.x < b.x)
        {
            return -1;
        }
        else if (b.x < a.x)
        {
            return 1;
        }
        else if (a.y < b.y)
        {
            return -1;
        }
        else if (b.y < a.y)
        {
            return 1;
        }
        else if (a.z < b.z)
        {
            return -1;
        }
        else if (b.z < a.z)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
