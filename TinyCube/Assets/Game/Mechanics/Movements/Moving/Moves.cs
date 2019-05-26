using Plugins.Lanski.Subjective;

public class Moves : SubjectComponent
{
    public float Speed;
    
    public void Stop() => RemoveSelf();
}