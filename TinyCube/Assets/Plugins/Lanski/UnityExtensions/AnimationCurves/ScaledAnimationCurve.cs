using System;
using UnityEngine;

namespace Plugins.Lanski.UnityExtensions.AnimationCurves
{
    [Serializable]
    public struct ScaledAnimationCurve
    {
        public AnimationCurve Curve;
        public float Magnitude;
        public float TimeScale;

        public float Evaluate(float time)
        {
            var ts = TimeScale;
            var ms = Magnitude;

            var at = time / ts; // adjusted time

            return ms * Curve.Evaluate(at);
        }

        public float GetLastTime() => Curve.GetLastTime() * TimeScale;

        public float EvaluateSmoothOutside(float et, float dt)
        {
            var lt = GetLastTime();
            var ot = et - lt; // overtime

            float rv; // resulting value
            if (ot <= 0) // no overtime
            {
                rv = Evaluate(et);
            }
            else // overtime
            {
                var lv = Evaluate(lt); // last value
                var pv = Evaluate(lt - dt); // previous value

                var dv = lv - pv; // delta value
                rv = lv + dv * ot / dt;
            }

            return rv;
        }
    }
}