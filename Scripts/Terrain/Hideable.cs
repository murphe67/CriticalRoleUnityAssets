using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.BattleCamera;
using CriticalRole.Turns;

//----------------------------------------------------------------------------
//             Class Description
//----------------------------------------------------------------------------
//
// Add this component to any mesh that could block the camera view
// Then add a trigger setup (collider + rigidbody)
//
// When the camera enters the collider, the mesh will vanish, but still cast shadows
//
// Is also connected to the BattleCamManager, because cameras are simply disabled
// when a camera transition happens, so the meshes have to be manually reenabled
// when the camera switch

public class Hideable : MonoBehaviour
{
    //----------------------------------------------------------------------------
    //             Initialise
    //----------------------------------------------------------------------------

    #region Initialise

    public void Initialise()
    {
        myMaterial = GetComponent<MeshRenderer>().material;
    }

    Material myMaterial;

    #endregion




    //----------------------------------------------------------------------------
    //             Trigger Functions
    //----------------------------------------------------------------------------

    #region Trigger Functions

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out Camera camera);
        if(camera != null)
        {
            StopAllCoroutines();
            StartCoroutine(HideCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.TryGetComponent(out Camera camera);
        if (camera != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowCoroutine());
        }
    }

    #endregion




    //----------------------------------------------------------------------------
    //             Hide/Show
    //----------------------------------------------------------------------------

    #region Hide/Show
    public IEnumerator HideCoroutine()
    {
        StandardShaderUtils.ChangeRenderMode(myMaterial, StandardShaderUtils.BlendMode.Fade);

        Color color = myMaterial.color;
        while(color.a > 0)
        {
            color.a -= Time.deltaTime * 5;
            myMaterial.color = color;
            yield return null;
        }
        color.a = 0;
        myMaterial.color = color;
    }

    public IEnumerator ShowCoroutine()
    {
        Color color = myMaterial.color;
        while (color.a < 1)
        {
            color.a += Time.deltaTime * 5;
            myMaterial.color = color;
            yield return null;
        }
        color.a = 1;
        myMaterial.color = color;
        StandardShaderUtils.ChangeRenderMode(myMaterial, StandardShaderUtils.BlendMode.Opaque);
    }

    #endregion



}
