namespace Game.Mechanics.Movements
{
    public partial class _MovementsSystem
    {
        private static void ApplyVelocityWhenMoving() =>
            Q
            .OnPresentationUpdate()
            .Includes<CanMove>()
            .Do((Moves m) => 
            {
                Q.Subject.transform.position += m.Velocity * Q.DeltaTime;
            })
        ;
    }
}