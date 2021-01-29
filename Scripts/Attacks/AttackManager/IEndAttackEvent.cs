using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Turns;


namespace CriticalRole.Attacking
{
    public enum EndAttackEventType
    {
        CameraFocus,
        RemoveGameobject
    }

    public interface IEndAttackEvent
    {
        IEnumerator EndAttackEvent(IHasTurn hasTurn);
        EndAttackEventType MyEndAttackEventType { get; }
    }
}

