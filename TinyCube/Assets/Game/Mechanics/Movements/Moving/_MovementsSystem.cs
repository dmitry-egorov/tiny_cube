﻿namespace Game.Mechanics.Movements
{
    public partial class _MovementsSystem
    {
        void Apply_movement_velocity() =>
            Gameplay()
            .Do((Located l, Moves m) => 
            {
                l.Location += m.Velocity * DeltaTime;
            })
        ;
    }
}