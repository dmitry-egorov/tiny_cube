using JetBrains.Annotations;
using Plugins.Lanski.Subjective;

namespace Game.Mechanics.Movements
{
    [UsedImplicitly]
    public partial class _Movement: SSystem
    {
        public override void Register()
        {
            Gameplay();
            {
                // Start
                Check_paths_prev_next_pointers_on_start();
                Check_paths_levels_order_on_start();
                Start_moving_on_start();
                Follow_closest_point_on_start();
                    
                //Regular
                Add_height_when_following_a_path();
                
                Debug_Remember_all_locations();
                Remember_last_location();
                Remember_last_rotation();
                
                Advance_airborne_elapsed_time();

                Start_jumping_on_click();
                Start_falling_when_finished_jumping();
                Start_moving_when_jumping();
            
                Apply_paths_movement_velocity();
                Stop_when_reaching_a_wall();
                Apply_adjacent_intersections();
                Switch_to_a_higher_intersected_path();
                //Switch_to_the_highest_path_below();
                //Handle_walking_beyond_the_path();
                Apply_airborne_height();
                Land_when_height_is_below_path();
                Become_airborne_when_height_is_above_path();
                
                
                Apply_paths_height_when_not_airborne();
                
                Apply_paths_location();
                Apply_paths_rotation();
            }

            Presentation();
            {
                Apply_interpolated_location();
                Apply_interpolated_rotation();
                Debug_Remember_all_transforms();
            }
        }
    }
}