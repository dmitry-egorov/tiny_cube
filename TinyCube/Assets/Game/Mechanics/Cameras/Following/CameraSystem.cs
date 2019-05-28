using Plugins.Lanski.Subjective;
using UnityEngine;

namespace Game.Mechanics.Cameras.Following
{
    public class FollowingMechanics: SubjectiveMechanics
    {
        public static void follow_target() =>
        act((Follows_a_target ft) =>
        {
            var p = transform.position;
            var tp = ft.Target.position;
            var s = ft.Strength;
            var tx = ft.X ? Mathf.Lerp(p.x, tp.x, s) : p.x;
            var ty = ft.Y ? Mathf.Lerp(p.y, tp.y, s) : p.y;
            var tz = ft.Z ? Mathf.Lerp(p.z, tp.z, s) : p.z;
            
            transform.position = new Vector3(tx, ty, tz);
        });
    }
}