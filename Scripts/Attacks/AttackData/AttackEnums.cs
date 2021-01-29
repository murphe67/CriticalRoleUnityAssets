using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Attacking
{
    /// <summary>
    /// Is this a Melee or Ranged attack?
    /// </summary>
    public enum AttackRangeType
    {
        Melee,
        Ranged
    }

    /// <summary>
    /// Is this a weapon or spell attack?
    /// </summary>
    public enum AttackType
    {
        Weapon,
        Spell
    }

    /// <summary>
    /// What core stat does this attack rely on? <para/>
    /// [I'm specifically thinking about Enlarge here, which I'm planning to homebrew] <para/>
    /// [But it would be unwise to presume this does not factor into any other spell effect]
    /// </summary>
    public enum AttackStat
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    }
}
