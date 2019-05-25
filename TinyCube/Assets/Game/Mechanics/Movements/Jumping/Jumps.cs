using Plugins.Lanski.Subjective;
using Plugins.Lanski.UnityExtensions.AnimationCurves;
using UnityEngine;

[RequireComponent(typeof(Is_airborne))]
public class Jumps : SubjectComponent
{
    public ScaledAnimationCurve falling_curve;
}