using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableManager
{
    public void Initialise()
    {
        Hideable[] hideableArray = GameObject.FindObjectsOfType<Hideable>();
        CurrentlyHidden = new HashSet<Hideable>();
        foreach (Hideable hideable in hideableArray)
        {
            hideable.Initialise(this);
        }
    }

    public HashSet<Hideable> CurrentlyHidden;

    public void ResetForNewCamera(Vector3 position)
    {
        HashSet<Hideable> temp = new HashSet<Hideable>(CurrentlyHidden);
        foreach(Hideable hideable in temp)
        {
            hideable.ResetForNewCamera(position);
        }
    }

    public void SetAsHidden(Hideable hideable)
    {
        if(!CurrentlyHidden.Contains(hideable))
        {
            CurrentlyHidden.Add(hideable);
        }
    }

    public void SetAsUnhidden(Hideable hideable)
    {
        if(CurrentlyHidden.Contains(hideable))
        {
            CurrentlyHidden.Remove(hideable);
        }
    }

    public List<Hideable> Hideables;
}
