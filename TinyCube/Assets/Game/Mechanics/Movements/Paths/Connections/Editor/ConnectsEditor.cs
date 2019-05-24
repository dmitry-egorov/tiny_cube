using UnityEditor;
using UnityEngine;

public static class ConnectsEditor
{
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmoForConnects(Connects scr, GizmoType gizmoType)
    {
        var s = scr.Start;
        var e = scr.End;
        if (s == null || e == null) return;
        
        var sp = s.transform.position;
        var ep = e.transform.position;

        Gizmos.DrawLine(sp, ep);

        var mp = Vector3.Lerp(sp, ep, 0.5f);
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(mp, Vector3.one * 0.4f);
    }
    
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmoForCanBeConnected(Can_be_connected scr, GizmoType gizmoType)
    {
        var p = scr.transform.position;
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(p, Vector3.one * 0.5f);
    }
}
