using Plugins.Lanski.Subjective;
using UnityEngine;

namespace Game.Mechanics.Movements.Jumping
{
    public class _JumpingMechanics: SubjectiveMechanics
    {
        public static void start_jumping_on_click() =>
        except_when<Is_airborne>()
        .act((Jumps_on_click jc, Has_height hh) =>
        {
            if (!key_press_started(KeyCode.Mouse0))
                return;
            
            var ab = add<Is_airborne>();
            ab.height_curve = jc.jumping_curve;
            ab.elapsed_time = delta_time;
            ab.starting_height = hh.height;
            
            var j = add<Jumps>();
            j.falling_curve = jc.falling_curve;
        });

        public static void start_falling_when_finished_jumping() =>
        act((Jumps j, Has_height hh) =>
        {
            var ab = require<Is_airborne>();
            var et = ab.elapsed_time;
            var lt = ab.height_curve.last_time;
            var ot /* overtime */ = et - lt;
            
            var iot /* is overtime */ = ot > 0;
            var knh /* key not held */ = !key_is_pressed(KeyCode.Mouse0);
            if (iot || knh)//NOTE: can split here
            {
                ab.height_curve = j.falling_curve;
                ab.elapsed_time = iot ? ot : 0;
                ab.starting_height = hh.height;

                remove<Jumps>();
            }
        });

        public static void start_moving_when_jumping() =>
        when<Jumps>()
        .except_when<Moves>()
        .act((Can_move cm) =>
        {
            var m = add<Moves>();
            m.speed = cm.speed;
        });
    }
}