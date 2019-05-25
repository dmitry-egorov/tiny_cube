namespace Game.Mechanics.Movements
{
    public partial class _MovementMechanics
    {
        void Advance_airborne_elapsed_time() =>
            OnGameplay()
            .Do((Is_airborne ab) =>
            {
                ab.elapsed_time += DeltaTime;
            })
        ;
        
        void Apply_airborne_height() =>
            OnGameplay()
            .Do((Located l, Is_airborne ab) =>
            {
                var ah /* airborne height */ = ab.GetHeight(DeltaTime);
                l.SetHeight(ah);
            })
        ;

        void Land_when_height_is_below_path() =>
            OnGameplay()
            .ExceptWhen<Jumps>()
            .When<Is_airborne>()
            .Do((Follows_a_path f, Located l) =>
            {
                //NOTE: assumes airborne height is already applied
                if (f.GetHeight() >= l.GetHeight())
                {
                    Remove<Is_airborne>();
                }
            });
    }
}