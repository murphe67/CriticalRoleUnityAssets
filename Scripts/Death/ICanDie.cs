using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Attacking;
using CriticalRole.Narration;
using CriticalRole.Turns;

namespace CriticalRole.Death
{
    public interface ICanDie
    {
        IEnumerator TakeAttack(AttackRoll attackRoll);

        void RegisterNarrator(Narrator narrator);

        void RegisterDeathManager(IDeathManager deathManager);

        void RegisterHasTurn(IHasTurn hasTurn);

        void RegisterAttackManager(IAttackManager attackManager);
    }
}


