using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------
//                    Class Description
//----------------------------------------------------------------------------

// This gives a object the abilty to occupy a hexagon, blocking entry to other
// IHexContents

/// <summary>
/// 
/// </summary>
public interface IHexContents
{
    Transform ContentTransform { get; }

    IHexagon Location { get; set; }
}