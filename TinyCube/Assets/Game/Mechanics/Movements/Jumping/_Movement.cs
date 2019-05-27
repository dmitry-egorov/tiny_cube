using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _Movement
    {
        void Start_jumping_on_click() =>
            ExceptWhen<Is_airborne>()
            .Do((Jumps_on_click jc, Has_height hh) =>
            {
                if (!get_input_key_down(KeyCode.Mouse0))
                    return;
                
                var ab = add<Is_airborne>();
                ab.height_curve = jc.jumping_curve;
                ab.elapsed_time = delta_time;
                ab.starting_height = hh.height;
                
                var j = add<Jumps>();
                j.falling_curve = jc.falling_curve;
            })
        ;

        void Start_falling_when_finished_jumping() =>
            Do((Jumps j, Has_height hh) =>
            {
                var ab = require<Is_airborne>();
                var et = ab.elapsed_time;
                var lt = ab.height_curve.GetLastTime();
                var ot /* overtime */ = et - lt;
                
                var iot /* is overtime */ = ot > 0;
                var knh /* key not held */ = !get_input_key(KeyCode.Mouse0);
                if (iot || knh)//TODO: split here
                {
                    ab.height_curve = j.falling_curve;
                    ab.elapsed_time = iot ? ot : 0;
                    ab.starting_height = hh.height;

                    remove<Jumps>();
                }
            })
        ;

        void Start_moving_when_jumping() =>
            When<Jumps>()
            .ExceptWhen<Moves>()
            .Do((Can_move cm) =>
            {
                var m = add<Moves>();
                m.speed = cm.speed;
            })
        ;
    }
}