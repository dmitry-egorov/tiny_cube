using Plugins.Lanski.Subjective;
using Plugins.Lanski.UnityExtensions.AnimationCurves;

public class Can_jump: SubjectComponent
{
    public ScaledAnimationCurve jumping_curve = new ScaledAnimationCurve { Magnitude = 5, TimeScale = 1 };
    public ScaledAnimationCurve falling_curve = new ScaledAnimationCurve { Magnitude = 5, TimeScale = 1 };
    public float request_cahing_time;
}