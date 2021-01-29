using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Rolling
{
    public class AdvantageRoller
    {
        public int Roll(AdvantageSetter advantageSetter)
        {
            bool advantage = advantageSetter.Advantage;
            bool disadvantage = advantageSetter.Disadvantage;

            int a = UnityEngine.Random.Range(1, 21);
            int b = UnityEngine.Random.Range(1, 21);
            if (advantage && !disadvantage)
            {
                if(b > a)
                {
                    a = b;
                }
            }
            else if(disadvantage && !disadvantage)
            {
                if(b < a)
                {
                    a = b;
                }
            }

            return a;
        }
    }

}
