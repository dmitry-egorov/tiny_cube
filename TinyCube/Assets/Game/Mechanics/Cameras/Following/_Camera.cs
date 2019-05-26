using UnityEngine;

namespace Game.Mechanics.Cameras
{
    public partial class _Camera
    {
        void Follow_target() =>
            Do((Follows_a_target ft) =>
            {
                var p = Transform.position;
                var tp = ft.Target.position;
                var s = ft.Strength;
                var tx = ft.X ? Mathf.Lerp(p.x, tp.x, s) : p.x;
                var ty = ft.Y ? Mathf.Lerp(p.y, tp.y, s) : p.y;
                var tz = ft.Z ? Mathf.Lerp(p.z, tp.z, s) : p.z;
                
                Transform.position = new Vector3(tx, ty, tz);
            })
        ;
    }
}