using Plugins.Lanski.Subjective;
using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _Movement
    {
        void Add_location_on_start() =>
            When<Located_on_start>().ExceptWhen<Has_location>()
            .Do(() =>
            {
                var l = Add<Has_location>();
                l.Location = Transform.position;
            })
        ;
        
        void Remember_last_location() =>
            Do((Has_location l, Interpolates_location il) =>
            {
                il.LastLocation = l.Location;
            })
        ;

        void Apply_interpolated_location() =>
            Do((Has_location l, Interpolates_location il) =>
            {
                var ll = il.LastLocation;
                var cl = l.Location;
                var tr = PresentationTimeRatio;
                
                Transform.position = Vector3.Lerp(ll, cl, tr);
            })
        ;

        void Debug_Remember_all_locations() =>
            When<Interpolates_location, Debugs_location_interpolation>()
            .Do((Has_location l) =>
            {
                var r = GetOrAdd<Remembers_all_locations>();
                r.Add(l.Location);
            })
        ;

        void Debug_Remember_all_transforms() =>
            When<Interpolates_location, Debugs_location_interpolation>()
            .Do(() =>
            {
                var r = GetOrAdd<Remembers_all_transform_positions>();
                r.Add(Transform.position);
            })
        ;
    }
}