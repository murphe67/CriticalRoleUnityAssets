using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.BattleCamera;
using CriticalRole.Turns;

public class Hideable : MonoBehaviour, IStartTurnEvent
{ 
    public void Initialise()
    {
        myMaterial = GetComponent<MeshRenderer>().material;
        myRenderer = GetComponent<MeshRenderer>();

        TurnControllerMarker[] turnControllerMarkers = FindObjectsOfType<TurnControllerMarker>();
        foreach (TurnControllerMarker turnControllerMarker in turnControllerMarkers)
        {
            turnControllerMarker.GetComponent<ITurnController>().AddStartTurnEvent(this);
        }
    }

    public StartTurnType MyStartTurnType
    {
        get
        {
            return StartTurnType.HideEvent;
        }
    }

    public IEnumerator StartTurn(IHasTurn hasTurn)
    {
        StartCoroutine(ShowCoroutine());
        yield break;
    }


    Material myMaterial;
    MeshRenderer myRenderer;

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
            StartCoroutine(ShowCoroutine());
        }
    }

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
}
