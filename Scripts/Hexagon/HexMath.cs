using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------
//              Class Description
//----------------------------------------------------------------------------
//
// Collection of static functions for doing hex math
//

/// <summary>
/// Collection of static functions for doing hex math
/// </summary>
public class HexMath
{
    /// <summary>
    /// Find distance between 2 hexagons in hexes
    /// </summary>
    public static int FindDistance(IHexagon hex_a, IHexagon hex_b)
    {
        Vector3Int coords_a = hex_a.MyHexMap.CoOrds;
        Vector3Int coords_b = hex_b.MyHexMap.CoOrds;
        return Mathf.Max(Mathf.Abs(coords_a.x - coords_b.x), Mathf.Abs(coords_a.y - coords_b.y), Mathf.Abs(coords_a.z - coords_b.z));
    }

    /// <summary>
    /// Converts world position to hex coords <para/>
    /// Ignores y position
    /// </summary>
    public static Vector3Int CalculateHexCoords(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / 0.75f);

        int w = Mathf.RoundToInt(position.z / 0.433f);

        int y = -(x - w) / 2;
        int z = -x - y;
        return new Vector3Int(x, y, z);
    }

    public static Vector3 HexCoordsToWorldSpace(Vector3Int coords)
    {
        float x_loc = 0, z_loc = 0;
        x_loc += coords.x * 0.75f;
        z_loc += coords.y * 0.433f;
        z_loc -= coords.z * 0.433f;
        return new Vector3(x_loc, 0, z_loc);
    }
}
