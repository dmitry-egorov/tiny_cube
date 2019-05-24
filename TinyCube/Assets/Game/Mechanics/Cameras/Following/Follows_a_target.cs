using Plugins.Lanski.Subjective;
using UnityEngine;

public class Follows_a_target: SubjectComponent
{
    public bool X;
    public bool Y;
    public bool Z;
    [Range(0, 1)] public float Strength;
    public Transform Target;
}
