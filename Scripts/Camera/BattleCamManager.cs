using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Turns;

namespace CriticalRole.BattleCamera
{
    public class BattleCamManager : MonoBehaviour, IStartTurnEvent
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
        //             Initialise
        //----------------------------------------------------------------------------

        #region Initialise

        private void Awake()
        {
            GameObject dependancyGO = FindObjectOfType<DependancyManagerMarker>().gameObject;
            IBattleDependancyManager dependancyManager = dependancyGO.GetComponent<IBattleDependancyManager>();
            dependancyManager.RegisterCameraManager(this);
        }

        public void Initialise()
        {
            TurnControllerMarker[] turnControllerMarkers = FindObjectsOfType<TurnControllerMarker>();
            foreach(TurnControllerMarker turnControllerMarker in turnControllerMarkers)
            {
                TurnController turnController = turnControllerMarker.GetComponent<TurnController>();
                turnController.AddStartTurnEvent(this);
            }

            GameObject PlayerCameraGO = new GameObject("Player Camera");
            IBattleCameraSpawner PlayerCameraSpawner = PlayerCameraGO.AddComponent<BattleCamSpawner>();
            PlayerCamController = PlayerCameraSpawner.SpawnCamera(FocusHeight, CameraPrefab, BattleCameraInputType.PlayerControlled);
            PlayerCamController.CamGameObject.SetActive(false);

            GameObject ScriptCameraGO = new GameObject("Script Camera");
            IBattleCameraSpawner ScriptCameraSpawner = ScriptCameraGO.AddComponent<BattleCamSpawner>();
            ScriptCamController =  ScriptCameraSpawner.SpawnCamera(FocusHeight, CameraPrefab, BattleCameraInputType.ScriptController);
            ScriptCamInput = (BattleCamScriptInput)ScriptCamController.MyBattleCamInput;
            ScriptCamController.CamGameObject.SetActive(false);
        }

        IBattleCamController PlayerCamController;
        IBattleCamController ScriptCamController;
        BattleCamScriptInput ScriptCamInput;

        #endregion





        //----------------------------------------------------------------------------
        //             StartTurn
        //----------------------------------------------------------------------------
        
        public IEnumerator StartTurn(IHasTurn hasTurn)
        {
            Crossfade.SetTrigger("Start");
            
            yield return new WaitForSeconds(0.5f);
            PlayerCamController.CamGameObject.SetActive(false);
            ScriptCamController.CamGameObject.SetActive(true);

            Crossfade.SetTrigger("End");

            Vector3 myPosition = hasTurn.MyHexContents.ContentTransform.position;
            ScriptCamController.JumpTo(myPosition.x, myPosition.z);
            ScriptCamController.ZoomTo(2);
            ScriptCamInput.RotationFloat = 0.1f;

            yield return new WaitForSeconds(2f);

            Crossfade.SetTrigger("Start");
            yield return new WaitForSeconds(0.5f);

            PlayerCamController.CamGameObject.SetActive(true);
            ScriptCamController.CamGameObject.SetActive(false);
            PlayerCamController.JumpTo(myPosition.x, myPosition.z);
            

            Crossfade.SetTrigger("End");
            yield return new WaitForSeconds(0.4f);
        }

        //----------------------------------------------------------------------------
        //             StartTurnType
        //----------------------------------------------------------------------------

        public StartTurnType MyStartTurnType
        {
            get
            {
                return StartTurnType.CameraEvent;
            }
        }
    }
}



