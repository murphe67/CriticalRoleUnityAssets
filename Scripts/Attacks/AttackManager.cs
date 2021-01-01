using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public void Initialise(PlayerAttackController attackController)
    {

    }

    public void Attack(IHasTurn attacker, IContents victim)
    {
        MyAttacker = attacker;
        Debug.Log("Attacking " + victim.ContentTransform.gameObject.name);
        StartCoroutine(AttackCoroutine());
    }

    IHasTurn MyAttacker;

    public IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1f);
        MyAttacker.EndAttack();
    }
}
