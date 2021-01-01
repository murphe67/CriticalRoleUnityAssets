using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController
{
    public PlayerAttackController()
    {
        MyAttackRanges = new AttackRanges();

        GameObject go = new GameObject("Player Attack GO");
        MyAttackManager = go.AddComponent<AttackManager>();
        MyAttackManager.Initialise(this);
    }

    AttackRanges MyAttackRanges;
    AttackManager MyAttackManager;
    HashSet<IHexagon> TargetsInRange;
    IHasTurn CurrentHasTurn;


    public void SelectAttackTarget(IHasTurn currentIHasTurn)
    {
        CurrentHasTurn = currentIHasTurn;
        TargetsInRange = MyAttackRanges.GetTargetsInRange(currentIHasTurn.MyHexContents.Location, 1);
        foreach (IHexagon target in TargetsInRange)
        {
            target.Interaction.ChangeColor(Color.green);
            target.Interaction.MakeSelectable();
        }
    }

    public void DoAttack(IHexagon hexagon)
    {
        foreach(IHexagon target in TargetsInRange)
        {
            target.Interaction.MakeUnselectable();
        }

        MyAttackManager.Attack(CurrentHasTurn, hexagon.Contents);
    }

    public void HighlightHex(IHexagon hexagon)
    {
        hexagon.Interaction.ChangeColor(DarkGreen);
    }

    public void UnhighlightHex(IHexagon hexagon)
    {
        hexagon.Interaction.ChangeColor(Color.green);
    }

    Color DarkGreen
    {
        get
        {
            return new Color(0, 0.2f, 0);
        }
    }

    public void BackFromAttack()
    {
        foreach(IHexagon target in TargetsInRange)
        {
            target.Interaction.MakeUnselectable();
        }
    }
}
