using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOccluder : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out MeshRenderer myRenderer);
        if(myRenderer != null)
        {
            myRenderer.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.TryGetComponent(out MeshRenderer myRenderer);
        if (myRenderer != null)
        {
            myRenderer.enabled = true;
        }
    }
}
