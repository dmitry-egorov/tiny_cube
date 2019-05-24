using System;
using ExtraCollections;
using Plugins.Lanski.Reflections;

namespace Plugins.Lanski.Subjective
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

        public QRegistrationContext When<T1, T2>() 
            where T1 : QComponent where T2 : QComponent 
            => When<T1>().When<T2>();
        
        public QRegistrationContext When<T1, T2, T3>() 
            where T1 : QComponent where T2 : QComponent where T3: QComponent
            => When<T1>().When<T2>().When<T3>();
        
        public QRegistrationContext When<T>() where T: QComponent
        {
            var included = Included ?? ImmutableList<Type>.Empty;
            return new QRegistrationContext(included.Add(typeof(T)), Excluded, Stage);
        }
        public QRegistrationContext ExceptWhen<T>() where T: QComponent
        {
            var excluded = Excluded ?? ImmutableList<Type>.Empty;
            return new QRegistrationContext(Included, excluded.Add(typeof(T)), Stage);
        }

        public void Do(Action a) => QManager.Register(Stage, Reflect.CallingMethodName(), Included, Excluded, a);
        public void Do<T1>(Action<T1> a) where T1 : QComponent => QManager.Register(Stage, Reflect.CallingMethodName(), Included, Excluded, a);
        public void Do<T1, T2>(Action<T1, T2> a) where T1 : QComponent where T2: QComponent => QManager.Register(Stage, Reflect.CallingMethodName(), Included, Excluded, a);

        
    }
}
