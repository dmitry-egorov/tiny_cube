using Plugins.Lanski.Subjective;
using UnityEngine;

public class Connects: MarkingComponent
{
    public Can_be_connected Start;
    public Can_be_connected End;

    public Vector3 GetPositionAt(float distance)
    {
        var d = GetDirection();
        var sp = Start.transform.position;
        return sp + d * distance;
    }

    public Vector3 GetDirection()
    {
        var o = GetOffset();
        return o.normalized;
    }

    public Vector3 GetOffset()
    {
        var sp = Start.transform.position;
        var ep = End.transform.position;
        return ep - sp;
    }

    public Quaternion GetRotation(Direction od)
    {
        var d = GetDirection();
        var s = od == Direction.Forward ? 1f : -1f;
        return Quaternion.LookRotation(d * s);
    }

    public float GetDistance()
    {
        return GetOffset().magnitude;
    }
}
