using UnityEngine;

namespace Plugins.Lanski.UnityExtensions.AnimationCurves
{
    public static class ExtensionAnimationCurveGetLastTime
    {
        public static float GetLastTime(this AnimationCurve c) => c.length == 0 ? 0f : c[c.length - 1].time;

    }
}