using System;
using ExtraCollections;
using Plugins.Lanski.Reflections;

namespace Plugins.Lanski.Subjective
{
    public struct MechanicRegistration
    {
        public readonly ImmutableList<Type> Included;
        public readonly ImmutableList<Type> Excluded;
        public readonly MechanicStage Stage;

        public MechanicRegistration(MechanicStage stage): this(ImmutableList<Type>.Empty, ImmutableList<Type>.Empty, stage) {}
        public MechanicRegistration(ImmutableList<Type> included, ImmutableList<Type> excluded, MechanicStage stage)
        {
            Included = included;
            Excluded = excluded;
            Stage = stage;
        }

        public MechanicRegistration When<T1, T2>() 
            where T1 : SubjectComponent where T2 : SubjectComponent 
            => When<T1>().When<T2>();
        
        public MechanicRegistration When<T1, T2, T3>() 
            where T1 : SubjectComponent where T2 : SubjectComponent where T3: SubjectComponent
            => When<T1>().When<T2>().When<T3>();
        
        public MechanicRegistration When<T>() where T: SubjectComponent
        {
            var included = Included ?? ImmutableList<Type>.Empty;
            return new MechanicRegistration(included.Add(typeof(T)), Excluded, Stage);
        }
        public MechanicRegistration ExceptWhen<T>() where T: SubjectComponent
        {
            var excluded = Excluded ?? ImmutableList<Type>.Empty;
            return new MechanicRegistration(Included, excluded.Add(typeof(T)), Stage);
        }

        public void Do(Action a) => SManager.Register(Stage, Reflect.CallingMethodName(), Included, Excluded, a);
        public void Do<T1>(Action<T1> a) 
            where T1: SubjectComponent 
            => SManager.Register(Stage, Reflect.CallingMethodName(), Included, Excluded, a);
        public void Do<T1, T2>(Action<T1, T2> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            => SManager.Register(Stage, Reflect.CallingMethodName(), Included, Excluded, a);
        public void Do<T1, T2, T3>(Action<T1, T2, T3> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            where T3: SubjectComponent 
            => SManager.Register(Stage, Reflect.CallingMethodName(), Included, Excluded, a);

        
    }
}
