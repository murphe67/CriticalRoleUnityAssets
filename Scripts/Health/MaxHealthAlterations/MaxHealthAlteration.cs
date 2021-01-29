using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Health
{
    public interface IMaxHealthAlteration
    {
        int Alter(int mod);
    }

}
