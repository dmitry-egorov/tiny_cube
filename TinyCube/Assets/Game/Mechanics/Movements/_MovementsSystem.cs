using JetBrains.Annotations;
using Plugins.Lanski.Subjective;

namespace Game.Mechanics.Movements
{
    [UsedImplicitly]
    public partial class _MovementsSystem: QSystem
    {
        public override void Register()
        {
            Add_location();
            
            Remember_all_locations();
            Remember_last_location();
            Apply_movement_velocity();

            Apply_interpolated_location();
            Remember_all_transforms();
        }
    }
}