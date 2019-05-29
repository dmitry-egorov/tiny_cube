using Plugins.Lanski.Subjective;
using Plugins.Lanski.UnityExtensions.AnimationCurves;
using UnityEngine;

[RequireComponent(typeof(Can_jump))]
public class Jumps_on_key_press : SubjectComponent, IKeySpecifier
{
    public KeyCode[] keys;
    public KeyCode[] get_keys() => keys;
}