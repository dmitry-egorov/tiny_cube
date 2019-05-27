using Plugins.Lanski.Subjective;
using UnityEngine.Serialization;

public class Moves : SubjectComponent
{
    public float speed;
    
    public void Stop() => RemoveSelf();
}