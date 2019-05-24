using Plugins.UnityExtensions;
using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _MovementMechanics
    {
        void Apply_location_when_following_a_path() =>
            OnGameplay()
            .Do((Follows f) =>
            {
                var t = f.Path;
                if (!t.TryGetComponent(out Connects c)) 
                    return;

                var d = f.DistanceWalked;
                var p = c.GetPositionAt(d);
                var l = GetOrAdd<Located>();
                
                l.Location = p;
            })
        ;

        void Apply_rotation_when_following_a_path() =>
            OnGameplay()
            .Do((Follows f) =>
            {
                var t = f.Path;
                if (!t.TryGetComponent(out Connects c)) 
                    return;

                var r = GetOrAdd<Rotated>();
                var tr = c.GetRotation(f.Direction);
                var cr = r.Rotation;
                var st = f.RotationStrength;

                var nr = Quaternion.Lerp(cr, tr, st);
                var msp = f.MinRotationSpeed * DeltaTime;
                nr = Quaternion.RotateTowards(nr, tr, msp);
                r.Rotation = nr;
            })
        ;

        void Switch_follow_target_when_reaching_segments_end() =>
            OnGameplay()
            .When<Moves>()
            .Do((Follows f) =>
            {
                //Note: if the paths are short enough -- need to do it recursively
                
                var t = f.Path;
                if (!t.TryGetComponent(out Connects c)) 
                    return;
                
                var wd = f.DistanceWalked;
                var pd = c.GetDistance();
                if (wd > pd)
                {
                    if (!t.TryGetComponent(out Has_next_followed nf))
                        return;

                    var nt = nf.Next;
                    if (!nt.TryGetComponent(out Connects nc))
                        return;
                    
                    f.Path = nt;
                    if (nf.Side == ConnectionSide.Start)
                    {
                        f.DistanceWalked -= pd;
                    }
                    else
                    {
                        f.Reverse();
                        f.DistanceWalked = nc.GetDistance() - (wd - pd);
                    }
                }
                else if (wd < 0)
                {
                    if (!t.TryGetComponent(out Has_prev_followed pf))
                        return;
                    
                    var nt = pf.Prev;
                    if (!nt.TryGetComponent(out Connects nc))
                        return;

                    f.Path = nt;

                    if (pf.Side == ConnectionSide.End)
                    {
                        f.DistanceWalked = nc.GetDistance() + wd;
                    }
                    else
                    {
                        f.Reverse();
                        f.DistanceWalked = -wd;
                    }
                }
            })
        ;

        void Stop_when_reaching_segments_end_and_theres_no_next_segment() =>
            OnGameplay()
            .Do((Follows f, Moves m) =>
            {
                //Note: if the paths are short enough -- need to do it recursively
                
                var t = f.Path;
                if (!t.TryGetComponent(out Connects c)) 
                    return;
                
                var wd = f.DistanceWalked;
                var pd = c.GetDistance();
                if (wd > pd)
                {
                    if (t.TryGetComponent(out Has_next_followed nf))
                        return;

                    m.Stop();
                    f.DistanceWalked = pd;
                }
                else if (wd < 0)
                {
                    if (t.TryGetComponent(out Has_prev_followed pf))
                        return;
                    
                    m.Stop();
                    f.DistanceWalked = 0;
                }
            })
        ;
        
        static float GetVelocity(Follows f, Moves m) => 
            m.Speed * (f.Direction == Direction.Forward ? 1f : -1f)
        ;
    }
}