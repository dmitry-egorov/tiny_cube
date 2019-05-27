using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _MovementEditor
    {
        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmoForFollower(Follows_a_path fp, GizmoType gizmoType)
        {
            var l = fp.level;
            var p = l.position;
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(p + Vector3.up * 0.5f, Vector3.one * 0.4f);

            var n = l.next;
            if (n == null)
                return;

            var np = n.position;
        
            Gizmos.DrawLine(p + Vector3.up * 0.1f, np + Vector3.up * 0.1f);
        }
        
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        static void DrawGizmoForWaypoint(Marks_a_waypoint w, GizmoType gizmoType)
        {
            var p = w.GetPosition();

            var ls = w.Levels;
            if (ls.Length == 0)
                return;

            var max_height = ls.Max(x => x.height);
            var lp = new Vector3(p.x, max_height, p.z);
        
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawLine(p, lp);
        }
        
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
        static void DrawGizmoForWaypoint(Marks_a_waypoint_level l, GizmoType gizmoType)
        {
            var p = l.position;
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawCube(p, Vector3.one * 0.4f);

            var n = l.next;
            if (n == null)
                return;

            var np = n.position;
        
            Gizmos.DrawLine(p, np);
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        static void DrawGizmoForIntersections(Has_paths_intersections pi, GizmoType gizmoType)
        {
            var h = pi.GetComponent<Has_height>().height;
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            
            var n = pi.front;
            var nl = n.level;
            var nd = n.distance;
            var /* next intersection start */ ns = nl.position_at(nd);
            var /* next intersection end   */ ne = new Vector3(ns.x, h, ns.z);
            Gizmos.DrawLine(ns, ne);
            
            var p = pi.back;
            var pl = p.level;
            var pd = p.distance;
            var /* prev intersection start */ ps = pl.position_at(pd);
            var /* prev intersection end   */ pe = new Vector3(ps.x, h, ps.z);
            Gizmos.DrawLine(ps, pe);
        }
    }
}