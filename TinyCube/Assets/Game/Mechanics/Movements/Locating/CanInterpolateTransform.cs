using UnityEngine;

public class CanInterpolateTransform : QComponent
{
    void OnDrawGizmos()
    {
        var rl = GetComponent<RemembersAllLocations>();
        if (rl == null)
            return;

        var rt = GetComponent<RemembersAllTransformPositions>();
        if (rt == null)
            return;

        foreach (var l in rl.Locations)
        {
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawCube(l, Vector3.one * 0.02f);
        }

        foreach (var p in rt.Positions)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(p, Vector3.one * 0.02f);
        }
    }
}