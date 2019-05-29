using Plugins.Lanski.Subjective;

namespace Game.Mechanics.Movements.Airborne
{
    public class _AirborneMechanics: SubjectiveMechanics
    {
        public static void advance_airborne_elapsed_time() =>
            act((Is_airborne ab) =>
            {
                ab.elapsed_time += delta_time;
            })
        ;

        public static void apply_airborne_height() =>
            act((Has_height l, Is_airborne ab) =>
            {
                l.height = ab.GetHeight(delta_time);
            })
        ;

        public static void land_when_height_is_below_path() =>
        except_when<Jumps>().
        when<Is_airborne>().
        act((Follows_a_path f, Has_height l) =>
        {
            if (l.height <= f.path_height)
            {
                add<Lands>();
            }
        });

        public static void remove_airborne_when_landing() =>
        when<Lands>().
        act(() => remove<Is_airborne>());

        public static void remove_landing() => act_remove<Lands>();

    }
}