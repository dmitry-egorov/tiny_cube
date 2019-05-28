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
            var p = w.position;

            var ls = w.Levels;
            if (ls.Length == 0)
                return;

            // assuming they're in order of height
            var min_height = ls[0].height;
            var max_height = ls[ls.Length - 1].height;
            var fp = new Vector3(p.x, min_height, p.z);
            var lp = new Vector3(p.x, max_height, p.z);
        
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawLine(fp, lp);
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
            
            var f = pi.front;
            if (f.has_value)
            {
                var fl = f.level;
                var fd = f.distance;
                var /* next intersection start */ fs = fl.position_at(fd);
                var /* next intersection end   */ fe = new Vector3(fs.x, h, fs.z);
                Gizmos.DrawLine(fs, fe);
            }

            var b = pi.back;
            if (b.has_value)
            {
                var bl = b.level;
                var bd = b.distance;
                var /* prev intersection start */ bs = bl.position_at(bd);
                var /* prev intersection end   */ be = new Vector3(bs.x, h, bs.z);
                Gizmos.DrawLine(bs, be);                
            }
        }
    }
}