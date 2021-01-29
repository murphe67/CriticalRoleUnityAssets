using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Rolling
{
    public class AddDexModToInitiative : IRollDataAlteration
    {
        public AddDexModToInitiative(int dexMod)
        {
            MyDexMod = dexMod;
        }
        int MyDexMod;

        public void Alter(IRollData rollData)
        {
            rollData.AddMod(MyDexMod);
        }
    }

}

