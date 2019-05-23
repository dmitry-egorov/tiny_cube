using System;
using ExtraCollections;

public struct QRegistrationContext
{
    public readonly ImmutableList<Type> Included;

    public QRegistrationContext(ImmutableList<Type> included)
    {
        Included = included;
    }

    public QRegistrationContext Includes<T>() where T: QComponent
    {
        var included = Included ?? new ImmutableList<Type>();
        return new QRegistrationContext(included.Add(typeof(T)));
    }

    public void Do<T1>(Action<T1> a) where T1 : QComponent => QManager.Register(Included, a);
    public void Do<T1, T2>(Action<T1, T2> a) where T1 : QComponent where T2: QComponent => QManager.Register(Included, a);
}
