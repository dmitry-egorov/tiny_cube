using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _MovementMechanics
    {
        void Apply_movement_velocity_when_not_following_a_path() =>
            OnGameplay()
            .ExceptWhen<Follows>()
            .Do((Located l, Moves m) => 
            {
                l.Location += Transform.rotation * Vector3.forward * (m.Speed * DeltaTime);
            })
        ;
        
        void Apply_movement_velocity_when_following_a_path() =>
            OnGameplay()
            .Do((Follows f, Moves m) => 
            {
                f.DistanceWalked += GetVelocity(f, m) * DeltaTime;
            })
        ;
    }
}