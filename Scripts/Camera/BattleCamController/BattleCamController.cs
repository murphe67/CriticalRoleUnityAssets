using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CriticalRole.BattleCamera
{
    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------

    // This class gives camera controls for moving around a battlefield.
    // Enable/Disable the gameObject it is attached to for camera switching
    // 
    // This class should never manually be placed in a scene, and must be spawned by
    // a BattleCameraSpawner.
    //
    // Input comes through the IBattleCamInput interface
    //
    // Trigonometric zoom using mouse wheel, angle changes appropriately for up close/tactical
    //
    // Focus based translation and rotation, so you're moving and rotating around
    // the spot you're looking at
    //
    // All movement is lerped to be responsive + smooth
    //
    // BattleCamTransform subclass replicates Transform functionality

    public interface IBattleCamController
    {
        IBattleCamInput MyBattleCamInput { get; }

        IBattleCamTransform MyBattleCamTransform { get; }

    }






    public class BattleCamController : MonoBehaviour, IBattleCamController
    {

        public IBattleCamInput MyBattleCamInput { get; set; }

        public IBattleCamTransform MyBattleCamTransform { get; set; }

        public Vector3 Position
        {
            get
            {
                return gameObject.transform.position;
            }
        }

        //----------------------------------------------------------------------------
        //                    Initialisation
        //----------------------------------------------------------------------------

        #region Initialisation



        public void Initialise(GameObject cameraLookAt, GameObject cameraLerped, GameObject focusControlled, GameObject focusLerped, IBattleCamInput battleCameraInput)
        {
            MyBattleCamInput = battleCameraInput;

            CameraLookAt = cameraLookAt.transform;
            CameraLerped = cameraLerped.transform;
            FocusControlled = focusControlled.transform;
            FocusLerped = focusLerped.transform;

            MyBattleCamTransform = new BattleCamTransform(this);
        }

        //These 4 variables must be spawned and set by BattleCameraSpawner
        //They make up the 4 moving parts of the class, including the GameObject of this class

        /// <summary>
        /// GameObject to call LookAt on, in order to lerp the look at rotation
        /// </summary>
        public Transform CameraLookAt;

        /// <summary>
        /// Transform of the GameObject camera is attached to
        /// </summary>
        public Transform CameraLerped;

        /// <summary>
        /// This focus is what the player actually controls. <para />
        /// However, the camera has no idea it exists.  <para />
        /// Its height from the ground is a construction parameter, and is not changed during gameplay.
        /// </summary>
        public Transform FocusControlled;

        /// <summary>
        /// The camera is a set distance away from this focus, and pointed towards it <para />
        /// Every frame, it linearly interpolates towards the FocusControlled.
        /// </summary>
        public Transform FocusLerped;





        #endregion




        //----------------------------------------------------------------------------
        //                    Frame to Frame Change
        //----------------------------------------------------------------------------

        #region Frame To Frame Change

        /// <summary>
        /// Monobehaviour functions should only call a single other function
        /// </summary>
        public void Update()
        {
            UpdateCamera();
        }

        /// <summary>
        /// Update the camera's position and rotation based on new inputs and interpolations
        /// </summary>
        public void UpdateCamera()
        {
            RotateFocus();
            UpdateZoomVariables();
            TranslateFocus();

            LerpToFocus();

            MoveCamera();
        }

        #endregion



        //----------------------------------------------------------------------------
        //                    Zoom
        //----------------------------------------------------------------------------

        #region Zoom

        /// <summary>
        /// Self Explanatory purpose <para />
        /// Updates ZoomControlled based on input and clamps it <para />
        /// Lerps ZoomLerped to ZoomControlled <para />
        /// 
        /// Keeps the camera the right offset from FocusLerped based on ZoomLerped <para />
        /// </summary>
        public void UpdateZoomVariables()
        {
            ZoomControlled += MyBattleCamInput.ZoomInput() * ZoomSpeed * DefaultZoomSpeed;

            ZoomControlled = Mathf.Clamp(ZoomControlled, MinimumZoom, MaximumZoom);

            ZoomLerped = Mathf.Lerp(ZoomLerped, ZoomControlled, Time.deltaTime * ZoomLerpSpeed);
        }


        /// <summary>
        /// Zoom Speed parameter for a settings menu <para />
        /// Scales zoom per mouse rotation from a default of 1
        /// </summary>
        [HideInInspector]
        public float ZoomSpeed = 1;

        /// <summary>
        /// The current level of zoom <para />
        /// Linearly interpolated to ZoomControlled every frame
        /// </summary>
        public float ZoomLerped = 4;

        /// <summary>
        /// The zoom level actually controlled by the player <para />
        /// In isolation, has no effect on the camera
        /// </summary>
        public float ZoomControlled = 4;

        /// <summary>
        /// The default amount of zoom <para />
        /// Originally set to 0.5f
        /// </summary>
        public readonly float DefaultZoomSpeed = 0.5f;

        /// <summary>
        /// The closest the camera can get to the camera focus <para />
        /// Originally set to 2
        /// </summary>
        public readonly float MinimumZoom = 1f;

        /// <summary>
        /// The furthest the camera can get from the camera focus <para />
        /// Originally set to 12
        /// </summary>
        public readonly float MaximumZoom = 12f;

        /// <summary>
        /// How quickly to lerp between ZoomCurrent and ZoomLerped <para />
        /// Originally set to 3
        /// </summary>
        public readonly float ZoomLerpSpeed = 3f;

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        public readonly float RadRatio = 0.01745f;
        /// <summary>
        /// The actual camera position is calculated in this property <para />
        /// 
        /// How far behind the focus the camera is <para />
        /// Calculated from ZoomLerped and CameraAngle
        /// </summary>
        public float CameraZOffset
        {
            get { return ZoomLerped * -Mathf.Cos(CameraAngle * RadRatio); }
        }

        /// <summary>
        /// The actual camera position is calculated in this property <para />
        /// 
        /// How far above the focus the camera is <para />
        /// Two way relationship- CameraAngle changes based on CameraYOffset. <para />
        /// 
        /// YOffset is based on ZoomLerped and CameraAngle
        /// </summary>
        public float CameraYOffset
        {
            get { return ZoomLerped * Mathf.Sin(CameraAngle * RadRatio); }
        }

        /// <summary>
        /// Minimum Angle Camera is to the Focus <para />
        /// Originally set to 20f
        /// </summary>
        public readonly float MinCameraAngle = 20f;

        /// <summary>
        /// Maximum angle camera makes with the focus <para />
        /// Originally set to 45
        /// </summary>
        public readonly float MaxCameraAngle = 45f;

        /// <summary>
        /// Rotates the camera based on how high it is
        /// </summary>
        public float CameraAngle
        {
            get
            {
                return MinCameraAngle + (((ZoomLerped - MinimumZoom) / (MaximumZoom - MinimumZoom) * (MaxCameraAngle - MinCameraAngle)));
            }
        }


        #endregion




        //----------------------------------------------------------------------------
        //                    Rotate
        //----------------------------------------------------------------------------

        #region Rotate
        /// <summary>
        /// Rotate FocusControlled around its y axis <para />
        /// This means the camera rotates around whatever its looking at
        /// </summary>
        public void RotateFocus()
        {
            float rot = MyBattleCamInput.RotationInput() * Time.deltaTime * RotationSpeed * DefaultRotationSpeed;
            FocusControlled.Rotate(0, rot, 0);
        }

        /// <summary>
        /// Rotation Speed parameter for a settings menu <para />
        /// Scales rotation per frame from default of 1
        /// </summary>
        [HideInInspector]
        public float RotationSpeed = 1;

        /// <summary>
        /// The default amount of rotation <para />
        /// Originally set to 25
        /// </summary>
        public readonly float DefaultRotationSpeed = 100f;

        #endregion





        //----------------------------------------------------------------------------
        //                    Translate
        //----------------------------------------------------------------------------

        #region Translate
        public void TranslateFocus()
        {
            float deltaX = MyBattleCamInput.TranslateRightInput();
            float deltaZ = MyBattleCamInput.TranslateForwardInput();

            //If zooming the opposite direction of travelling, increase travel speed so it stays roughly constant
            if (MyBattleCamInput.ZoomInput() * MyBattleCamInput.TranslateForwardInput() > 0)
            {
                deltaZ += MyBattleCamInput.ZoomInput() * DefaultZoomCorrection;
            }

            Vector3 deltaTranslate = new Vector3(0, 0, 0);
            deltaTranslate += deltaX * FocusControlled.right;
            deltaTranslate += deltaZ * FocusControlled.forward;
            deltaTranslate = deltaTranslate * Time.deltaTime * AltitudeSpeed * DefaultTranslationSpeed * TranslationSpeed;

            FocusControlled.position += deltaTranslate;
        }

        /// <summary>
        /// Rotation Speed parameter for a settings menu <para />
        /// Scales translation per frame from default of 1
        /// </summary>
        public float TranslationSpeed = 1;

        /// <summary>
        /// Ranges from 1-2 based on zoom level 
        /// </summary>
        public float AltitudeSpeed
        {
            get { return 1 + ((ZoomLerped - MinimumZoom) / MaximumZoom); }
        }

        /// <summary>
        /// Default amount of translation <para />
        /// Originally set to 3
        /// </summary>
        public readonly float DefaultTranslationSpeed = 3f;

        /// <summary>
        /// Amount of zoom to speed up by when zooming in the opposite direction of travel <para />
        /// Originally set to 0.03
        /// </summary>
        public readonly float DefaultZoomCorrection = 0.03f;

        #endregion






        //----------------------------------------------------------------------------
        //                    Interpolation
        //----------------------------------------------------------------------------

        #region LerpToFocus
        public void LerpToFocus()
        {
            FocusLerped.position = Vector3.Lerp(FocusLerped.position, FocusControlled.position, Time.deltaTime * 8);
            FocusLerped.rotation = Quaternion.Lerp(FocusLerped.rotation, FocusControlled.rotation, Time.deltaTime * 8);
        }

        #endregion


        //----------------------------------------------------------------------------
        //                    MoveCamera
        //----------------------------------------------------------------------------

        #region MoveCamera
        /// <summary>
        /// This function properly connects the camera to FocusLerped <para />
        /// 
        /// Purpose 1: point the camera at FocusLerped <para/>
        /// Purpose 2: keep the camera at the correct triginometric offset <para/>
        /// Purpose 3: rotate the camera when FocusLerped rotates  <para/>
        /// 
        /// The second 2 purposes act as an advanced parent/child relationship <para/>
        /// </summary>
        public void MoveCamera()
        {
            //Important: using the local direction vectors means the camera also rotates when FocusLerped Rotates
            //The rotate function breaks if world direction vectors used
            transform.position = FocusLerped.position + (FocusLerped.forward * CameraZOffset) + (FocusLerped.up * CameraYOffset);

            //The lerp required is much less than for other inputs
            //it just removes the small amount of stutter from LookAt
            CameraLookAt.LookAt(FocusLerped);
            CameraLerped.rotation = Quaternion.Lerp(CameraLerped.rotation, CameraLookAt.rotation, 0.9f);
        }
        #endregion






    }
}


