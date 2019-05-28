using Plugins.Lanski.Subjective;
using UnityEngine;

namespace Game.Mechanics.Movements.Locating
{
    public class _LocatingMechanics: SubjectiveMechanics
    {
        public static void remember_last_location() =>
            act((Has_location l, Interpolates_location il) =>
            {
                il.LastLocation = l.location;
            })
        ;

        public static void apply_interpolated_location() =>
            act((Has_location l, Interpolates_location il) =>
            {
                var ll = il.LastLocation;
                var cl = l.location;
                var tr = presentation_time_ratio;
                
                transform.position = Vector3.Lerp(ll, cl, tr);
            })
        ;

        public static void DEBUG_remember_all_locations() =>
            when<Interpolates_location, Debugs_location_interpolation>()
            .act((Has_location l) =>
            {
                var r = get_or_add<Remembers_all_locations>();
                r.Add(l.location);
            })
        ;

        public static void DEBUG_remember_all_transforms() =>
            when<Interpolates_location, Debugs_location_interpolation>()
            .act(() =>
            {
                var r = get_or_add<Remembers_all_transform_positions>();
                r.Add(transform.position);
            })
        ;
    }
}