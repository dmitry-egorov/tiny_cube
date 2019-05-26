using Plugins.Lanski.Subjective;
using UnityEngine;

public class Marks_a_point : MarkingComponent
{
    public Marks_a_path path;
    
    public Vector3 GetPosition() => transform.position;

    public float GetHeight() => GetPosition().y;
}