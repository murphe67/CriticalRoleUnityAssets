using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Map
{
    public interface IHexKeep
    {
        void SetTerrainData();

        bool ShouldRemove { get; }

        void DeleteHex();
    }

    public class HexKeep : MonoBehaviour, IHexKeep
    {
        public void Awake()
        {
            ShouldRemove = true;
        }

        public void SetTerrainData()
        {
            ShouldRemove = false;
        }

        public bool ShouldRemove { get; set; }

        public void DeleteHex()
        {
            Destroy(gameObject);
        }
    }
}

