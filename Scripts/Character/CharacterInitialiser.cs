using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Dependancy;
using CriticalRole.Turns;
using CriticalRole.Health;
using CriticalRole.Attacking;


namespace  CriticalRole.Character
{
    public class CharacterInitialiser : MonoBehaviour, IDependantManager
    {
        public DependantManagerType MyDependantManagerType
        {
            get
            {
                return DependantManagerType.CharacterInitialiser;
            }
        }

        public void Awake()
        {
            IBattleDependancyManager[] battleDependancyManagers = FindObjectsOfType<BattleDependancyManager>();
            foreach (IBattleDependancyManager battleDependancyManager in battleDependancyManagers)
            {
                battleDependancyManager.Register(this);
            }
        }

        public void Initialise()
        {
            HasTurnMarker[] hasTurnMarkers = FindObjectsOfType<HasTurnMarker>();
            foreach(HasTurnMarker hasTurnMarker in hasTurnMarkers)
            {
                GameObject go = hasTurnMarker.gameObject;
                go.GetComponent<IHasHealth>().Initialise();
                go.GetComponent<IRacialAbilities>().Initialise();
                go.GetComponent<IClassAbilities>().Initialise();
                go.GetComponent<IHasAttack>().Initialise();
                go.GetComponent<IIsVictim>().Initialise();
            }
        }
    }
}


