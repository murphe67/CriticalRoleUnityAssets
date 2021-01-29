using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Attacking;
using CriticalRole.Turns;

//----------------------------------------------------------------------------
//             Class Description
//----------------------------------------------------------------------------
//
// This alterer is pretty boring. It matches the other alterations in terms
// of syntax, but you just add an integer mod to the AC.



namespace CriticalRole.Armored
{
    public interface IACAlterer
    {
        void Initialise();
        int GetAC(IIsVictim isVictim);

        void AddACMod(IIsVictim isVictim, int mod);
    }

    public class ACAlterer : IACAlterer
    {

        //----------------------------------------------------------------------------
        //             Initialise
        //----------------------------------------------------------------------------

        #region Initialise

        /// <summary>
        /// Dependancy Inject to IsVictims
        /// </summary>
        public void Initialise()
        {
            _DependancyInjectionToIsVictims();   
        }

        private Dictionary<IIsVictim, int> _ACDict;
        public Dictionary<IIsVictim, int> ACDict
        {
            get
            {
                if(_ACDict == null)
                {
                    _ACDict = new Dictionary<IIsVictim, int>();
                }
                return _ACDict;
            }
        }

        private void _DependancyInjectionToIsVictims()
        {
            HasTurnMarker[] hasTurnMarkers = GameObject.FindObjectsOfType<HasTurnMarker>();
            foreach(HasTurnMarker hasTurnMarker in hasTurnMarkers)
            {
                hasTurnMarker.gameObject.GetComponent<IIsVictim>().RegisterACAlterer(this);
            }
        }

        #endregion




        //----------------------------------------------------------------------------
        //             GetAc
        //----------------------------------------------------------------------------

        #region GetAC
        public int GetAC(IIsVictim isVictim)
        {
            return ACDict[isVictim];
        }

        #endregion



        //----------------------------------------------------------------------------
        //             Add ACMod
        //----------------------------------------------------------------------------

        #region AddACMod
        public void AddACMod(IIsVictim isVictim, int mod)
        {
            if(!ACDict.ContainsKey(isVictim))
            {
                ACDict[isVictim] = mod;
            }
            else
            {
                ACDict[isVictim] += mod;
            }
        }
        #endregion


    }

}
