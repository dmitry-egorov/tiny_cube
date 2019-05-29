using Plugins.Lanski.Subjective;
using UnityEngine;

public class Turns_around_on_keys : SubjectComponent, IKeySpecifier
{
    public KeyCode[] keys;
    public KeyCode[] get_keys() => keys;
}