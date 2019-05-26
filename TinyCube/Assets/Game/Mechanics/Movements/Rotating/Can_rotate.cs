using Plugins.Lanski.Subjective;
using UnityEngine;
using UnityEngine.Serialization;

public class Can_rotate: SubjectComponent
{
    [Range(0, 1)] public float RotationExpStrength = 0.1f;
    public float MinRotationSpeed = 180f;
}