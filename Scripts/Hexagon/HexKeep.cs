using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHexKeep
{
    void KeepHexagon();

    bool RemoveUnused();
}

public class HexKeep : MonoBehaviour, IHexKeep
{
    public void KeepHexagon()
    {
        Remove = false;
    }

    public bool Remove = true;

    public bool RemoveUnused()
    {
        if (Remove)
        {
            Destroy(gameObject);
        }
        return Remove;
    }

}
