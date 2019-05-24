using System.Collections.Generic;
using Plugins.Lanski.Subjective;
using UnityEngine;

public class RemembersAllLocations: QComponent
{
    public List<Vector3> Locations;

    public void Add(Vector3 location)
    {
        if (Locations == null) Locations = new List<Vector3>(128);
        Locations.Add(location);
    }

    void OnDrawGizmos()
    {
        foreach (var l in Locations)
        {
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawCube(l, Vector3.one * 0.02f);
        }
    }
}
