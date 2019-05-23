namespace Game.Mechanics.Movements
{
    public partial class _MovementsSystem
    {
        private static void ApplyVelocityWhenMoving() =>
            Q
            .OnGameplayUpdate()
            .Includes<CanMove>()
            .Includes<CanBeLocated>()
            .Do((Located l, Moves m) => 
            {
                l.Location += m.Velocity * Q.DeltaTime;
            })
        ;
        
        private static void AddMovementWhenCanMoveAndNotMoving() =>
            Q
            .OnGameplayUpdate()
            .Excludes<Moves>()
            .Do((CanMove cm) => 
            {
                var s = Q.Subject;
                var m = s.Add<Moves>();
                m.Velocity = cm.InitialVelocity;
            })
        ;
    }
}