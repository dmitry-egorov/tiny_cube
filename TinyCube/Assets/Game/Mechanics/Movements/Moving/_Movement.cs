using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _Movement
    {
        void Start_moving_on_start() =>
            ExceptWhen<Is_started>()
            .Do((Can_move cm) =>
            {
                if (cm.moves_on_start)
                {
                    var m = add<Moves>();
                    m.speed = cm.speed;
                }
            });
    }
}