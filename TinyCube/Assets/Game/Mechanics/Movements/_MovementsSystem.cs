using JetBrains.Annotations;

namespace Game.Mechanics.Movements
{
    [UsedImplicitly]
    public partial class _MovementsSystem: ISystem
    {
        public void Register()
        {
            ApplyVelocityWhenMoving();
        }
    }
}