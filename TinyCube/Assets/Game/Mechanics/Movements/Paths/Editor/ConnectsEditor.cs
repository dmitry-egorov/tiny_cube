using UnityEditor;
using UnityEngine;

public static class ConnectsEditor
{
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmoForPath(Marks_a_path scr, GizmoType gizmoType)
    {
        var s = scr.start;
        var e = scr.end;
        if (s == null || e == null) return;
        
        var sp = s.GetPosition();
        var ep = e.GetPosition();

        Gizmos.DrawLine(sp, ep);

        var mp = Vector3.Lerp(sp, ep, 0.5f);
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(mp, Vector3.one * 0.4f);
    }
    
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmoForPoint(Marks_a_point scr, GizmoType gizmoType)
    {
        var p = scr.transform.position;
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(p, Vector3.one * 0.5f);
    }
    
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawGizmoForConnects(Connects_to scr, GizmoType gizmoType)
    {
        var s = scr.GetComponent<Marks_a_point>();
        var e = scr.point;
        if (s == null || e == null) return;
        
        var sp = s.GetPosition();
        var ep = e.GetPosition();

        Gizmos.color = new Color(0, 0.5f, 0, 0.5f);
        Gizmos.DrawLine(sp, ep);
    }
}
