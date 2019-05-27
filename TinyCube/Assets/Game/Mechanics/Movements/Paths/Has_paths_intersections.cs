using Plugins.Lanski.Subjective;
using UnityEngine;

[RequireComponent(typeof(Follows_a_path))]
public class Has_paths_intersections : SubjectComponent
{
    public PathIntersection front;
    public PathIntersection back;
}