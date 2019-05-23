using System.Collections.Generic;
using UnityEngine;

public class RemembersAllLocations: QComponent
{
    public List<Vector3> Locations;

    public void Add(Vector3 location)
    {
        if (Locations == null) Locations = new List<Vector3>(128);
        Locations.Add(location);
    }
}
