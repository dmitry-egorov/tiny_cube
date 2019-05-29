using Plugins.Lanski.Subjective;

namespace Game.Mechanics.Movements.Jumping
{
    public class _JumpingMechanics: SubjectiveMechanics
    {
        public static void request_jumping_on_key_press_start() =>
        act_request_action_on_key_press_start<Jumps_on_key_press, Requested_to_start_jumping>();
        
        public static void request_to_continue_jumping_on_key_hold() =>
        act_request_action_on_key_hold<Jumps_on_key_press, Requested_to_continue_jumping>();

        public static void remove_start_jumping_request() => 
        act_remove<Requested_to_start_jumping>();

        public static void remove_continue_jumping_request() => 
        act_remove<Requested_to_continue_jumping>();
        
        public static void start_jumping_on_request() =>
        except_when<Is_airborne, Jumps>().
        when<Requested_to_start_jumping>().
        act((Can_jump cj, Has_height hh) =>
        {
            (subject, cj, hh).start_jumping();
        });

        public static void cache_jumping_request_when_airborne() =>
        when<Is_airborne, Requested_to_start_jumping>()
        .act(() =>
        {
            var c = get_or_add<Caches_jump_request>();
            c.elapsed_time = 0;
        });

        public static void advance_cached_jumping_request_elapsed_time() =>
        act((Caches_jump_request c) => c.elapsed_time += delta_time);
        
        public static void start_jumping_on_cached_request() =>
        except_when<Jumps>().
        when<Lands>().
        act((Can_jump cj, Caches_jump_request c, Has_height hh) =>
        {
            if (has<Requested_to_continue_jumping>() && c.elapsed_time < cj.request_cahing_time)
            {
                (subject, cj, hh).start_jumping();
            }
            
            remove<Caches_jump_request>();
        });
        

        public static void start_falling_when_finished_jumping() =>
        act((Jumps j, Has_height hh) =>
        {
            var ab = require<Is_airborne>();
            var et = ab.elapsed_time;
            var lt = ab.height_curve.last_time;
            var ot /* overtime */ = et - lt;
            
            var iot /* is overtime */ = ot > 0;
            var knh /* key not held */ = !has<Requested_to_continue_jumping>();
            if (iot || knh)//NOTE: can split here
            {
                ab.height_curve = j.falling_curve;
                ab.elapsed_time = iot ? ot : 0;
                ab.starting_height = hh.height;

                remove<Jumps>();
            }
        });

        public static void start_moving_when_jumping() =>
        when<Jumps>().
        except_when<Moves>().
        act((Can_move cm) =>
        {
            var m = add<Moves>();
            m.speed = cm.speed;
        });
    }

    public static class Helper
    {
        public static void start_jumping(this (Subject s, Can_jump cj, Has_height hh) x)
        {
            var ab = x.s.add<Is_airborne>();
            ab.height_curve = x.cj.jumping_curve;
            ab.elapsed_time = SubjectiveManager.delta_time;
            ab.starting_height = x.hh.height;

            var j = x.s.add<Jumps>();
            j.falling_curve = x.cj.falling_curve;
        }
    }
}