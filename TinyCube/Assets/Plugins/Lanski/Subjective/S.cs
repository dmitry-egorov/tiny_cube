using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    public static class S
    {
        public static MechanicRegistration Presentation() => new MechanicRegistration(MechanicStage.Presentation);
        public static MechanicRegistration Gameplay() => new MechanicRegistration(MechanicStage.Gameplay);
        public static Subject Subject => SManager.Subject;
        public static float DeltaTime => SManager.DeltaTime;
        public static float PresentationTimeRatio => SManager.PresentationTimeRatio;
        public static bool GetKey(KeyCode key) => SManager.GetKey(key);
        public static bool GetKeyDown(KeyCode key) => SManager.GetKeyDown(key);

    }
}