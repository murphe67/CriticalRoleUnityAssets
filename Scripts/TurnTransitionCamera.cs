using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTransitionCamera : MonoBehaviour
{
    public IEnumerator TurnTransition(IHasTurn hasTurn)
    {
        yield return new WaitForSeconds(2);
    }
}
