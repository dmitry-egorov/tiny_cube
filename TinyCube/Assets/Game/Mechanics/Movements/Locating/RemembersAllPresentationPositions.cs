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

    void OnDrawGizmos()
    {
        foreach (var p in Positions)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(p + Vector3.up * 0.06f, Vector3.one * 0.02f);
        }
    }
}