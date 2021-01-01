using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.BattleCamera
{
    public enum BattleCameraInputType
    {
        PlayerControlled,
        ScriptController
    }

    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------

    // This class spawns a BattleCamera, a camera that can move around the battlefield
    //
    // It has a lot of parts so it has its own dedicated spawner class
    //
    // Call initialise to spawn the camera, passing its height from the ground, camera prefab, and input type


    public interface IBattleCameraSpawner
    {
        BattleCamController SpawnCamera(float focusHeight, GameObject cameraPrefab, BattleCameraInputType myBattleCameraType);
    }






    public class BattleCamSpawner : MonoBehaviour, IBattleCameraSpawner
    {

        //----------------------------------------------------------------------------
        //                    SpawnCamera
        //----------------------------------------------------------------------------

        #region SpawnCamera

        public BattleCamController SpawnCamera(float focusHeight, GameObject cameraPrefab, 
                                                    BattleCameraInputType myBattleCameraType)
        {
            FocusHeight = focusHeight;
            CameraPrefab = cameraPrefab;
            MyBattleCameraInputType = myBattleCameraType;
            


            SpawnCameraController();
            SpawnCameraLookAt();
            SpawnCameraLerped();
            SpawnFocusControlled();
            SpawnFocusLerp();
            SpawnInput();

            MyBattleCameraController.Initialise(CameraLookAt, CameraLerped, 
                                                FocusControlled, FocusLerp, MyBattleCameraInput);

            return MyBattleCameraController;
        }


        /// <summary>
        /// How high off the ground is the camera. <para/>
        /// Used by both SpawnFocus's
        /// </summary>
        public float FocusHeight;
        /// <summary>
        /// Prefab to set up camera, audio listener and raycaster <para/>
        /// Used by spawnCameraLerped.
        /// </summary>
        public GameObject CameraPrefab;
        /// <summary>
        /// Used by SpawnInput 
        /// </summary>
        BattleCameraInputType MyBattleCameraInputType;

        #endregion





        //----------------------------------------------------------------------------
        //                    SpawnCameraController
        //----------------------------------------------------------------------------

        #region SpawnController

        /// <summary>
        /// Add the BattleCameraController component to a new gameobject, and set it as a child of this gameobject
        /// </summary>
        public void SpawnCameraController()
        {
            BattleCamera = new GameObject("Battle Camera Parent");
            BattleCamera.transform.parent = gameObject.transform;
            MyBattleCameraController = BattleCamera.AddComponent<BattleCamController>();
        }

        /// <summary>
        /// Gameobject BattleCameraController component is added to. <para/>
        /// Added through new by SpawnCameraController
        /// </summary>
        GameObject BattleCamera;
        /// <summary>
        /// BattleCameraController component that is being spawned. <para/>
        /// Added through AddComponent to BattleCamera by SpawnCameraController
        /// </summary>
        BattleCamController MyBattleCameraController;

        #endregion



        //----------------------------------------------------------------------------
        //                    SpawnCameraLookAt
        //----------------------------------------------------------------------------

        #region SpawnCameraLookAt

        /// <summary>
        /// Create the empty GameObject CameraLookAt, and make it a child of BattleCamera
        /// </summary>
        public void SpawnCameraLookAt()
        {
            CameraLookAt = new GameObject("Camera Look At");
            CameraLookAt.transform.parent = BattleCamera.transform;
        }

        /// <summary>
        /// Empty Gameobject. Spawned by SpawnCameraLookAt. Child of BattleCamera
        /// </summary>
        public GameObject CameraLookAt;

        #endregion



        //----------------------------------------------------------------------------
        //                    SpawnCameraLerped
        //----------------------------------------------------------------------------

        #region SpawnCameraLerped

        /// <summary>
        /// Add the CameraPrefab CameraLerped, and make it a child of BattleCamera. <para/>
        /// This is the component that actually has the camera.
        /// </summary>
        public void SpawnCameraLerped()
        {
            CameraLerped = Instantiate(CameraPrefab);
            CameraLerped.name = "Battle Camera";
            CameraLerped.transform.parent = BattleCamera.transform;
        }

        /// <summary>
        /// Actual camera. Instance of CameraPrefab. Child of BattleCamera. Spawned by SpawnCameraLerped
        /// </summary>
        GameObject CameraLerped;

        #endregion




        //----------------------------------------------------------------------------
        //                    SpawnControlledFocus
        //----------------------------------------------------------------------------

        #region SpawnControlledFocus

        /// <summary>
        /// Add the empty Gameobject FocusControlled at (0, FocusHeight, 0). <para/>
        /// Make it a child of this GameObject;
        /// </summary>
        public void SpawnFocusControlled()
        {
            FocusControlled = new GameObject("Controlled Focus");
            FocusControlled.transform.position = new Vector3(0, FocusHeight, 0);
            FocusControlled.transform.parent = transform;
        }

        /// <summary>
        /// Empty gameobject. Spawned by SpawnCFocusControlled at (0, FocusHeight, 0). <para/>
        /// Child of this Gameobject.
        /// </summary>
        GameObject FocusControlled;

        #endregion




        //----------------------------------------------------------------------------
        //                    SpawnLerpedFocus
        //----------------------------------------------------------------------------

        #region SpawnLerpedFocus

        /// <summary>
        /// Add the empty Gameobject FocusLerp at (0, FocusHeight, 0). <para/>
        /// Make it a child of this Gameobject
        /// </summary>
        public void SpawnFocusLerp()
        {
            FocusLerp = new GameObject("Lerped Focus");
            FocusLerp.transform.position = new Vector3(0, FocusHeight, 0);
            FocusLerp.transform.parent = transform;
        }

        /// <summary>
        /// Empty gameobject. Spawned by SpawnFocusLerp at (0, FocusHeight, 0). Child of this Gameobject.
        /// </summary>
        GameObject FocusLerp;

        #endregion



        //----------------------------------------------------------------------------
        //                    SpawnInput
        //----------------------------------------------------------------------------
        
        #region SpawnInput

        /// <summary>
        /// Spawn the IBattleCameraInput MyBattleCamera input based on the enum passed to Initialise
        /// </summary>
        public void SpawnInput()
        {
            switch(MyBattleCameraInputType)
            {
                case BattleCameraInputType.PlayerControlled:
                    MyBattleCameraInput = new BattleCamPlayerInput();
                    break;
                case BattleCameraInputType.ScriptController:
                    MyBattleCameraInput = new BattleCamScriptInput();
                    break;
            }
        }

        /// <summary>
        /// Spawned by SpawnInput based on the enum passed to Initialise
        /// </summary>
        IBattleCamInput MyBattleCameraInput;

        #endregion
    }

}

