using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _MovementsSystem
    {
        void Add_location() =>
            Gameplay()
            .Includes<LocatedOnStart>().Excludes<Located>()
            .Do(() =>
            {
                var l = Subject.Add<Located>();
                
                l.Location = Transform.position;
            })
        ;
        
        void Remember_last_location() =>
            Gameplay()
            .Includes<LocatedOnStart, InterpolatesLocation>()
            .Do((Located l) =>
            {
                var r = Subject.GetOrAdd<RemembersLastLocation>();
                
                r.LastLocation = l.Location;
            })
        ;

        void Apply_interpolated_location() =>
            Presentation()
            .Includes<LocatedOnStart, InterpolatesLocation>()
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
            Gameplay()
            .Includes<DebugsLocationInterpolation, InterpolatesLocation>()
            .Do((Located l) =>
            {
                var r = Subject.GetOrAdd<RemembersAllLocations>();
                r.Add(l.Location);
            })
        ;

        void Remember_all_transforms() =>
            Presentation()
            .Includes<DebugsLocationInterpolation, InterpolatesLocation>()
            .Do(() =>
            {
                var r = Subject.GetOrAdd<RemembersAllTransformPositions>();
                r.Add(Transform.position);
            })
        ;
        
        #endregion
    }
}