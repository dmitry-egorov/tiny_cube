using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _MovementsSystem
    {
        private static void AddLocationWhenCanBeLocatedAndNotLocated() =>
            Q
            .OnGameplayUpdate()
            .Includes<CanBeLocated>()
            .Excludes<Located>()
            .Do(() =>
            {
                var s = Q.Subject;
                var l = s.Add<Located>();
                l.Location = s.transform.position;
            })
        ;
        
        private static void RememberLastLocationWhenInterpolatingTransform() =>
            Q
            .OnGameplayUpdate()
            .Includes<CanBeLocated>()
            .Includes<CanInterpolateTransform>()
            .Do((Located l) =>
            {
                var r = Q.Subject.GetOrAdd<RemembersLastLocation>();
                r.LastLocation = l.Location;
            })
        ;

        private static void RememberAllLocationsWhenInterpolatingTransform() =>
            Q
            .OnGameplayUpdate()
            .Includes<CanBeLocated>()
            .Includes<CanInterpolateTransform>()
            .Do((Located l) =>
            {
                var r = Q.Subject.GetOrAdd<RemembersAllLocations>();
                r.Add(l.Location);
            })
        ;

        private static void RememberAllTransformsWhenInterpolatingTransform() =>
            Q
            .OnGameplayUpdate()
            .Includes<CanBeLocated>()
            .Includes<CanInterpolateTransform>()
            .Do(() =>
            {
                var r = Q.Subject.GetOrAdd<RemembersAllTransformPositions>();
                r.Add(Q.Subject.transform.position);
            })
        ;

        private static void InterpolateLocation() =>
            Q
            .OnPresentationUpdate()
            .Includes<CanBeLocated>()
            .Includes<CanInterpolateTransform>()
            .Do((Located l, RemembersLastLocation r) =>
            {
                Q.Subject.transform.position = Vector3.Lerp(r.LastLocation, l.Location, Q.PresentationTimeRatio);
            });
    }
}