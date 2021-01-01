using System.Collections.Generic;
using UnityEngine;

public interface IMapGeneration
{
    /// <summary>
    /// Dictionary of the hexagons of the map, with the coords as the key
    /// </summary>
    Dictionary<Vector3Int, IHexagon> HexDict { get; }

    void Initialise();
}

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


public class MapGeneration : MonoBehaviour, IMapGeneration
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
    /// Furthest point from centre to generate hex at
    /// </summary>
    [Tooltip("Default Value: 10")]
    public int MapRadius = 10;

    #endregion


    void Awake()
    {
        _RegisterWithDependancyManager();
    }

    private void _RegisterWithDependancyManager()
    {
        GameObject dependancyGO = FindObjectOfType<DependancyManagerMarker>().gameObject;
        IBattleDependancyManager dependancyManager = dependancyGO.GetComponent<IBattleDependancyManager>();
        dependancyManager.RegisterMapGeneration(this);
    }

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
        HexDict = new Dictionary<Vector3Int, IHexagon>();

        GenerateMap();

        ConfigureTerrain();

        UpdateHexNeighbours();

        ConnectContentsToHex();
    }

    #region Implementation

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
        for(int x = -MapRadius; x <= MapRadius; x++)
        {
           for(int y = Mathf.Max(-MapRadius, (-x - MapRadius)); y <= Mathf.Min(MapRadius, MapRadius - x); y++)
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
    void ConfigureTerrain()
    {
        TerrainMarker[] terrainArray = FindObjectsOfType<TerrainMarker>();
        foreach (TerrainMarker terrainMarker in terrainArray)
        {
            terrainMarker.gameObject.GetComponent<ITerrain>().Configure();
        }

        List<Vector3Int> destroyedHexes = new List<Vector3Int>();

        foreach (KeyValuePair<Vector3Int, IHexagon> hex in HexDict)
        {
            if (((IHexagonRestricted)(hex.Value)).MyHexKeep.RemoveUnused())
            {
                destroyedHexes.Add(hex.Key);
            }
        }

        foreach (Vector3Int coords in destroyedHexes)
        {
            HexDict.Remove(coords);
        }

    }

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

        foreach(KeyValuePair<Vector3Int, IHexagon> hex in HexDict)
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
        foreach(IContentMarker hexContentMarker in hexContentMarkers)
        {
            GameObject go = hexContentMarker.gameObject;
            IContents contents = go.GetComponent<IContents>();

            //object told about hex
            contents.Location = HexDict[HexMath.CalculateHexCoords(contents.ContentTransform.position)];
            //hex told about object
            contents.Location.Contents = contents;

            //Move the object to the centre of the hex
            contents.ContentTransform.position = HexMath.HexCoordsToWorldSpace(contents.Location.MyHexMap.CoOrds);
        }
    }

    #endregion





    #endregion




    #endregion

}
