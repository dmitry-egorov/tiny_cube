using Plugins.Lanski.Subjective;

namespace Game.Mechanics.Movements.Moving
{
    public class _MovingMechanics: SubjectiveMechanics
    {
        public static void start_moving_on_start() =>
        except_when<Is_started>()
        .act((Can_move cm) =>
        {
            if (cm.moves_on_start)
            {
                var m = add<Moves>();
                m.speed = cm.speed;
            }
        });
    }
}