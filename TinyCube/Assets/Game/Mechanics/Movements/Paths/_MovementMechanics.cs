using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _MovementMechanics
    {
        void Apply_paths_planar_location() =>
            OnGameplay()
            .Do((Follows_a_path f) =>
            {
                var p = f.GetPosition(); 
                var l = GetOrAdd<Has_location>();

                l.Location = new Vector3(p.x, l.GetHeight(), p.z);
            })
        ;

        void Apply_paths_height_when_not_airborne() =>
            OnGameplay()
            .ExceptWhen<Is_airborne>()
            .Do((Follows_a_path f, Has_location l) =>
            {
                var ph = f.GetHeight();
                l.SetHeight(ph);
            })
        ;

        void Apply_paths_rotation() =>
            OnGameplay()
            .Do((Follows_a_path fp, Can_rotate cr) =>
            {
                if (!fp.Rotates)
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

        void Handle_walking_beyond_the_path() =>
            OnGameplay()
            .Do((Follows_a_path fp, Moves m) =>
            {
                while (true)
                {
                    if 
                    (
                        !fp.TryGetWalkedBeyond
                        (
                            out /* side which the subject has walked beyond */ var s, 
                            out /* extra distance */ var ed
                        )
                    )
                        break;

                    if // the side is not connected
                    (
                        !fp.TryGetConnectedAt
                        (
                            s,
                            out var /* new path */ np,
                            out var /* new side */ ns
                        )
                    )
                    {
                        m.Stop();
                        fp.ResetTo(s);
                        break;
                    }

                    fp.Path = np;

                    if (s == ns)
                    {
                        fp.Reverse();
                    }

                    fp.SetDistanceFrom(ns, ed);
                }
            })
        ;

        void Apply_paths_movement_velocity() =>
            OnGameplay()
            .Do((Follows_a_path f, Moves m) =>
            {
                var /* direction multiplier */ dm = f.Direction == Direction.Forward ? 1f : -1f;
                var ms = m.Speed;
                var dt = DeltaTime;
                
                f.DistanceWalked += ms * dm * dt;
            })
        ;

//        void Update_paths_collider() =>
//            OnGameplay()
//            .Do((Marks_a_path p) =>
//            {
//                var /* collider */         c = Subject.GetOrAddComponent<BoxCollider>();
//                var /* start position */  sp = p.start.position;
//                var /* end position */    ep = p.end.position;
//                var /* middle position */ mp = (sp + ep) * 0.5f;
//                var /* rotation */         r = p.GetRotation(Direction.Forward);
//                var w = p.width;
//                var h = p.height;
//                var d = p.GetLength();
//
//                Transform.position = mp;
//                Transform.rotation = r;
//                c.center = Vector3.zero;
//
//                c.size = new Vector3(w, h, d);
//            })
//        ;
    }
}