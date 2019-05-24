using Plugins.Lanski.Subjective;
using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _MovementMechanics
    {
        void Add_location_on_start() =>
            OnGameplay()
            .When<Located_on_start>().ExceptWhen<Located>()
            .Do(() =>
            {
                var l = Add<Located>();
                l.Location = Transform.position;
            })
        ;
        
        void Remember_last_location() =>
            OnGameplay()
            .Do((Located l, Interpolates_location il) =>
            {
                il.LastLocation = l.Location;
            })
        ;

        void Apply_interpolated_location() =>
            OnPresentation()
            .Do((Located l, Interpolates_location il) =>
            {
                var ll = il.LastLocation;
                var cl = l.Location;
                var r = PresentationTimeRatio;
                
                Transform.position = Vector3.Lerp(ll, cl, r);
            })
        ;

        #region DEBUG
        void Debug_Remember_all_locations() =>
            OnGameplay()
            .When<Interpolates_location, Debugs_location_interpolation>()
            .Do((Located l) =>
            {
                var r = GetOrAdd<Remembers_all_locations>();
                r.Add(l.Location);
            })
        ;

        void Debug_Remember_all_transforms() =>
            OnPresentation()
            .When<Interpolates_location, Debugs_location_interpolation>()
            .Do(() =>
            {
                var r = GetOrAdd<Remembers_all_transform_positions>();
                r.Add(Transform.position);
            })
        ;
        #endregion
    }
}