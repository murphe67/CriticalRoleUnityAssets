using System.Collections;
using System.Collections.Generic;
using CriticalRole.Attacking;
using UnityEngine;
using CriticalRole.Narration;
using CriticalRole.Turns;

namespace CriticalRole.Death
{
    public class DeathSavesCanDie : MonoBehaviour, ICanDie
    {
        public IEnumerator TakeAttack(AttackRoll attackRoll)
        {
            Debug.Log("Death!");
            yield break;
        }

        public void RegisterNarrator(Narrator narrator)
        {
            MyNarrator = narrator;
        }
        Narrator MyNarrator;
        public void RegisterDeathManager(IDeathManager deathManager)
        {
            MyDeathManager = deathManager;
        }
        IDeathManager MyDeathManager;

        public void RegisterHasTurn(IHasTurn hasTurn)
        {
            MyHasTurn = hasTurn;
        }

        IHasTurn MyHasTurn;

        public void RegisterAttackManager(IAttackManager attackManager)
        {
            MyAttackManager = attackManager;
        }

        IAttackManager MyAttackManager;

       

    }

}

