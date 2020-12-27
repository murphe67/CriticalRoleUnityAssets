using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITerrain
{
    void Configure();
}

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(TerrainMarker))]
public class TerrainBlocker : MonoBehaviour, ITerrain
{
    public void Configure()
    {
        Bounds colliderBounds = GetComponent<BoxCollider>().bounds;
        
        Collider[] colliderArray = Physics.OverlapBox(colliderBounds.center, colliderBounds.extents);

        

        foreach (Collider collider in colliderArray)
        {
            if (collider.gameObject.TryGetComponent(out IHexagon hex))
            {
                hex.KeepHexagon();
            }
        }

        gameObject.SetActive(false);
    }
}
