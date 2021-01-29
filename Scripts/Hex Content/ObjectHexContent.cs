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

    // I have a similiar confusion with this class as the interface it implements
    //
    // This was originally supposed to store the methods a character would use to move
    //
    // But that has all been abstracted up to the PlayerMoveController, which all the
    // player controlled characters will share
    //
    // so instead its an exact replica of the interface
    //
    // Thinking it might come into play more once the objects start interacting
    // with each other, might be the access point for shoving/attacking
    //
    // which will obviously be interpretted differently for objects vs characters

    [RequireComponent(typeof(IContentMarker))]
    public class ObjectHexContent : MonoBehaviour, IContents
    {
        public void Initialise()
        {

        }

        public Transform ContentTransform
        {
            get
            {
                return gameObject.transform;
            }
        }

        public IHexagon Location { get; set; }

        public IHasTurn MyHasTurn
        {
            get
            {
                return null;
            }
        }
    }
}

