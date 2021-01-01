using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Turns;

//----------------------------------------------------------------------------
//              Class Description
//----------------------------------------------------------------------------
//
// All camera based events are controlled by the BattleCamManager
//
// It currently manages 2 BattleCamControllers, each of which controls a single camera
//
// One of the BattleCamControllers is the players, while the other is 
// for programmed camera motion

namespace CriticalRole.BattleCamera
{
    public interface IBattleCamManager
    {
        void Initialise();
    }


    public class BattleCamManager : MonoBehaviour, IBattleCamManager, IStartTurnEvent
    {
        //----------------------------------------------------------------------------
        //             Inspector Variables
        //----------------------------------------------------------------------------
        #region Inspector variables
        /// <summary>
        /// Camera Prefab, allowing setup of raycasting and camera settings
        /// </summary>
        [Header("Set in Inspector")]
        [Tooltip("Set up camera settings and raycasting in prefab, then assign here")]
        public GameObject CameraPrefab;

        /// <summary>
        /// How high above the ground the focus is <para />
        /// Originally set to 0.8f
        /// </summary>
        [Tooltip("Default value: 0.8")]
        public float FocusHeight = 0.8f;


        public Animator Crossfade;

        #endregion



        //----------------------------------------------------------------------------
        //             Register
        //----------------------------------------------------------------------------

        #region Register
        private void Awake()
        {
            _RegisterWithDependancyManager();
        }



        #region Implementation
        private void _RegisterWithDependancyManager()
        {
            GameObject dependancyGO = FindObjectOfType<DependancyManagerMarker>().gameObject;
            IBattleDependancyManager dependancyManager = dependancyGO.GetComponent<IBattleDependancyManager>();
            dependancyManager.RegisterCameraManager(this);
        }

        #endregion

        #endregion




        //----------------------------------------------------------------------------
        //             Initialise
        //----------------------------------------------------------------------------

        #region Initialise

        public void Initialise()
        {
            _AddToStartTurnEventManager();
            _SpawnPlayerCamera();
            _SpawnScriptCamera();

            _GetHideables();
        }

        public IBattleCamController PlayerCamController;
        public IBattleCamController ScriptCamController;
        public BattleCamScriptInput ScriptCamInput;
        public List<Hideable> Hideables;

        #region Implementation
        private void _AddToStartTurnEventManager()
        {
            TurnControllerMarker[] turnControllerMarkers = FindObjectsOfType<TurnControllerMarker>();
            foreach (TurnControllerMarker turnControllerMarker in turnControllerMarkers)
            {
                TurnController turnController = turnControllerMarker.GetComponent<TurnController>();
                turnController.AddStartTurnEvent(this);
            }
        }

        private void _SpawnPlayerCamera()
        {
            GameObject PlayerCameraGO = new GameObject("Player Camera");
            IBattleCameraSpawner PlayerCameraSpawner = PlayerCameraGO.AddComponent<BattleCamSpawner>();
            PlayerCamController = PlayerCameraSpawner.SpawnCamera(FocusHeight, CameraPrefab, BattleCameraInputType.PlayerControlled);
            PlayerCamController.SetActive(false);
        }

        private void _SpawnScriptCamera()
        {
            GameObject ScriptCameraGO = new GameObject("Script Camera");
            IBattleCameraSpawner ScriptCameraSpawner = ScriptCameraGO.AddComponent<BattleCamSpawner>();
            ScriptCamController = ScriptCameraSpawner.SpawnCamera(FocusHeight, CameraPrefab, BattleCameraInputType.ScriptController);
            ScriptCamInput = (BattleCamScriptInput)ScriptCamController.MyBattleCamInput;
            ScriptCamController.SetActive(false);
        }

        /// <summary>
        /// Initialise the Hideables list, and also each of the hideables
        /// </summary>
        private void _GetHideables()
        {
            Hideables = new List<Hideable>();
            Hideable[] hideableArray = FindObjectsOfType<Hideable>();
            foreach (Hideable hideable in hideableArray)
            {
                Hideables.Add(hideable);
                hideable.Initialise();
            }
        }

        #endregion

        #endregion





        //----------------------------------------------------------------------------
        //             StartTurn
        //----------------------------------------------------------------------------

        #region StartTurn

        public IEnumerator StartTurn(IHasTurn hasTurn)
        {
            yield return StartCoroutine(_SwitchToScriptCamera());
            _FocusOnIHasTurn(hasTurn);

            yield return new WaitForSeconds(2f);

            yield return StartCoroutine(_SwitchToPlayerCamera());
            _PointAtIHasTurn(hasTurn);

            yield return new WaitForSeconds(0.5f);
        }

        #region Implementation

        /// <summary>
        /// Crossfade to ScriptCamera <para/>
        /// Takes 0.5 seconds to return <para/>
        /// Takes 0.5 seconds after it returns to fade back in, to allow things to be moved in the blackout
        /// </summary>
        private IEnumerator _SwitchToScriptCamera()
        {
            Crossfade.SetTrigger("Start");

            yield return new WaitForSeconds(0.5f);

            _ResetHideables();
            PlayerCamController.SetActive(false);
            ScriptCamController.SetActive(true);

            Crossfade.SetTrigger("End");
        }

        /// <summary>
        /// Set the ScriptCamera to rotate around the new IHasTurn
        /// </summary>
        private void _FocusOnIHasTurn(IHasTurn hasTurn)
        {
            Vector3 myPosition = hasTurn.MyHexContents.ContentTransform.position;
            ScriptCamController.JumpTo(myPosition.x, myPosition.z);
            ScriptCamController.ZoomTo(2);
            ScriptCamInput.RotationFloat = 0.1f;
        }

        /// <summary>
        /// Crossfade to PlayersCamera <para/>
        /// Takes 0.5 seconds to return<para/>
        /// Takes 0.5 seconds after it returns to fade back in, to allow things to be moved in the blackout
        /// </summary>
        private IEnumerator _SwitchToPlayerCamera()
        {
            
            Crossfade.SetTrigger("Start");
            yield return new WaitForSeconds(0.5f);

            _ResetHideables();
            PlayerCamController.SetActive(true);
            ScriptCamController.SetActive(false);

            Crossfade.SetTrigger("End");
        }

        /// <summary>
        /// When giving control back to the player, move to the camera
        /// to the new IHasTurn
        /// </summary>
        private void _PointAtIHasTurn(IHasTurn hasTurn)
        {
            Vector3 myPosition = hasTurn.MyHexContents.ContentTransform.position;
            PlayerCamController.JumpTo(myPosition.x, myPosition.z);
        }

        private void _ResetHideables()
        {
            foreach(Hideable hideable in Hideables)
            {
                StartCoroutine(hideable.ShowCoroutine());
            }
        }

        public StartTurnType MyStartTurnType
        {
            get
            {
                return StartTurnType.CameraEvent;
            }
        }


        #endregion

        #endregion
    }
}



