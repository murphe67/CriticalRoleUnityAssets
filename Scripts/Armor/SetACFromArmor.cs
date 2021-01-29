using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Attacking;
using CriticalRole.Character;

//----------------------------------------------------------------------------
//             Class Description
//----------------------------------------------------------------------------
//
// Calculate character AC based off their armour setup.
// Dependancy injects into the IsVictim

namespace CriticalRole.Armored
{
    public class SetACFromArmor : MonoBehaviour, IACSetter
    {
        public void Awake()
        {
            GetComponent<IIsVictim>().RegisterACSetter(this);
        }

        public void SetAC(IIsVictim isVictim, IACAlterer acAlterer)
        {
            Equipment equipment = gameObject.GetComponent<Equipment>();
            ArmorObject armorObject = equipment.MyArmor;
            int mod = armorObject.BaseAC;

            IHasStats hasStats = gameObject.GetComponent<HasStats>();
            int dexMod = hasStats.GetStatMod(StatsType.Dexterity);

            if (armorObject.HasMaxDexMod && (dexMod > armorObject.MaxDexMod))
            {
                dexMod = armorObject.MaxDexMod;
            }

            mod += dexMod;

            acAlterer.AddACMod(isVictim, mod);
        }
    }
}