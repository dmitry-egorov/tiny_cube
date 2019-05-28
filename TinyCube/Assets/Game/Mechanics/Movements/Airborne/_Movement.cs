namespace Game.Mechanics.Movements
{
    public partial class _Movement
    {
        void Advance_airborne_elapsed_time() =>
            Do((Is_airborne ab) =>
            {
                ab.elapsed_time += delta_time;
            })
        ;
        
        void Apply_airborne_height() =>
            Do((Has_height l, Is_airborne ab) =>
            {
                l.height = ab.GetHeight(delta_time);
            })
        ;

        void Land_when_height_is_below_path() =>
            ExceptWhen<Jumps>()
            .When<Is_airborne>()
            .Do((Follows_a_path f, Has_height l) =>
            {
                if (l.height <= f.path_height)
                {
                    remove<Is_airborne>();
                }
            })
        ;
    }
}