using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Turns;
using CriticalRole.Attacking;
using CriticalRole.Death;
using CriticalRole.Dependancy;

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
        /// <summary>
        /// Subclass that showcases an attack with the camera
        /// </summary>
        BattleCamAttackFocus MyAttackFocus { get; }


        IEnumerator SwitchToScriptCamera();
        IEnumerator SwitchToPlayerCamera();

        IEnumerator StartTurnScript(IHasTurn hasTurn);
        IEnumerator StartTurnPlayer(IHasTurn hasTurn);

        /// <summary>
        /// Showcase the dying character with the camera
        /// </summary>
        IEnumerator FocusOnDeath(Transform dying);

        /// <summary>
        /// Change the speed at which the camera fade in happens
        /// </summary>
        IEnumerator ChangeSwitchSpeed(float speed);
    }


    public class BattleCamManager : MonoBehaviour, IBattleCamManager
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
        /// Originally set to 0.7f
        /// </summary>
        [Tooltip("Default value: 0.7")]
        public float FocusHeight = 0.7f;


        public Animator Crossfade;

        #endregion



        //----------------------------------------------------------------------------
        //             Initialise
        //----------------------------------------------------------------------------

        #region Initialise


        public void Awake()
        {
            _RegisterWithTurnController();
            _RegisterWithAttackManager();
            _RegisterWithDeathManager();

            _SpawnPlayerCamera();
            _SpawnScriptCamera();
            
            _SpawnHideableManager();
            _SpawnAttackFocus();

            Crossfade.speed = DefaultFadeSpeed;
            CurrentSwitchDelay = DefaultSwitchDelay;
        }

        public IBattleCamController PlayerCamController;
        public IBattleCamController ScriptCamController;
        public IBattleCamScriptInput ScriptCamInput;
        public HideableManager MyHideableManager;
        public BattleCamAttackFocus MyAttackFocus { get; set; }

        #region Implementation
        private void _RegisterWithTurnController()
        {
            TurnControllerMarker[] turnControllerMarkers = FindObjectsOfType<TurnControllerMarker>();
            foreach (TurnControllerMarker turnControllerMarker in turnControllerMarkers)
            {
                ITurnController turnController = turnControllerMarker.GetComponent<ITurnController>();
                turnController.RegisterBattleCamManager(this);
            }
        }

        private void _RegisterWithAttackManager()
        {
            AttackManagerMarker[] attackManagerMarkers = FindObjectsOfType<AttackManagerMarker>();
            foreach (AttackManagerMarker attackManagerMarker in attackManagerMarkers)
            {
                attackManagerMarker.gameObject.GetComponent<IAttackManager>().RegisterCamManager(this);
            }
        }

        private void _RegisterWithDeathManager()
        {
            DeathManagerMarker[] deathManagerMarkers = FindObjectsOfType<DeathManagerMarker>();
            foreach(DeathManagerMarker deathManagerMarker in deathManagerMarkers)
            {
                deathManagerMarker.gameObject.GetComponent<IDeathManager>().RegisterBattleCamManager(this);
            }
        }

        private void _SpawnPlayerCamera()
        {
            GameObject PlayerCameraGO = new GameObject("Player Camera");
            IBattleCameraSpawner PlayerCameraSpawner = PlayerCameraGO.AddComponent<BattleCamSpawner>();
            PlayerCamController = PlayerCameraSpawner.SpawnCamera(FocusHeight, CameraPrefab, BattleCameraInputType.PlayerControlled);
            PlayerCamController.MyBattleCamTransform.SetActive(false);
        }

        private void _SpawnScriptCamera()
        {
            GameObject ScriptCameraGO = new GameObject("Script Camera");
            IBattleCameraSpawner ScriptCameraSpawner = ScriptCameraGO.AddComponent<BattleCamSpawner>();
            ScriptCamController = ScriptCameraSpawner.SpawnCamera(FocusHeight, CameraPrefab, BattleCameraInputType.ScriptController);
            ScriptCamInput = (IBattleCamScriptInput)ScriptCamController.MyBattleCamInput;
            ScriptCamController.MyBattleCamTransform.SetActive(false);
        }

        /// <summary>
        /// Initialise the Hideables list, and also each of the hideables
        /// </summary>
        private void _SpawnHideableManager()
        {
            MyHideableManager = new HideableManager();
            MyHideableManager.Initialise();
        }

        private void _SpawnAttackFocus()
        {
            MyAttackFocus = gameObject.AddComponent<BattleCamAttackFocus>();
            MyAttackFocus.Initialise(this);
        }

        #endregion

        #endregion





        //----------------------------------------------------------------------------
        //             StartTurn
        //----------------------------------------------------------------------------

        #region StartTurn

        public IEnumerator StartTurnScript(IHasTurn hasTurn)
        {
            CurrentHasTurn = hasTurn;
            CurrentHasTurnTransform = hasTurn.MyHexContents.ContentTransform;

            yield return StartCoroutine(SwitchToScriptCamera());
            _StartTurnFocus();
        }

        public IEnumerator StartTurnPlayer(IHasTurn hasTurn)
        {
            yield return StartCoroutine(SwitchToPlayerCamera());
            _PointAtIHasTurn();

            yield return new WaitForSeconds(0.5f);
        }

        public IHasTurn CurrentHasTurn;
        public Transform CurrentHasTurnTransform;

        #region Implementation

        /// <summary>
        /// Set the ScriptCamera to rotate around the new IHasTurn
        /// </summary>
        private void _StartTurnFocus()
        {
            Vector3 myPosition = CurrentHasTurnTransform.position;
            Quaternion myRotation = CurrentHasTurnTransform.rotation;

            ScriptCamController.MyBattleCamTransform.JumpTo(myPosition.x, myPosition.z);
            ScriptCamController.MyBattleCamTransform.ZoomTo(3);
            ScriptCamController.MyBattleCamTransform.RotateTo(myRotation);
            ScriptCamController.MyBattleCamTransform.RotateAround(myPosition, -10);

            ScriptCamInput.SetInputs(0.1f, 0, 0, 0);
        }


        #endregion

        #endregion


        //----------------------------------------------------------------------------
        //             FocusOnDeath
        //----------------------------------------------------------------------------
        
        #region FocusOnDeath
        public IEnumerator FocusOnDeath(Transform dying)
        {
            yield return StartCoroutine(SwitchToScriptCamera());
            Vector3 position = dying.position;
            ScriptCamController.MyBattleCamTransform.JumpTo(position.x, position.z);
            ScriptCamController.MyBattleCamTransform.RotateTo(dying.rotation);
            ScriptCamController.MyBattleCamTransform.RotateAround(position, 130);
            ScriptCamController.MyBattleCamTransform.ZoomTo(1);

            ScriptCamInput.SetInputs(0.02f, 0, 0, 0);
            yield return new WaitForSeconds(3f);
            ScriptCamInput.SetInputs(0, 0, 0, 0);
        }

        #endregion



        //----------------------------------------------------------------------------
        //             Camera Switching
        //----------------------------------------------------------------------------

        #region Camera Switching

        /// <summary>
        /// Crossfade to ScriptCamera <para/>
        /// Takes 0.4 seconds to return <para/>
        /// Takes 0.5 seconds after it returns to fade back in, to allow things to be moved in the blackout
        /// </summary>
        public IEnumerator SwitchToScriptCamera()
        {
            Crossfade.SetTrigger("Start");

            yield return new WaitForSeconds(CurrentSwitchDelay);

            PlayerCamController.MyBattleCamTransform.SetActive(false);
            ScriptCamController.MyBattleCamTransform.SetActive(true);

            Crossfade.SetTrigger("End");
            ResetHideables(ScriptCamController.MyBattleCamTransform.Position);
        }

        /// <summary>
        /// Crossfade to PlayersCamera <para/>
        /// Takes 0.4 seconds to return<para/>
        /// Takes 0.5 seconds after it returns to fade back in, to allow things to be moved in the blackout
        /// </summary>
        public IEnumerator SwitchToPlayerCamera()
        {
            Crossfade.SetTrigger("Start");
            yield return new WaitForSeconds(CurrentSwitchDelay);

            PlayerCamController.MyBattleCamTransform.SetActive(true);
            ScriptCamController.MyBattleCamTransform.SetActive(false);

            Crossfade.SetTrigger("End");
            ResetHideables(PlayerCamController.MyBattleCamTransform.Position);
        }

        

        public IEnumerator ChangeSwitchSpeed(float speed)
        {
            Crossfade.speed = speed;
            CurrentSwitchDelay = DefaultSwitchDelay / speed;
            yield return new WaitForSeconds(CurrentSwitchDelay * 2);
            Crossfade.speed = DefaultFadeSpeed;
            CurrentSwitchDelay = DefaultSwitchDelay;
        }

        public float CurrentSwitchDelay;
        public readonly float DefaultSwitchDelay = 0.6f; 
        public readonly float DefaultFadeSpeed = 1f;


        /// <summary>
        /// When giving control back to the player, move to the camera
        /// to the new IHasTurn
        /// </summary>
        public void _PointAtIHasTurn()
        {
            Vector3 myPosition = CurrentHasTurnTransform.position;
            Quaternion myRotation = CurrentHasTurnTransform.rotation;

            PlayerCamController.MyBattleCamTransform.RotateTo(myRotation);
            PlayerCamController.MyBattleCamTransform.RotateAround(myPosition, 20);

            PlayerCamController.MyBattleCamTransform.JumpTo(myPosition.x, myPosition.z);
        }

        #endregion




        //----------------------------------------------------------------------------
        //             ResetHideables
        //----------------------------------------------------------------------------

        #region ResetHideables

        public void ResetHideables(Vector3 position)
        {
            MyHideableManager.ResetForNewCamera(position);
        }

        #endregion

        

    }
}



