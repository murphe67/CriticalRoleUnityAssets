using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Character;
using CriticalRole.Attacking;

//----------------------------------------------------------------------------
//            Class Description
//----------------------------------------------------------------------------
//
// This is for setting the base AC of a character.
//
// Two current implementations:
// Calculate from armour
// or set arbitrarily

namespace CriticalRole.Armored
{
    public interface IACSetter
    {
        void SetAC(IIsVictim isVictim, IACAlterer acAlterer);
    }

}
