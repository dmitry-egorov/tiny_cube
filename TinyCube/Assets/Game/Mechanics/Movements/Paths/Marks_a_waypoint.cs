using System;
using Plugins.Lanski.Subjective;
using UnityEngine;

public class Marks_a_waypoint: MarkingComponent
{
    public Marks_a_waypoint_level[] Levels => _levels ?? (_levels = GetComponentsInChildren<Marks_a_waypoint_level>());
    
    [NonSerialized] Marks_a_waypoint_level[] _levels;

    public Vector3 position => transform.position;
}