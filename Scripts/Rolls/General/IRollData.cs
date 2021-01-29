using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Rolling
{
    public interface IRollData
    {
        void AddMod(int mod);

        void SetRoll(int roll);

        AdvantageSetter MyAdvantageSetter { get; }

        int PureRollValue { get; }

        int RollValue { get; }

        int Mods { get; }
    }
}

