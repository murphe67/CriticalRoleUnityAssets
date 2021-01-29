using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------
//                    Class Description
//----------------------------------------------------------------------------
//
// This is for any effect that alters an attack roll.
// Must be hooked to either an attacker or a victim.

namespace CriticalRole.Attacking
{
    public interface IAttackDataAlteration
    {
        void Alter(IAttackData attackData);
    }

}
