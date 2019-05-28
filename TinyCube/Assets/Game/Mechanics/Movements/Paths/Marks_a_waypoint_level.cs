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

    public Vector3 position => Waypoint.position + Vector3.up * transform.localPosition.y;
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

    public float distance_from(Direction d, float ed) => d == Direction.Front ? length - ed : ed;
    
    public bool is_a_path() => next != null;

    public (Marks_a_waypoint_level /* new level */, float /* new distance */) path_at(float /* distance */ d)
    {
        if (d < 0)
        {
            var pl = prev;
            if (pl != null)
            {
                return (pl, pl.length - -d);
            }
        }
        else if (d > length)
        {
            var nl = next;
            if (nl != null && nl.is_a_path())
            {
                return (nl, d - length);
            }
        }

        return (this, d);
    }

}