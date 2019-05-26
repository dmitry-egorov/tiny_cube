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
                Add_location_on_start();
            
                Debug_Remember_all_locations();
                Remember_last_location();
                Remember_last_rotation();
                
                Advance_airborne_elapsed_time();

                Start_jumping_on_click();
                Start_falling_when_finished_jumping();
            
                Apply_paths_movement_velocity();
                Stop_when_reaching_an_end_of_an_unconnected_path();
                //Handle_walking_beyond_the_path();
                Apply_paths_planar_location();
                Apply_paths_rotation();
                
                Apply_airborne_height();
                Land_when_height_is_below_path();
                
                Apply_paths_height_when_not_airborne();
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