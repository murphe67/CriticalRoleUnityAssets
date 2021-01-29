using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Turns
{
    public interface IActionAlteration
    {
        void Alter(ITurnStarter turnStarter);
    }
}

