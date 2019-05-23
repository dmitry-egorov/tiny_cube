using UnityEngine;

public class CanInterpolateTransform : QComponent
{
    void OnDrawGizmos()
    {
        var rl = GetComponent<RemembersAllLocations>();
        if (rl != null)
        {
            foreach (var l in rl.Locations)
            {
                Gizmos.color = new Color(0, 0, 1, 0.5f);
                Gizmos.DrawCube(l, Vector3.one * 0.02f);
            }
        }
        
        var rt = GetComponent<RemembersAllTransformPositions>();
        if (rt != null)
        {

            foreach (var p in rt.Positions)
            {
                Gizmos.color = new Color(0, 1, 0, 0.5f);
                Gizmos.DrawCube(p + Vector3.up * 0.03f, Vector3.one * 0.02f);
            }
        }
        
        var rp = GetComponent<RemembersAllPresentationPositions>();
        if (rp != null)
        {

            foreach (var p in rt.Positions)
            {
                Gizmos.color = new Color(1, 0, 0, 0.5f);
                Gizmos.DrawCube(p + Vector3.up * 0.06f, Vector3.one * 0.02f);
            }
        }
    }
}