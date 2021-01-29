using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Health
{
    public class BaseHealth : IMaxHealthAlteration
    {
        public BaseHealth(int health)
        {
            MyHealth = health;
        }

        int MyHealth;

        public int Alter(int mod)
        {
            return (mod + MyHealth);
        }
    }
}


