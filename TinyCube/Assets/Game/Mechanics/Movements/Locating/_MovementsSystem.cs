using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _MovementsSystem
    {
        void Add_location() =>
            OnGameplay()
            .When<LocatedOnStart>().ExceptWhen<Located>()
            .Do(() =>
            {
                var l = Add<Located>();
                l.Location = Transform.position;
            })
        ;
        
        void Remember_last_location() =>
            OnGameplay()
            .When<LocatedOnStart, InterpolatesLocation>()
            .Do((Located l) =>
            {
                var r = GetOrAdd<RemembersLastLocation>();
                r.LastLocation = l.Location;
            })
        ;

        void Apply_interpolated_location() =>
            OnPresentation()
            .When<LocatedOnStart, InterpolatesLocation>()
            .Do((Located l, RemembersLastLocation rll) =>
            {
                var ll = rll.LastLocation;
                var cl = l.Location;
                var r = PresentationTimeRatio;
                
                Transform.position = Vector3.Lerp(ll, cl, r);
            })
        ;

        #region DEBUG
        void Remember_all_locations() =>
            OnGameplay()
            .When<InterpolatesLocation, DebugsLocationInterpolation>()
            .Do((Located l) =>
            {
                var r = GetOrAdd<RemembersAllLocations>();
                r.Add(l.Location);
            })
        ;

        void Remember_all_transforms() =>
            OnPresentation()
            .When<InterpolatesLocation, DebugsLocationInterpolation>()
            .Do(() =>
            {
                var r = GetOrAdd<RemembersAllTransformPositions>();
                r.Add(Transform.position);
            })
        ;
        
        #endregion
    }
}