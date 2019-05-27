using System;
using Game.Mechanics.Movements;
using Plugins.Lanski.Subjective;
using UnityEngine;
using UnityEngine.Assertions;
using static Direction;

[RequireComponent(typeof(Can_follow_a_path))]
public class Follows_a_path: SubjectComponent
{
    public Marks_a_waypoint_level level;
    public float distance;
    public Direction direction;

    public Vector3 position => level.position_at(distance);
    public float path_height => level.height_at(distance);
    public float path_length => level.length;
    public Quaternion rotation => level.rotation_for(direction);
    public Marks_a_waypoint_level[] levels => level.levels;

    public void switch_to(Marks_a_waypoint_level l, float new_distance)
    {
        Assert.IsNotNull(l);
        Assert.IsNotNull(l.next);
        level = l;
        distance = new_distance;
    }

    public void set_reverse_distance(float ed)
    {
        distance = level.length - ed;
    }

    public void set_distance(float ed)
    {
        distance = ed;
    }

    public void set_distance_from(Direction d, float ed)
    {
        switch (d)
        {
            case Front:
                set_reverse_distance(ed);
                break;
            case Back:
                set_distance(ed);
                break;
            default:
                throw new ArgumentException("Unknown value", nameof(d));
        }
    }

    public float direction_multiplier() => direction.multiplier();
}