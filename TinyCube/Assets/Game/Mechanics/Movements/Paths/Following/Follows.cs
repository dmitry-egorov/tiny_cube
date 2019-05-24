using Plugins.Lanski.Subjective;
using UnityEngine;
using static Direction;

public class Follows: SubjectComponent
{
    [Range(0, 1)] public float RotationStrength = 1f;
    public float MinRotationSpeed = 15f;
    
    public Can_be_followed Path;
    public float DistanceWalked;
    public Direction Direction;
    
    public void Reverse()
    {
        Direction = Direction == Forward ? Backward : Forward;
    }
}