using System;
using Plugins.Lanski.Subjective;
using UnityEngine;

public class Marks_a_waypoint_level : MarkingComponent
{
    public Marks_a_waypoint_level prev;
    public Marks_a_waypoint_level next;
    
    public Marks_a_waypoint Waypoint => _waypoint ? _waypoint : _waypoint = GetComponentInParent<Marks_a_waypoint>();
    
    [NonSerialized] Marks_a_waypoint _waypoint;

    public Vector3 GetPositionAt(float distance)
    {
        var d = GetDirection();
        var sp = GetPosition();
        return sp + d * distance;
    }

    public Vector3 GetPosition() => Waypoint.GetPosition() + Vector3.up * transform.localPosition.y;
    public float GetHeight() => GetPosition().y;
    public float GetHeightAt(float distance) => GetPositionAt(distance).y;
    public Vector3 GetDirection() => GetOffset().normalized;

    public Vector3 GetOffset() => next.GetPosition() - GetPosition();

    public Quaternion GetRotationFor(Direction od)
    {
        var d = GetDirection();
        var s = od == Direction.Forward ? 1f : -1f;
        return Quaternion.LookRotation(d * s);
    }

    public float GetLength() => GetOffset().magnitude;

}