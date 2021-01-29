using System.Collections;
using System.Collections.Generic;
using CriticalRole.Attacking;
using CriticalRole.Narration;
using CriticalRole.Turns;
using UnityEngine;

namespace CriticalRole.Death
{
    public class InstaDie : MonoBehaviour, ICanDie
    {
        public void RegisterHasTurn(IHasTurn hasTurn)
        {
            MyHasTurn = hasTurn;
        }

        IHasTurn MyHasTurn;

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

        public void RegisterAttackManager(IAttackManager attackManager)
        {
            MyAttackManager = attackManager;
        }

        IAttackManager MyAttackManager;

        public IEnumerator TakeAttack(AttackRoll attackRoll)
        {
            yield return StartCoroutine(MyNarrator.HasTurnDeath(MyHasTurn));
            yield return StartCoroutine(MyDeathManager.Die(MyHasTurn));
        }
    }

}
