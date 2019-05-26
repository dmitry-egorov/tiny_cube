namespace Game.Mechanics.Movements
{
    public partial class _Movement
    {
        void Advance_airborne_elapsed_time() =>
            Do((Is_airborne ab) =>
            {
                ab.elapsed_time += DeltaTime;
            })
        ;
        
        void Apply_airborne_height() =>
            Do((Has_location l, Is_airborne ab) =>
            {
                var /* airborne height */ ah = ab.GetHeight(DeltaTime);
                l.SetHeight(ah);
            })
        ;

        void Land_when_height_is_below_path() =>
            ExceptWhen<Jumps>()
            .When<Is_airborne>()
            .Do((Follows_a_path f, Has_location l) =>
            {
                var /* path's height */    ph = f.GetHeight();
                var /* subject's height */ sh = l.GetHeight();

                if (ph >= sh)
                {
                    Remove<Is_airborne>();
                }
            });
    }
}