using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Turns
{
    public enum StartTurnType
    {
        CameraEvent,
        UIEvent,
        HideEvent
    }

    public interface IStartTurnEvent
    {
        IEnumerator StartTurn(IHasTurn hasTurn);
        StartTurnType MyStartTurnType { get; }
    }

    public class StartTurnSort : IComparer<IStartTurnEvent>
    {
        public int Compare(IStartTurnEvent x, IStartTurnEvent y)
        {
            if(x.MyStartTurnType < y.MyStartTurnType)
            {
                return -1;
            }
            else if(x.MyStartTurnType > y.MyStartTurnType)
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
