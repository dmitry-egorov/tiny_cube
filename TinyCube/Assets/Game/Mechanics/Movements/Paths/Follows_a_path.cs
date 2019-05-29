using Game.Mechanics.Movements;
using Game.Mechanics.Movements.Paths;
using Plugins.Lanski.Subjective;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Can_follow_a_path))]
public class Follows_a_path: SubjectComponent
{
    public Marks_a_waypoint_level level;
    public float distance;
    public Direction direction;

    public Vector3 paths_location => level.position_at(distance);
    public float path_height => level.height_at(distance);
    public float path_length => level.length;
    public Quaternion rotation => level.rotation_for(direction);
    public Marks_a_waypoint_level[] levels => level.levels;
    public Vector3 travel_direction => direction.multiplier() * level.direction.normalized;

    public void switch_to(Marks_a_waypoint_level l, float new_distance)
    {
        Assert.IsNotNull(l);
        Assert.IsNotNull(l.next);
        
        level = l;
        distance = new_distance;
    }

    public void set_distance_from(Direction d, float ed) => distance = distance_from(d, ed);

    public float distance_from(Direction d, float ed) => level.distance_from(d, ed);

    public float direction_multiplier() => direction.multiplier();

    public (Marks_a_waypoint_level, float) path_at(float d) => level.path_at(d);

    public void reverse_direction() => direction = direction.opposite();
}