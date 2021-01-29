using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Contents;
using CriticalRole.Dependancy;

namespace CriticalRole.Map
{
 
    
    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------
    
    // This class manages everything to do with spawning the hexes in the correct pattern
    // as well as initialising them and their contents correctly
    //
    // Not dependant on anything else. Other independant systems are reliant on it,
    // so it registers itself to the dependancy manager in awake, and is the first
    // system called by it to initialise
    //
    // Initialise dependant subsystems IHexagon and IHexContent
    // 
    // Once it finishes, the entire map exists, and everything on the map knows where it is
    // and every IHexagon knows whats standing on it
    
    
    public class MapGeneration : MonoBehaviour, IDependantManager
    {
        //----------------------------------------------------------------------------
        //              Hex Dictionary
        //----------------------------------------------------------------------------
    
        /// <summary>
    /// Dictionary of the hexagons of the map, with the coords as the key
    /// </summary>
        public Dictionary<Vector3Int, IHexagon> HexDict { get; set; }
    
        //----------------------------------------------------------------------------
        //              Inspector Parameters
        //----------------------------------------------------------------------------
    
        #region Inspector Parameters
        /// <summary>
    /// Prefab allows set up of mesh and collider
    /// </summary>
        [Header("Set In Inspector")]
        [Tooltip("Set up mesh and collider in prefab")]
        public GameObject HexagonPrefab;
    
        /// <summary>
    /// Radius of the generated map in hexagons
    /// </summary>
        public int MapRadius;

        #endregion





        //----------------------------------------------------------------------------
        //             Registration
        //---------------------------------------------------------------------------

        #region Registration

        public DependantManagerType MyDependantManagerType
        {
            get
            {
                return DependantManagerType.MapGeneration;
            }
        }

        void Awake()
        {
            _RegisterWithDependancyManager();
        }
    
        private void _RegisterWithDependancyManager()
        {
            GameObject dependancyGO = _GetDependancyGO();
            IBattleDependancyManager dependancyManager = dependancyGO.GetComponent<IBattleDependancyManager>();
            dependancyManager.Register(this);
        }

        private GameObject _GetDependancyGO()
        {
            DependancyManagerMarker dependancy = FindObjectOfType<DependancyManagerMarker>();
            if(dependancy == null)
            {
                Debug.LogError("No dependancy manager in scene");
            }
            return dependancy.gameObject;
        }

        #endregion


        //----------------------------------------------------------------------------
        //             Initialise
        //----------------------------------------------------------------------------

        #region Initialise

        /// <summary>
        /// UI_Input is dependant on MapGeneration's initialise <para />
        /// The dependancy manager must call MapGeneration's initialise first
        /// </summary>
        public void Initialise()
        {
            _CheckInspectorVariables();

            HexDict = new Dictionary<Vector3Int, IHexagon>();
    
            GenerateMap();
    
            ConfigureTerrain();
    
            UpdateHexNeighbours();
    
            ConnectContentsToHex();
        }

        private void _CheckInspectorVariables()
        {
            if(HexagonPrefab == null)
            {
                Debug.LogError("No hexagon prefab set in MapGeneration");
            }

            if(MapRadius == 0)
            {
                Debug.LogError("MapRadius not set in Inspector");
            }
        }

        #endregion

        //----------------------------------------------------------------------------
        //            Generate Map
        //----------------------------------------------------------------------------

        #region GenerateMap

        /// <summary>
        /// Loops through all valid indexes within the radius
        /// and instantiates a hexagon
        /// </summary>
        public void GenerateMap()
        {
            for (int x = -MapRadius; x <= MapRadius; x++)
            {
                for (int y = Mathf.Max(-MapRadius, (-x - MapRadius)); y <= Mathf.Min(MapRadius, MapRadius - x); y++)
                {
                    int z = -x - y;
                    _InstantiateHexagon(new Vector3Int(x, y, z));
                }
            }
        }
    
        #region Implementation
    
        private void _InstantiateHexagon(Vector3Int coords)
        {
            Vector3 worldSpacePos = HexMath.HexCoordsToWorldSpace(coords);
    
            GameObject HexagonGO = Instantiate(HexagonPrefab, worldSpacePos, Quaternion.identity, gameObject.transform);
            HexagonGO.name = "Hex " + coords.ToString();
    
            Hexagon hexagon = HexagonGO.AddComponent<Hexagon>();
            hexagon.Initialise(coords, 1);
    
            HexDict.Add(coords, hexagon);
        }
    
        #endregion
    
    
        #endregion
    
    
        //----------------------------------------------------------------------------
        //          ConfigureTerrain
        //----------------------------------------------------------------------------
    
        #region Configure Terrain

        /// <summary>
        /// Map Layout implementation occurs here. <para/>
        /// Any hexagon not within a terrain bounds is deleted. <para/>
        /// Extra information about movement speed etc is set by the Terrain.
        /// </summary>
        void ConfigureTerrain()
        {
            _SetHexTerrainData();


            _DeleteUnusedHexagons();
        }

        #region Implementation

        private void _SetHexTerrainData()
        {
            TerrainMarker[] terrainArray = FindObjectsOfType<TerrainMarker>();

            _CheckTerrainMarkers(terrainArray);

            foreach (TerrainMarker terrainMarker in terrainArray)
            {
                terrainMarker.gameObject.GetComponent<ITerrain>().SetTerrainDataOnHexes();
            }
        }

        private void _CheckTerrainMarkers(TerrainMarker[] terrainArray)
        {
            if (terrainArray.Length == 0)
            {
                Debug.LogError("No TerrainMarkers in map.");
            }
        }

        //This is hard to make self documenting because of the ordering.
        //If a hex is not a part of the map, it should be destroyed.
        //However, you can't remove the hexagon from the HexDict while
        //iterating through the HexDict, it breaks the foreach loop
        //So there's an extra list to add the key to, in order to afterwards
        //remove that key from the hex dict

        /// <summary>
        /// Go through every hex in the HexDict and delete if it not part of the map
        /// </summary>
        private void _DeleteUnusedHexagons()
        {
            List<Vector3Int> destroyedHexes = new List<Vector3Int>();

            foreach (KeyValuePair<Vector3Int, IHexagon> hex in HexDict)
            {
                if (_ShouldDeleteThisHex(hex.Value))
                {
                    _DeleteHex(hex.Value);
                    destroyedHexes.Add(hex.Key);
                }
            }

            foreach (Vector3Int coords in destroyedHexes)
            {
                HexDict.Remove(coords);
            }
        }

        private bool _ShouldDeleteThisHex(IHexagon hex)
        {
            return ((IHexagonRestricted)hex).MyHexKeep.ShouldRemove;
        }

        private void _DeleteHex(IHexagon hex)
        {
            ((IHexagonRestricted)hex).MyHexKeep.DeleteHex();
        }

        #endregion

        #endregion


        //----------------------------------------------------------------------------
        //           UpdateHexNeighbours
        //----------------------------------------------------------------------------

        #region UpdateHexNeighbours

        /// <summary>
        /// After all hexes have been instantiated and validated
        /// check in each direction for a valid neighbour
        /// </summary>
        public void UpdateHexNeighbours()
        {
            List<Vector3Int> directions = GetDirections();
    
            foreach (KeyValuePair<Vector3Int, IHexagon> hex in HexDict)
            {
                HashSet<IHexagon> neighbours = new HashSet<IHexagon>();
                foreach (Vector3Int direction in directions)
                {
                    Vector3Int neighbourCoords = hex.Value.MyHexMap.CoOrds + direction;
    
                    if (HexDict.ContainsKey(neighbourCoords))
                    {
                        neighbours.Add(HexDict[neighbourCoords]);
                    }
                }
                ((IHexagonRestricted)hex.Value).MyHexMapRestricted.SetNeighbours(neighbours);
            }
        }
    
    
        /// <summary>
    /// Returns a list of each of the 6 cardinal directions
    /// of the hex map
    /// </summary>
    /// <returns></returns>
        public List<Vector3Int> GetDirections()
        {
            List<Vector3Int> directions = new List<Vector3Int>();
            directions.Add(new Vector3Int(1, -1, 0));
            directions.Add(new Vector3Int(1, 0, -1));
            directions.Add(new Vector3Int(0, 1, -1));
            directions.Add(new Vector3Int(-1, 1, 0));
            directions.Add(new Vector3Int(-1, 0, 1));
            directions.Add(new Vector3Int(0, -1, 1));
    
            return directions;
        }
    
        #endregion
    




        //----------------------------------------------------------------------------
        //           ConnectContentsToHex
        //----------------------------------------------------------------------------
    
        #region ConnectContentsToHex
    
        /// <summary>
    /// Initialise the IHexContents subsystem <para />
    /// This allows objects/map to interact
    /// </summary>
        public void ConnectContentsToHex()
        {
            IContentMarker[] hexContentMarkers = GameObject.FindObjectsOfType<IContentMarker>();
            foreach (IContentMarker hexContentMarker in hexContentMarkers)
            {
                GameObject go = hexContentMarker.gameObject;
                IContents contents = go.GetComponent<IContents>();
    
                //object told about hex
                contents.Location = HexDict[HexMath.CalculateHexCoords(contents.ContentTransform.position)];
                //hex told about object
                contents.Location.Contents = contents;
    
                //Move the object to the centre of the hex
                contents.ContentTransform.position = HexMath.HexCoordsToWorldSpace(contents.Location.MyHexMap.CoOrds);
                contents.Initialise();
            }
        }
    
        #endregion
    
    
    }
}


