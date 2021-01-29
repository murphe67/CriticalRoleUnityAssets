using System.Collections;
using System.Collections.Generic;
using CriticalRole.Attacking;
using UnityEngine;

//----------------------------------------------------------------------------
//             Class Description
//----------------------------------------------------------------------------
//
// ACSetter implementation. Ignore equipment setup, and just use the value
// set in the inspector.

namespace CriticalRole.Armored
{
    public class SetACInInspector : MonoBehaviour, IACSetter
    {
        public int AC;

        public void SetAC(IIsVictim isVictim, IACAlterer acAlterer)
        {
            if(AC == 0)
            {
                Debug.LogError("AC was not set in Inspector");
            }

            acAlterer.AddACMod(isVictim, AC);
        }

        public void Awake()
        {
            GetComponent<IIsVictim>().RegisterACSetter(this);
        }
    }
}
