using Plugins.Lanski.Subjective;
using UnityEngine;

namespace Game.Mechanics.Movements.Turning
{
    public class _TurningMechanics: SubjectiveMechanics
    {
        public static void request_to_turn_around_on_key() =>
        act_request_action_on_key_press_start<Turns_around_on_keys, Requested_to_turn_around>();
        
        public static void request_to_turn_around_on_swipe_back() =>
        when<Can_turn_around>().
        act((Turns_around_on_swipe_back tsb, Follows_a_path fp) =>
        {
            if
            (
                try_get_mouse_drag_complete(out var d) &&
                Vector3.Dot(d.world_offset, fp.travel_direction) < -tsb.orientation_magnitude
            )
            {
                add<Requested_to_turn_around>();
            }
        });
        
        public static void remove_turn_around_request() => 
        act_remove<Requested_to_turn_around>();

        public static void turn_around_when_requested() =>
        when<Requested_to_turn_around>().
        act((Can_turn_around cta, Follows_a_path fp) =>
        {
            fp.reverse_direction();

            if (cta.move_when_turning_around && try_get<Can_move>(out var cm))
            {
                var m = get_or_add<Moves>();
                m.speed = cm.speed;
            }
        });
    }
}