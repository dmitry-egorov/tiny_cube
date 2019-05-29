using Plugins.Lanski.Subjective;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Can_turn_around))]
public class Turns_around_on_swipe_back : SubjectComponent
{
    [FormerlySerializedAs("orientation_ratio")] public float orientation_magnitude = 0.5f;
}