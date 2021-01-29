using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Dependancy
{
    public enum DependantManagerType
    {
        MapGeneration,
        //Dependancy: the hexagons must be spawned
        UIManager,
        //Any feature dependant on an Alterer must initialise after the AlterationManager.
        AlterationManager,
        //Have all features that exists at game start add themselves to the AlterationManager
        CharacterInitialiser
    }

    public interface IDependantManager 
    {
        DependantManagerType MyDependantManagerType { get; }
        void Initialise();
    }

    public class DependantManagerSort : IComparer<IDependantManager>
    {
        public int Compare(IDependantManager x, IDependantManager y)
        {
            if(x.MyDependantManagerType > y.MyDependantManagerType)
            {
                return 1;
            }
            else if(x.MyDependantManagerType < y.MyDependantManagerType)
            {
                return -1;
            }
            return 0;
        }
    }
}

