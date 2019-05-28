using UnityEngine;
using UnityEngine.Assertions;
using static Direction;

namespace Game.Mechanics.Movements
{
    public partial class _Movement
    {
        void Check_paths_prev_next_pointers_on_start() => // prev/next pointers
            ExceptWhen<Is_started>()
            .When<Checks_paths_consistency_on_start>()
            .Do(() =>
            {
                var ls = Object.FindObjectsOfType<Marks_a_waypoint_level>();
                foreach (var l in ls)
                {
                    var n = l.next;
                    if (n != null && n.prev != l) Debug.LogWarning("Inconsistent point", l);
                    var pr = l.prev;
                    if (pr != null && pr.next != l) Debug.LogWarning("Inconsistent point", l);
                }
            })
        ;
        void Check_paths_levels_order_on_start() => // prev/next pointers
            ExceptWhen<Is_started>()
            .When<Checks_paths_consistency_on_start>()
            .Do(() =>
            {
                var ps = Object.FindObjectsOfType<Marks_a_waypoint>();
                foreach (var p in ps)
                {
                    var h = float.MinValue;
                    foreach (var l in p.Levels)
                    {
                        var lh = l.height;
                        if (lh <= h)
                        {
                            Debug.LogWarning("Level is out of order", l);
                        }
                        h = lh;
                    }
                }
            })
        ;
        
        void Follow_closest_point_on_start() =>
            ExceptWhen<Is_started>().
            Do((Can_follow_a_path cf) =>
            {
                if (cf.follow_closest_point_on_start)
                {
                    var p = transform.position;
                    var ls = Object.FindObjectsOfType<Marks_a_waypoint_level>();
                    var /* min distance  */ md = float.MaxValue;
                    var /* closest level */ cl = default(Marks_a_waypoint_level);
                    foreach (var l in ls)
                    {
                        var d = (l.position - p).magnitude;
                        if (d < md)
                        {
                            md = d;
                            cl = l;
                        }
                    }

                    var fp = add<Follows_a_path>();
                    fp.level = cl;
                }
            })
        ;

        void Add_height_when_following_a_path() =>
            ExceptWhen<Has_height>()
            .Do((Follows_a_path fp) =>
            {
                var hh = add<Has_height>();
                hh.height = fp.path_height;
            })
        ;
        
        void Apply_paths_location() =>
            Do((Follows_a_path f, Has_height hh) =>
            {
                var l = get_or_add<Has_location>();
                l.location = (f, hh).location();
            })
        ;

        void Apply_paths_height_when_not_airborne() =>
            ExceptWhen<Is_airborne>()
            .Do((Follows_a_path f, Has_height l) =>
            {
                l.height = f.path_height;
            })
        ;

        void Apply_paths_rotation() =>
            Do((Can_follow_a_path cf, Can_rotate cr, Follows_a_path fp) =>
            {
                if (!cf.rotates)
                    return;

                var hr = get_or_add<Has_rotation>();
                var es = cr.exp_rotation_strength;
                var ls = cr.linear_rotation_speed;
                var dt = delta_time;

                var /* path's rotation */      pr = fp.rotation; 
                var /* subject's rotation */   sr = hr.rotation;
                var /* exponential rotation */ er = Quaternion.Lerp(sr, pr, es);
                var /* linear rotation */      lr = ls * dt;
                var /* the final rotation */   fr = Quaternion.RotateTowards(er, pr, lr);
                
                hr.rotation = fr;
            })
        ;

        void Apply_paths_movement_velocity() =>
            Do((Follows_a_path f, Moves m) =>
            {
                var dm = f.direction_multiplier();
                var ms = m.speed;
                var dt = delta_time;
                
                f.distance += ms * dm * dt;
            })
        ;

        void Stop_when_reaching_a_wall() =>
            Do((Can_follow_a_path cf, Follows_a_path fp, Has_height hh, Moves m) =>
            {
                var d = fp.direction;
                var s = (cf, fp, hh);
                
                if (s.intersection_at(d).is_empty)
                {
                    (cf, fp).reset_to(d);
                    m.Stop();
                }
            })
        ;

        void Apply_adjacent_intersections() =>
            Do((Can_follow_a_path cf, Follows_a_path fp, Has_height hh) =>
            {
                var ap = get_or_add<Has_paths_intersections>();

                var mr = cf.fall_detection_margin;
                ap.front = (cf, fp, hh).intersection_at(Front, mr);
                ap.back  = (cf, fp, hh).intersection_at(Back, mr);
            })
        ;

        void Switch_to_a_higher_intersected_path() =>
            Do((Can_follow_a_path cf, Follows_a_path fp, Has_paths_intersections pi) =>
            {
                var /* front intersection */ fi = pi.front;
                var /*  back intersection */ bi = pi.back;

                var /* front is empty */ fhv = fi.has_value;
                var /*  back is empty */ bhv = bi.has_value;
                
                if (!fhv && !bhv)
                    return;
                
                var /* first is higher */ fih = !bhv || fhv && fi.height >= bi.height;

                var mr = cf.fall_detection_margin;
                var hw = cf.half_width() - mr;

                var l = fih ? fi.level : bi.level;
                var d = fih ? fi.distance - hw : bi.distance + hw;
                fp.switch_to(l, d);
            })
        ;
    }
    
    public static partial class _Helpers
    {
        public static void reset_to(this (Can_follow_a_path cf, Follows_a_path fp) t, Direction d)
        {
            var hw = t.cf.half_width();
            t.fp.set_distance_from(d, hw);
        }


        public static PathIntersection intersection_at
        (
            this (Can_follow_a_path cf, Follows_a_path fp, Has_height hh) t, 
            Direction side, 
            float /* margin */ m = 0f
        )
        {
            var mp = side.multiplier();
            var d = t.fp.distance + mp * (t.cf.half_width() - m); 
            
            return (t.fp, t.hh).intersection_at(d);
        }
        
        public static PathIntersection intersection_at
        (
            this (Follows_a_path fp, Has_height hh) t, 
            float /* distance */ d
        )
        {
            var /* current levels */  ls = t.fp.levels;
            var /* path's length  */ pln = t.fp.path_length;
            
            using (ListPool<(Marks_a_waypoint_level l, float d)>.Borrow(out var nls))
            {
                if (d < 0) // over back bound
                {
                    foreach (var l in ls)
                    {
                        var /* prev level */ pl = l.prev;
                        if (pl == null) continue;
                        
                        var /* new distance */ nd = pl.length + d;
                        nls.Add((pl, nd));
                    }
                }
                else if (d < pln) // within bounds
                {
                    foreach (var l in ls)
                    {
                        if (l.next == null) continue;
                        nls.Add((l, d));
                    }
                }
                else // over front bound
                {
                    foreach (var l in t.fp.level.next.levels)
                    {
                        if (l.next == null) continue;
                        
                        var nd = d - pln;
                        nls.Add((l, nd));
                    }
                }

                // find and return highest path lower than the subject
                {
                    var /* subject's height */ sh = t.origin_height_at(d);
                    var /* max height */       mh = float.MinValue;
                    var /* highest level */    hl = default(Marks_a_waypoint_level);
                    var /* level's distance */ ld = 0.0f;
                
                    foreach (var (l, nd) in nls)
                    {
                        var lh = l.height_at(nd);
                    
                        if (lh > sh) continue;
                        if (lh <= mh) continue;
                    
                        mh = lh;
                        hl = l;
                        ld = nd;
                    }

                    return hl != null
                        ? new PathIntersection(hl, ld, mh)
                        : default
                    ;
                }
            }
        }

        public static float origin_height_at(this (Follows_a_path fp, Has_height hh) t, float /* distance */ d)
        {
            var (l, nd) = t.fp.path_at(d);
            return Mathf.Max(l.height_at(nd), t.hh.height);
        }

        public static Vector3 location(this (Follows_a_path f, Has_height hh) t)
        {
            var p = t.f.paths_location;
            var h = t.hh.height;
            return new Vector3(p.x, h, p.z);
        }
        
        public static float multiplier(this Direction d) => d == Front ? 1f : -1f;
    }
}