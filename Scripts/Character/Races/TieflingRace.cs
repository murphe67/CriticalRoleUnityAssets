using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Move;

namespace CriticalRole.Character
{
    public class TieflingRace : MonoBehaviour, IRacialAbilities
    {
        public void Initialise()
        {
            IHasSpeed hasSpeed = GetComponent<IHasSpeed>();
            BaseSpeed baseSpeed = new BaseSpeed(8);
            hasSpeed.MyHasSpeedAlterer.AddAlteration(hasSpeed, baseSpeed);
        }
    }

}
