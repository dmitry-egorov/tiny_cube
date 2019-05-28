using Plugins.Lanski.Subjective;
using UnityEngine.Serialization;

public class Can_follow_a_path : SubjectComponent
{
    [FormerlySerializedAs("wall_detection_width")] public float width = 1f;
    public bool rotates = true;
    public float fall_detection_margin = 0.05f;
    public bool follow_closest_point_on_start = true;

    public float half_width() => width / 2f;
}
