using Plugins.Lanski.NullableExtensions;
using UnityEngine;
using static Direction;

namespace Game.Mechanics.Movements
{
    public partial class _Movement
    {
        void Check_paths_consistency() =>
            ExceptWhen<Is_started>()
            .When<Checks_paths_consistency_on_start>()
            .Do(() =>
            {
                var ps = Object.FindObjectsOfType<Marks_a_waypoint_level>();
                foreach (var p in ps)
                {
                    var n = p.next;
                    if (n != null && n.prev != p) Debug.LogWarning("Inconsistent point", p);
                    var pr = p.prev;
                    if (pr != null && pr.next != p) Debug.LogWarning("Inconsistent point", p);
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
                l.location = (f, hh).position();
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
                
                if
                (
                    d == Front && s.front_intersection().is_empty() ||
                    d == Back  && s. back_intersection().is_empty()
                )
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
                var fd = (cf, fp).front_distance() - mr;
                var bd = (cf, fp).back_distance() + mr;
                
                //Note: assumes the wall stopping is already performed, and so the intersection exists 
                ap.front = (fp, hh).intersection_at(fd).Value;
                ap.back  = (fp, hh).intersection_at(bd).Value;
            })
        ;

        void Switch_to_a_higher_intersected_path() =>
            Do((Can_follow_a_path cf, Follows_a_path fp, Has_paths_intersections pi) =>
            {
                var /* front intersection */ fi = pi.front;
                var /*  back intersection */ bi = pi.back;
                var /* first is higher    */ fh = fi.height >= bi.height;

                var mr = cf.fall_detection_margin;
                var hw = cf.half_width() - mr;

                var l = fh ? fi.level : bi.level;
                var d = fh ? fi.distance - hw : bi.distance + hw;
                fp.switch_to(l, d);
            })
        ;
    }
    
    public static partial class _Helpers
    {
        public static float front_distance(this (Can_follow_a_path cf, Follows_a_path fp) x) => 
            x.fp.distance + x.cf.half_width()
        ;

        public static float back_distance(this (Can_follow_a_path cf, Follows_a_path fp) x) => 
            x.fp.distance - x.cf.half_width()
        ;

        public static void reset_to(this (Can_follow_a_path cf, Follows_a_path fp) x, Direction d)
        {
            var hw = x.cf.half_width();
            x.fp.set_distance_from(d, hw);
        }


        public static PathIntersection? front_intersection
        (
            this (Can_follow_a_path cf, Follows_a_path fp, Has_height hh) x 
        )
        {
            var fd = (x.cf, x.fp).front_distance();
            return intersection_at((x.fp, x.hh), fd);
        }
        
        public static PathIntersection? back_intersection
        (
            this (Can_follow_a_path cf, Follows_a_path fp, Has_height hh) x 
        )
        {
            var bd = (x.cf, x.fp).back_distance();
            return intersection_at((x.fp, x.hh), bd);
        }
            
        public static PathIntersection? intersection_at(this (Follows_a_path fp, Has_height hh) x, float /* distance */ d)
        {
            var /* current levels */  ls = x.fp.levels;
            var /* path's length  */ pln = x.fp.path_length;
            
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
                    foreach (var l in x.fp.level.next.levels)
                    {
                        if (l.next == null) continue;
                        
                        var nd = d - pln;
                        nls.Add((l, nd));
                    }
                }

                {
                    var /* subject's height */ sh = x.hh.height;
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
                            ? new PathIntersection {distance = ld, level = hl, height = mh}
                            : (PathIntersection?) null
                        ;
                }
            }
        }
        
        public static Vector3 position(this (Follows_a_path f, Has_height hh) x)
        {
            var p = x.f.position;
            return new Vector3(p.x, x.hh.height, p.z);
        }
        
        public static float multiplier(this Direction d) => d == Front ? 1f : -1f;
    }
}