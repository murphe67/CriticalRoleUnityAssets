using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Turns;

namespace CriticalRole.Death
{
    public interface IDeathEvent
    {
        IEnumerator ReactToDeath(IHasTurn hasTurn);
    }
}
