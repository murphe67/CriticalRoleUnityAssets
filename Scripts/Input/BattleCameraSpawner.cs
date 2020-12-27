using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//----------------------------------------------------------------------------
//                    Class Description
//----------------------------------------------------------------------------

// This is a setup class
// Single Purpose: Spawn Battlefield Camera, which the player has full control over

// This class exists as the Battlefield Camera needs a parent
// and cannot simply be attached to a gameObject and dropped in a scene
//
// Setup: attach this class to a GameObject


public class BattleCameraSpawner : MonoBehaviour
{
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


    void Awake()
    {
        Initialise();
    }

    void Initialise()
    {
        GameObject battleCamera = new GameObject("Battle Camera Parent");
        BattleCameraController myBattleCameraController = battleCamera.AddComponent<BattleCameraController>();
        battleCamera.transform.parent = gameObject.transform;

        GameObject cameraLookAt = new GameObject("Camera Look At");
        cameraLookAt.transform.parent = myBattleCameraController.transform;

        GameObject cameraLerped = Instantiate(CameraPrefab);
        cameraLerped.name = "Battle Camera";
        cameraLerped.transform.parent = battleCamera.transform;

        GameObject focusControlled = new GameObject("Controlled Focus");
        focusControlled.transform.position = new Vector3(0, FocusHeight, 0);
        focusControlled.transform.parent = gameObject.transform;

        GameObject focusLerp = new GameObject("Lerped Focus");
        focusLerp.transform.position = new Vector3(0, FocusHeight, 0);
        focusLerp.transform.parent = gameObject.transform;

        myBattleCameraController.Initialise(cameraLookAt, cameraLerped, focusControlled, focusLerp);
    }
}
