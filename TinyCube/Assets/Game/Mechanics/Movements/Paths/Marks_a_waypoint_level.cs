using System;
using Plugins.Lanski.Subjective;
using UnityEngine;

public class Marks_a_waypoint_level : MarkingComponent
{
    public Marks_a_waypoint_level prev;
    public Marks_a_waypoint_level next;
    
    public Marks_a_waypoint Waypoint => _waypoint ? _waypoint : _waypoint = GetComponentInParent<Marks_a_waypoint>();
    public Marks_a_waypoint_level[] levels => Waypoint.Levels;

    [NonSerialized] Marks_a_waypoint _waypoint;

    public Vector3 position_at(float distance)
    {
        var d = direction;
        var sp = position;
        return sp + d * distance;
    }

    public Vector3 position => Waypoint.GetPosition() + Vector3.up * transform.localPosition.y;
    public float height => position.y;
    public float height_at(float distance) => position_at(distance).y;
    public Vector3 direction => offset.normalized;
    public Vector3 offset => next.position - position;

    public Quaternion rotation_for(Direction od)
    {
        var d = direction;
        var s = od == Direction.Front ? 1f : -1f;
        return Quaternion.LookRotation(d * s);
    }

    public float length => offset.magnitude;
}