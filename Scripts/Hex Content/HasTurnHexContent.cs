using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Map;
using CriticalRole.Turns;
using CriticalRole.Death;

namespace CriticalRole.Contents
{
    [RequireComponent(typeof(IContentMarker))]
    public class HasTurnHexContent : MonoBehaviour, IContents, IDeathEvent
    {
        public Transform ContentTransform
        {
            get
            {
                return gameObject.transform;
            }
        }

        public IHexagon Location { get; set; }

        public void Initialise()
        {
            MyHasTurn = GetComponent<IHasTurn>();

            DeathManagerMarker[] deathManagerMarkers = FindObjectsOfType<DeathManagerMarker>();
            foreach(DeathManagerMarker deathManagerMarker in deathManagerMarkers)
            {
                deathManagerMarker.gameObject.GetComponent<IDeathManager>().AddSingleDeathEvent(MyHasTurn, this);
            }

        }

        public IEnumerator ReactToDeath(IHasTurn hasTurn)
        {
            Location.Contents = null;
            yield break;
        }

        public IHasTurn MyHasTurn { get; set; }
    }
}

