using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _Movement
    {
        void Apply_paths_planar_location() =>
            Do((Follows_a_path f) =>
            {
                var p = f.GetPosition(); 
                var l = GetOrAdd<Has_location>();

                l.Location = new Vector3(p.x, l.GetHeight(), p.z);
            })
        ;

        void Apply_paths_height_when_not_airborne() =>
            ExceptWhen<Is_airborne>()
            .Do((Follows_a_path f, Has_location l) =>
            {
                var ph = f.GetHeight();
                l.SetHeight(ph);
            })
        ;

        void Apply_paths_rotation() =>
            Do((Can_follow_a_path cf, Can_rotate cr, Follows_a_path fp) =>
            {
                if (!cf.rotates)
                    return;

                var hr = GetOrAdd<Has_rotation>();
                var st = cr.RotationExpStrength;
                var ms = cr.MinRotationSpeed;
                var dt = DeltaTime;

                var /* path's rotation */       pr = fp.GetRotation(); 
                var /* subject's rotation */    sr = hr.Rotation;
                var /* exponential rotation */  er = Quaternion.Lerp(sr, pr, st);
                var /* linear rotation delta */ dr = ms * dt;
                var /* the final rotation */    fr = Quaternion.RotateTowards(er, pr, dr);
                hr.Rotation = fr;
            })
        ;

        void Apply_paths_movement_velocity() =>
            Do((Follows_a_path f, Moves m) =>
            {
                var /* direction multiplier */ dm = f.direction == Direction.Forward ? 1f : -1f;
                var ms = m.Speed;
                var dt = DeltaTime;
                
                f.distance += ms * dm * dt;
            })
        ;
        
        void Stop_when_reaching_an_end_of_an_unconnected_path() =>
            Do((Can_follow_a_path cf, Follows_a_path fp, Moves m, Has_location l) =>
            {
                var /* half width */ hw = cf.width / 2.0f;
                if (fp.direction == Direction.Forward && fp.distance + hw >= fp.GetLength())
                {
                    if (!fp.TryGetConnectedPointAt(PathSide.End, out var cp) || cp.GetHeight() > l.GetHeight())
                    {
                        m.Stop();
                        fp.distance = fp.GetLength() - hw;
                    }
                }
                
                if (fp.direction == Direction.Backward)
                {
                    if
                    (
                        fp.distance - hw <= 0 &&
                        !fp.TryGetConnectedAt(PathSide.Start, out _, out _)
                    )
                    {
                        m.Stop();
                        fp.distance = hw;
                    }
                }
            })
        ;

//        void Handle_walking_beyond_the_path() =>
//            Do((Follows_a_path fp, Moves m) =>
//            {
//                while (true)
//                {
//                    if 
//                    (
//                        !fp.TryGetWalkedBeyond
//                        (
//                            out /* side which the subject has walked beyond */ var s, 
//                            out /* extra distance */ var ed
//                        )
//                    )
//                        break;
//
//                    if // the side is not connected
//                    (
//                        !fp.TryGetConnectedAt
//                        (
//                            s,
//                            out var /* new path */ np,
//                            out var /* new side */ ns
//                        )
//                    )
//                    {
//                        m.Stop();
//                        fp.ResetTo(s);
//                        break;
//                    }
//
//                    fp.path = np;
//
//                    if (s == ns)
//                    {
//                        fp.Reverse();
//                    }
//
//                    fp.SetDistanceFrom(ns, ed);
//                }
//            })
//        ;
        
    }
}