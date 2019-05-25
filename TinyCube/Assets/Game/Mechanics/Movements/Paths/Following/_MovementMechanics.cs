using Plugins.UnityExtensions;
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
                var l = GetOrAdd<Located>();

                l.Location = new Vector3(p.x, l.GetHeight(), p.z);
            })
        ;


        void Apply_path_height_when_not_airborne() =>
            OnGameplay()
            .ExceptWhen<Is_airborne>()
            .Do((Follows_a_path f, Located l) =>
            {
                var ph = f.GetHeight();
                l.SetHeight(ph);
            })
        ;

        void Apply_paths_rotation() =>
            OnGameplay()
            .Do((Follows_a_path f, Can_rotate rf) =>
            {
                if (!f.Rotates)
                    return;

                var /* path's rotation */ pr = f.GetRotation(); 

                var r = GetOrAdd<Rotated>();
                var st = rf.RotationExpStrength;
                var ms = rf.MinRotationSpeed;
                var dt = DeltaTime;

                var /* current rotation */     cr = r.Rotation;
                var /* exponential rotation */ er = Quaternion.Lerp(cr, pr, st);
                var /* rotation delta */       rd = ms * dt;
                var /* the final rotation */   fr = Quaternion.RotateTowards(er, pr, rd);
                r.Rotation = fr;
            })
        ;

        void Handle_reaching_paths_segments_end() =>
            OnGameplay()
            .Do((Follows_a_path f, Moves m) =>
            {
                while (true)
                {
                    var /* path's distance */ pd = f.GetDistance();
                    
                    var wd = f.DistanceWalked;
                    if (wd > pd) // walked past the end
                    {
                        if // the end is not connected
                        (
                            !f.TryGetNext
                            (
                                out var /* new path */ np, 
                                out var /* connection side */ cs 
                            ) 
                        )
                        {
                            m.Stop();
                            f.DistanceWalked = pd;
                            break;
                        }

                        f.Path = np;
                        
                        var /* extra distance */ ed = wd - pd;
                        
                        if (cs == ConnectionSide.Start)
                        {
                            f.DistanceWalked = ed;
                        }
                        else
                        {
                            f.Reverse();
                            f.DistanceWalked = np.GetDistance() - ed;
                        }
                        
                        continue;
                    }

                    if (wd < 0) // walked past the beginning
                    {
                        if // the beginning is not connected
                        (
                            !f.TryGetPrev
                            (
                                out var /* new path */ np, 
                                out var /* connection side */ cs
                            ) 
                        )
                        {
                            m.Stop();
                            f.DistanceWalked = 0;
                            break;
                        }
                    
                        f.Path = np;
                        var /* extra distance */ ed = -wd;

                        if (cs == ConnectionSide.End)
                        {
                            f.DistanceWalked = np.GetDistance() - ed;
                        }
                        else
                        {
                            f.Reverse();
                            f.DistanceWalked = ed;
                        }
                        
                        continue;
                    }

                    break;
                }
            })
        ;
        
        void Apply_paths_movement_velocity() =>
            OnGameplay()
            .Do((Follows_a_path f, Moves m) =>
            {
                f.DistanceWalked += GetVelocity(f, m) * DeltaTime;
            })
        ;

        void Update_path_colliders() =>
            OnGameplay()
            .Do((Marks_a_path p) =>
            {
                var /* collider */ c = Subject.GetOrAddComponent<BoxCollider>();
                
                var /* start position */  sp = p.start.position;
                var /* end position */    ep = p.end.position;
                var /* middle position */ mp = (sp + ep) * 0.5f;
                var /* rotation */         r = p.GetRotation(Direction.Forward);

                Transform.position = mp;
                Transform.rotation = r;
                c.center = Vector3.zero;
                c.size = new Vector3(p.width, p.height, p.GetDistance());
            })
        ;

        
        static float GetVelocity(Follows_a_path f, Moves m) => 
            m.Speed * (f.Direction == Direction.Forward ? 1f : -1f)
        ;
    }
}