using UnityEditor;
using UnityEngine;

public class HiderEditor
{
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmoForCanBeConnected(Hider scr, GizmoType gizmoType)
    {
        var t = scr.transform;
        var p = t.position;
        var s = t.localScale;
        
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(p, s);
    }
}
