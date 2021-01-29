using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthData 
{
    public int MaxHealth;
    public int MaxHealableHealth;

    public HealthData(int health)
    {
        MaxHealth = health;
        MaxHealableHealth = health;
    }

    public void ModMaxHealth(int mod)
    {
        MaxHealableHealth += mod;
        MaxHealth += mod;
    }

    public void AddTempHP(int tempHP)
    {
        MaxHealableHealth = MaxHealth + tempHP;
    }
}
