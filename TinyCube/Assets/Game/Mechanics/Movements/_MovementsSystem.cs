using JetBrains.Annotations;
using Plugins.Systematiq;

namespace Game.Mechanics.Movements
{
    [UsedImplicitly]
    public partial class _MovementsSystem: ISystem
    {
        public void Register()
        {
            AddLocationWhenCanBeLocatedAndNotLocated();
            AddMovementWhenCanMoveAndNotMoving();
            
            RememberLastLocationWhenInterpolatingTransform();
            ApplyVelocityWhenMoving();
            
            InterpolateLocation();
        }
    }
}