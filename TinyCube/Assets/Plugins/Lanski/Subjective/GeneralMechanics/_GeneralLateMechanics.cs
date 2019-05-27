using Plugins.Lanski.Subjective;

[Late]
public class _GeneralLateMechanics : SSystem
{
    public override void Register()
    {
        Gameplay();
        {
            ExceptWhen<Is_started>().
                Do(() => add<Is_started>());            
        }
    }
}