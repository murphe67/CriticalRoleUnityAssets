using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Character;


namespace CriticalRole.Health
{
    public interface IHasHealth
    {
        void Initialise();
        void TakeDamage(int damage);
        void Heal(int healing);

        int GetMaxHealth();

        int GetHealth();

        void RegisterHealthAlterer(IHealthAlterer healthAlterer);
    }

    public class HasHealth : MonoBehaviour, IHasHealth
    {
        public void RegisterHealthAlterer(IHealthAlterer healthAlterer)
        {
            MyHealthAlterer = healthAlterer;
        }

        IHealthAlterer MyHealthAlterer;

        [HideInInspector]
        public int MyHealth;

        public void Initialise()
        {
            int maxHealth = GetComponent<IHasStats>().MaxHealth;
            if(maxHealth == 0)
            {
                Debug.LogError("Health not set in inspector");
            }
            BaseHealth baseHealth = new BaseHealth(maxHealth);

            MyHealthAlterer.AddMaxHealthAlteration(this, baseHealth);

            Heal(maxHealth);
        }


        public void Heal(int healing)
        {
            MyHealth += healing;
            MyHealth = MyHealthAlterer.CheckHeal(this, MyHealth);
        }

        public void TakeDamage(int damage)
        {
            MyHealth -= MyHealthAlterer.TakeDamage(this, damage);
            MyHealth = Mathf.Max(0, MyHealth);
        }

        public int GetMaxHealth()
        {
            return MyHealthAlterer.GetMaxHealth(this);
        }

        public int GetHealth()
        {
            return (MyHealth + MyHealthAlterer.GetTempHP(this));
        }
    }

}
