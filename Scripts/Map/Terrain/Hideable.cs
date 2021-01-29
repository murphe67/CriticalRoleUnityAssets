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

    public void Initialise(HideableManager hideableManager)
    {
        MyMaterials = GetComponent<MeshRenderer>().materials;
        MyHideableManager = hideableManager;
        MyBoxCollider = GetComponent<BoxCollider>();
    }

    public Material[] MyMaterials;
    public HideableManager MyHideableManager;
    public BoxCollider MyBoxCollider;

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
        MyHideableManager.SetAsHidden(this);
        _SetRenderMode(StandardShaderUtils.BlendMode.Fade);

        Color color = MyMaterials[0].color;
        while(color.a > 0)
        {
            color.a -= Time.deltaTime * 5;
            _SetAlpha(color.a);
            yield return null;
        }
        _SetAlpha(0);
    }

    public IEnumerator ShowCoroutine()
    {
        MyHideableManager.SetAsUnhidden(this);
        Color color = MyMaterials[0].color;
        while (color.a < 1)
        {
            color.a += Time.deltaTime * 5;
            _SetAlpha(color.a);
            yield return null;
        }
        _SetAlpha(1);
        _SetRenderMode(StandardShaderUtils.BlendMode.Opaque);
    }

    private void _SetRenderMode(StandardShaderUtils.BlendMode blendMode)
    {
        foreach(Material material in MyMaterials)
        {
            StandardShaderUtils.ChangeRenderMode(material, blendMode);
        }
    }

    private void _SetAlpha(float alpha)
    {
        Color color;
        foreach(Material material in MyMaterials)
        {
            color = material.color;
            color.a = alpha;
            material.color = color;
        }
    }

    #endregion

    public void ResetForNewCamera(Vector3 position)
    {
        if(!MyBoxCollider.bounds.Contains(position))
        {
            StartCoroutine(ShowCoroutine());
        }
    }
}
