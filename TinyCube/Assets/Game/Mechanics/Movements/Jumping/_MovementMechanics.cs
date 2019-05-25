using Plugins.Lanski.Subjective;
using UnityEngine;

namespace Game.Mechanics.Movements
{
    public partial class _MovementMechanics
    {
        void Start_jumping_on_click() =>
            OnGameplay()
            .ExceptWhen<Is_airborne>()
            .Do((Jumps_on_click jc, Located l) =>
            {
                if (!GetKeyDown(KeyCode.Mouse0))
                    return;
                
                var ab = Add<Is_airborne>();
                ab.height_curve = jc.jumping_curve;
                ab.elapsed_time = DeltaTime;
                ab.starting_height = l.GetHeight();
                
                var j = Add<Jumps>();
                j.falling_curve = jc.falling_curve;
            })
        ;

        void Start_falling_when_finished_jumping() =>
            OnGameplay()
            .Do((Jumps j, Located l) =>
            {
                var ab = Require<Is_airborne>();
                var et = ab.elapsed_time;
                var lt = ab.height_curve.GetLastTime();
                var ot /* overtime */ = et - lt;
                
                var iot /* is overtime */ = ot > 0;
                var knh /* key not held */ = !GetKey(KeyCode.Mouse0);
                if (iot || knh)
                {
                    ab.height_curve = j.falling_curve;
                    ab.elapsed_time = iot ? ot : 0;
                    ab.starting_height = l.GetHeight();

                    Remove<Jumps>();
                }
            })
        ;
    }
}