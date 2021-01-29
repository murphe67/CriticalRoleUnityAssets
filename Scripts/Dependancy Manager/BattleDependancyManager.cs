using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Turns;


namespace CriticalRole.Dependancy
{

    //----------------------------------------------------------------------------
    //             Class Description
    //----------------------------------------------------------------------------
    //
    // Any system which is reliant on an different independant system should register
    // itself to the Dependancy Manager to allow them to initialise in the correct order
    //
    // This is only to be used as a last resort- if the dependant system can be initialised
    // by the other, it should
    //
    // But for core systems, they should not be aware of each other in that way


    public interface IBattleDependancyManager
    {
        void Register(IDependantManager manager);
        void RegisterTurnController(ITurnController turnController);
    }   

    [RequireComponent(typeof(DependancyManagerMarker))]
    public class BattleDependancyManager : MonoBehaviour, IBattleDependancyManager
    {
        //----------------------------------------------------------------------------
        //             Registration
        //----------------------------------------------------------------------------

        #region Registration

        /// <summary>
        /// Called by MapGeneration to register itself as THE MapGeneration <para />
        /// If there are two, the first is completely ignored
        /// </summary>
        public void Register(IDependantManager manager)
        {
            MyDependantManagers.Add(manager);
        }

        public List<IDependantManager> _MyDependantManagers;
        public List<IDependantManager> MyDependantManagers
        {
            get
            {
                if(_MyDependantManagers == null)
                {
                    _MyDependantManagers = new List<IDependantManager>();
                }
                return _MyDependantManagers;
            }
        }

        public void RegisterTurnController(ITurnController turnController)
        {
            MyTurnController = turnController;
        }

        public ITurnController MyTurnController;
            
        #endregion




        //----------------------------------------------------------------------------
        //             Initialisation
        //----------------------------------------------------------------------------


        #region Initialisation

        private void Start()
        {
            MyDependantManagers.Sort(new DependantManagerSort());
            foreach(IDependantManager dependantManager in MyDependantManagers)
            {
                dependantManager.Initialise();
            }

            MyTurnController.BeginGame();
        }



        #endregion




        //----------------------------------------------------------------------------
        //             Singleton Assertion
        //----------------------------------------------------------------------------

        #region Singleton Assertion

        // The dependancy manager is the only 'singleton-esque' manager
        // If you have more than one of the others, one is completely ignored
        //
        // But multiple dependancy managers break the registration process
        private void Awake()
        {
            DependancyManagerMarker[] dependancyManagers = FindObjectsOfType<DependancyManagerMarker>();
            Debug.Assert(dependancyManagers.Length == 1);
        }

        #endregion
    }

}
