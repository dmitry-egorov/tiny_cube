using System.Linq;
using UnityEditor;
using UnityEngine;

public static class WaypointEditor
{
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmoForWaypoint(Marks_a_waypoint_level l, GizmoType gizmoType)
    {
        var p = l.GetPosition();
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(p, Vector3.one * 0.4f);

        var n = l.next;
        if (n == null)
            return;

        var np = n.GetPosition();
        
        Gizmos.DrawLine(p, np);
    }
    
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawGizmoForWaypoint(Marks_a_waypoint w, GizmoType gizmoType)
    {
        var p = w.GetPosition();

        var ls = w.Levels;
        if (ls.Length == 0)
            return;

        var max_height = ls.Max(x => x.GetHeight());
        var lp = new Vector3(p.x, max_height, p.z);
        
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawLine(p, lp);
    }
}
