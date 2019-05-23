using System;
using ExtraCollections;
using Reflections;

namespace Plugins.Systematiq
{
    public struct QRegistrationContext
    {
        public readonly ImmutableList<Type> Included;
        public readonly ImmutableList<Type> Excluded;
        public readonly MechanicStage Stage;

        public QRegistrationContext(MechanicStage stage): this(ImmutableList<Type>.Empty, ImmutableList<Type>.Empty, stage) {}
        public QRegistrationContext(ImmutableList<Type> included, ImmutableList<Type> excluded, MechanicStage stage)
        {
            Included = included;
            Excluded = excluded;
            Stage = stage;
        }

        public QRegistrationContext Includes<T>() where T: QComponent
        {
            var included = Included ?? ImmutableList<Type>.Empty;
            return new QRegistrationContext(included.Add(typeof(T)), Excluded, Stage);
        }
        public QRegistrationContext Excludes<T>() where T: QComponent
        {
            var excluded = Excluded ?? ImmutableList<Type>.Empty;
            return new QRegistrationContext(Included, excluded.Add(typeof(T)), Stage);
        }

        public void Do(Action a) => QManager.Register(Stage, Reflect.CallingMethodName(), Included, Excluded, a);
        public void Do<T1>(Action<T1> a) where T1 : QComponent => QManager.Register(Stage, Reflect.CallingMethodName(), Included, Excluded, a);
        public void Do<T1, T2>(Action<T1, T2> a) where T1 : QComponent where T2: QComponent => QManager.Register(Stage, Reflect.CallingMethodName(), Included, Excluded, a);

        
    }
}
