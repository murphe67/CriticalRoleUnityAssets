using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Map
{

    public interface ITerrain
    {
        void SetTerrainDataOnHexes();
    }





    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(TerrainMarker))]
    public class TerrainBlocker : MonoBehaviour, ITerrain
    {
        public void SetTerrainDataOnHexes()
        {
            _GetOverlappedHexagons();

            _SetTerrainDataInHexagons();

            _DeactivateThisTerrainBlocker();
        }

        private Collider[] _ColliderArray;

        /// <summary>
        /// Get all the hexagons within the area of this terrain blocker.
        /// </summary>
        private void _GetOverlappedHexagons()
        {
            Bounds colliderBounds = GetComponent<BoxCollider>().bounds;
            _ColliderArray = Physics.OverlapBox(colliderBounds.center, colliderBounds.extents);

        }

        private void _SetTerrainDataInHexagons()
        {
            foreach (Collider collider in _ColliderArray)
            {
                if (collider.gameObject.TryGetComponent(out HexKeep hexKeep))
                {
                    hexKeep.SetTerrainData();
                }
            }
        }

        /// <summary>
        /// Once the terrain blocker has updated the hexagons, it should be disabled.
        /// </summary>
        private void _DeactivateThisTerrainBlocker()
        {
            gameObject.SetActive(false);
        }


    }
}
