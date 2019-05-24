using JetBrains.Annotations;
using Plugins.Lanski.Subjective;

namespace Game.Mechanics.Cameras
{
    [UsedImplicitly]
    public partial class _CameraMechanics: SSystem
    {
        public override void Register()
        {
            Follow_target();
        }
    }
}