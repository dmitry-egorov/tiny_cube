using Plugins.Lanski.Subjective;
using UnityEngine;
using UnityEngine.Serialization;

public class Can_rotate: SubjectComponent
{
    [FormerlySerializedAs("RotationExpStrength")] [Range(0, 1)] public float exp_rotation_strength = 0.1f;
    [FormerlySerializedAs("MinRotationSpeed")] public float linear_rotation_speed = 180f;
}