using Plugins.Lanski.Subjective;
using UnityEngine;
using UnityEngine.Serialization;

public class Has_rotation: SubjectComponent
{
    [FormerlySerializedAs("Rotation")] public Quaternion rotation;
}
