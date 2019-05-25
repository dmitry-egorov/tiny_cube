using Plugins.Lanski.Subjective;
using Plugins.Lanski.UnityExtensions.AnimationCurves;
using UnityEngine.Serialization;

public class Is_airborne : SubjectComponent
{
    public float starting_height;
    public float elapsed_time;
    public ScaledAnimationCurve height_curve;

    public float GetHeight(float deltaTime)
    {
        var sh = starting_height;
        var et = elapsed_time;
        var hc = height_curve;
        var dt = deltaTime;

        var oh /* offset height */ = hc.EvaluateSmoothOutside(et, dt);

        return sh + oh;
    }
}