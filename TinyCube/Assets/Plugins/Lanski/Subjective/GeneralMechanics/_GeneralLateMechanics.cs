using Plugins.Lanski.Subjective;

[Late]
public class _GeneralLateMechanics : SubjectiveSystem
{
    public override void Register()
    {
        gameplay();
        {
            except_when<Is_started>().
                act(() => add<Is_started>());            
        }
    }
}