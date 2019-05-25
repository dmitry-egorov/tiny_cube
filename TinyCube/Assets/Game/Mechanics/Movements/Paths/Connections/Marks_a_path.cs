using Plugins.Lanski.Subjective;
using UnityEngine;
using UnityEngine.Serialization;

public class Marks_a_path: SubjectComponent
{
    public Transform start;
    public Transform end;
    public float width = 1.0f;
    public float height = 0.5f;

    public Vector3 GetPositionAt(float distance)
    {
        var d = GetDirection();
        var sp = start.position;
        return sp + d * distance;
    }
    
    public float GetHeightAt(float distance) => GetPositionAt(distance).y;

    public Vector3 GetDirection() => GetOffset().normalized;

    public Vector3 GetOffset()
    {
        var sp = start.position;
        var ep = end.position;
        return ep - sp;
    }

    public Quaternion GetRotation(Direction od)
    {
        var d = GetDirection();
        var s = od == Direction.Forward ? 1f : -1f;
        return Quaternion.LookRotation(d * s);
    }

    public float GetDistance() => GetOffset().magnitude;
}