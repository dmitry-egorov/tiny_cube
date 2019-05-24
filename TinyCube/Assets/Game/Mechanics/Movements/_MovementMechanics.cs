using JetBrains.Annotations;
using Plugins.Lanski.Subjective;

namespace Game.Mechanics.Movements
{
    [UsedImplicitly]
    public partial class _MovementMechanics: SSystem
    {
        public override void Register()
        {
            // Gameplay
            {
                Add_location_on_start();
            
                Debug_Remember_all_locations();
                Remember_last_location();
                Remember_last_rotation();
            
                Apply_movement_velocity_when_not_following_a_path();
                Apply_movement_velocity_when_following_a_path();
                Switch_follow_target_when_reaching_segments_end();
                Stop_when_reaching_segments_end_and_theres_no_next_segment();
                Apply_location_when_following_a_path();
                Apply_rotation_when_following_a_path();
            }

            // Presentation
            {
                Apply_interpolated_location();
                Apply_interpolated_rotation();
                Debug_Remember_all_transforms();
            }
        }
    }
}