using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Turns;
using CriticalRole.Character;

namespace CriticalRole.Health
{
    public interface IHealthAlterer
    {
        void Initialise();

        int CheckHeal(IHasHealth hasHealth, int health);

        void AddMaxHealthAlteration(IHasHealth hasHealth, IMaxHealthAlteration maxHealthAlteration);

        int GetMaxHealth(IHasHealth hasHealth);

        int TakeDamage(IHasHealth hasHealth, int damage);

        int GetTempHP(IHasHealth hasHealth);

        void AddTempHP(IHasHealth hasHealth, int tempHP);
    }

    public class HealthAlterer : IHealthAlterer
    {
        public void Initialise()
        {
            _DependancyInjectionHasHealths();
        }

        private void _DependancyInjectionHasHealths()
        {
            HasTurnMarker[] hasTurnMarkers = GameObject.FindObjectsOfType<HasTurnMarker>();
            foreach(HasTurnMarker hasTurnMarker in hasTurnMarkers)
            {
                if(hasTurnMarker.gameObject.TryGetComponent(out IHasHealth hasHealth))
                {
                    hasHealth.RegisterHealthAlterer(this);
                }
            }
        }

        public int CheckHeal(IHasHealth hasHealth, int health)
        {
            int maxHealth = GetMaxHealth(hasHealth);
            if (health > maxHealth)
            {
                return maxHealth;
            }
            return health;
        }

        public int TakeDamage(IHasHealth hasHealth, int damage)
        {
            if(TempHitPointsDict.ContainsKey(hasHealth))
            {
                if(damage > TempHitPointsDict[hasHealth])
                {
                    damage -= TempHitPointsDict[hasHealth];
                    TempHitPointsDict[hasHealth] = 0;
                }
                else
                {
                    TempHitPointsDict[hasHealth] -= damage;
                    damage = 0;
                }
            }

            return damage;
        }

        public int GetMaxHealth(IHasHealth hasHealth)
        {
            int health = 0;
            foreach(IMaxHealthAlteration maxHealthAlteration in MaxHealthAlterationDict[hasHealth])
            {
                health = maxHealthAlteration.Alter(health);
            }
            return health;
        }

        public void AddMaxHealthAlteration(IHasHealth hasHealth, IMaxHealthAlteration maxHealthAlteration)
        {
            if(!MaxHealthAlterationDict.ContainsKey(hasHealth))
            {
                MaxHealthAlterationDict[hasHealth] = new List<IMaxHealthAlteration>();
            }

            MaxHealthAlterationDict[hasHealth].Add(maxHealthAlteration);
        }

        public int GetTempHP(IHasHealth hasHealth)
        {
            if(TempHitPointsDict.ContainsKey(hasHealth))
            {
                return TempHitPointsDict[hasHealth];
            }

            return 0;
        }

        public void AddTempHP(IHasHealth hasHealth, int tempHP)
        {
            TempHitPointsDict[hasHealth] = tempHP;
        }

        private Dictionary<IHasHealth, List<IMaxHealthAlteration>> _MaxHealthAlterationDict;
        public Dictionary<IHasHealth, List<IMaxHealthAlteration>> MaxHealthAlterationDict
        {
            get
            {
                if(_MaxHealthAlterationDict == null)
                {
                    _MaxHealthAlterationDict = new Dictionary<IHasHealth, List<IMaxHealthAlteration>>();
                }
                return _MaxHealthAlterationDict;
            }
        }

        private Dictionary<IHasHealth, int> _TempHitPointsDict;
        public Dictionary<IHasHealth, int> TempHitPointsDict
        {
            get
            {
                if(_TempHitPointsDict == null)
                {
                    _TempHitPointsDict = new Dictionary<IHasHealth, int>();
                }
                return _TempHitPointsDict;
            }
        }
    }

}
