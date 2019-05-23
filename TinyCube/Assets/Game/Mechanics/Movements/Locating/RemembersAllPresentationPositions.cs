using System.Collections.Generic;
using UnityEngine;

public class RemembersAllPresentationPositions : QComponent
{
    public List<Vector3> Positions;

    public void Add(Vector3 position)
    {
        if (Positions == null) Positions = new List<Vector3>(128);
        Positions.Add(position);
    }
}