using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    public struct MouseDrag
    {
        public Vector2 start_position;
        public Vector2 end_position;

        public float distance => _distance ?? (_distance = offset.magnitude).Value;
        public Vector2 offset => _offset ?? (_offset = end_position - start_position).Value;
        public Vector3 world_offset => _world_direction ?? (_world_direction = calculate_world_direction()).Value;
        public float world_distance => world_offset.magnitude;

        Vector3 calculate_world_direction()
        {
            var sp = start_position;
            var sp3 = new Vector3(sp.x, sp.y, 0);
            var ep = end_position;
            var ep3 = new Vector3(ep.x, ep.y, 0);
            var sr = Camera.main.ScreenPointToRay(sp3);
            var er = Camera.main.ScreenPointToRay(ep3);
            
            var p = new Plane(Vector3.up, 0);
            p.Raycast(sr, out var se);
            p.Raycast(er, out var ee);

            var swp = sr.GetPoint(se);
            var ewp = er.GetPoint(ee);

            return ewp - swp;
        }
        
        float? _distance;
        Vector2? _offset;
        Vector3? _world_direction;
    }
}