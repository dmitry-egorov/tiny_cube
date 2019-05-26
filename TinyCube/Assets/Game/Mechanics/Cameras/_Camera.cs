using JetBrains.Annotations;
using Plugins.Lanski.Subjective;

namespace Game.Mechanics.Cameras
{
    [UsedImplicitly]
    public partial class _Camera: SSystem
    {
        public override void Register()
        {
            Presentation();
            {
                Follow_target();
            }
        }
    }
}