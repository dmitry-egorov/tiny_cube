using JetBrains.Annotations;
using Plugins.Lanski.Subjective;
using static Game.Mechanics.Movements.Airborne._AirborneMechanics;
using static Game.Mechanics.Movements.Jumping._JumpingMechanics;
using static Game.Mechanics.Movements.Locating._LocatingMechanics;
using static Game.Mechanics.Movements.Paths._PathsMechanics;
using static Game.Mechanics.Movements.Moving._MovingMechanics;
using static Game.Mechanics.Movements.Rotating._RotatingMechanics;
using static Game.Mechanics.Movements.Turning._TurningMechanics;

namespace Game.Mechanics.Movements
{
    [UsedImplicitly]
    public class MovementSystem: SubjectiveSystem
    {
        public override void Register()
        {
            gameplay();
            {
                // Start
                check_paths_prev_next_pointers_on_start();
                check_paths_levels_order_on_start();
                start_moving_on_start();
                follow_closest_point_on_start();
                
                //Regular
                add_height_when_following_a_path();
                
                DEBUG_remember_all_locations();
                remember_last_location();
                remember_last_rotation();
                
                advance_airborne_elapsed_time();
                advance_cached_jumping_request_elapsed_time();
                
                request_to_turn_around_on_key();
                request_to_turn_around_on_swipe_back();
                turn_around_when_requested();

                request_jumping_on_key_press_start();
                request_to_continue_jumping_on_key_hold();
                cache_jumping_request_when_airborne();
                
                apply_paths_movement_velocity();
                apply_airborne_height();
                land_when_height_is_below_path();
                remove_airborne_when_landing();

                start_jumping_on_request();
                start_jumping_on_cached_request();
                start_falling_when_finished_jumping();
                start_moving_when_jumping();
                
                stop_when_reaching_a_wall();
                find_adjacent_intersections();
                switch_to_a_higher_intersected_path();
                apply_paths_height_when_not_airborne();
                
                apply_paths_location();
                apply_paths_rotation();

                remove_turn_around_request();
                remove_start_jumping_request();
                remove_continue_jumping_request();
                remove_landing();
            }

            presentation();
            {
                apply_interpolated_location();
                apply_interpolated_rotation();
                DEBUG_remember_all_transforms();
            }
        }
    }
}