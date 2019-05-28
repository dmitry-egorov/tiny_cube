using JetBrains.Annotations;
using Plugins.Lanski.Subjective;
using static Game.Mechanics.Cameras.Following.FollowingMechanics;

namespace Game.Mechanics.Cameras
{
    [UsedImplicitly]
    public class CameraSystem: SubjectiveSystem
    {
        public override void Register()
        {
            presentation();
            {
                follow_target();
            }
        }
    }
}