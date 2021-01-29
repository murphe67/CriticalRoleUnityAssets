using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Rolling
{
    public interface IInitiativeRollData : IDexterityRollData
    {

    }

    public class InitiativeRollData : IInitiativeRollData
    {
        public InitiativeRollData()
        {
            Mods = 0;
            MyAdvantageSetter = new AdvantageSetter();
        }

        public int RollValue { get; set; }

        public AdvantageSetter MyAdvantageSetter { get; set; }

        public void AddMod(int mod)
        {
            Mods += mod;
        }

        public int Mods { get; set; }

        public int PureRollValue { get; set; }

        public void SetRoll(int roll)
        {
            PureRollValue = roll;
            RollValue = roll + Mods;
        }
    }

}
