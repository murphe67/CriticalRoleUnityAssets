using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Map;
using CriticalRole.Turns;

namespace CriticalRole.Contents
{

    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------

    // This gives a object the abilty to occupy a hexagon, blocking entry to other
    // IHexContents

    /// <summary>
    /// 
    /// </summary>
    public interface IContents
    {
        void Initialise();

        Transform ContentTransform { get; }

        IHexagon Location { get; set; }

        IHasTurn MyHasTurn { get; }
    }
}
