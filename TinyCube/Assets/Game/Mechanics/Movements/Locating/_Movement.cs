using Plugins.Lanski.Subjective;
using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _Movement
    {
        void Remember_last_location() =>
            Do((Has_location l, Interpolates_location il) =>
            {
                il.LastLocation = l.location;
            })
        ;

        void Apply_interpolated_location() =>
            Do((Has_location l, Interpolates_location il) =>
            {
                var ll = il.LastLocation;
                var cl = l.location;
                var tr = presentation_time_ratio;
                
                transform.position = Vector3.Lerp(ll, cl, tr);
            })
        ;

        void Debug_Remember_all_locations() =>
            When<Interpolates_location, Debugs_location_interpolation>()
            .Do((Has_location l) =>
            {
                var r = get_or_add<Remembers_all_locations>();
                r.Add(l.location);
            })
        ;

        void Debug_Remember_all_transforms() =>
            When<Interpolates_location, Debugs_location_interpolation>()
            .Do(() =>
            {
                var r = get_or_add<Remembers_all_transform_positions>();
                r.Add(transform.position);
            })
        ;
    }
}