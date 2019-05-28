using System;

[Serializable]
public struct PathIntersection
{
    public Marks_a_waypoint_level level;
    public float distance;
    public float height;
    public bool has_value;
    
    public PathIntersection(Marks_a_waypoint_level level, float distance, float height)
    {
        this.level = level;
        this.distance = distance;
        this.height = height;
        has_value = true;
    }

    public bool is_empty => !has_value;
}