using System;
using ExtraCollections;
using Plugins.Lanski.Reflections;

namespace Plugins.Lanski.Subjective
{
    public struct MechanicRegistration
    {
        public ImmutableList<Type> Included => _included ?? ImmutableList<Type>.Empty;
        public ImmutableList<Type> Excluded => _excluded ?? ImmutableList<Type>.Empty;

        public MechanicRegistration(ImmutableList<Type> included, ImmutableList<Type> excluded)
        {
            _included = included;
            _excluded = excluded;
        }
        
        public MechanicRegistration When<T>() 
            where T: SubjectComponent 
            => new MechanicRegistration(Included.Add(typeof(T)), Excluded)
        ;

        public MechanicRegistration When<T1, T2>() 
            where T1 : SubjectComponent where T2 : SubjectComponent 
            => When<T1>().When<T2>()
        ;
        
        public MechanicRegistration When<T1, T2, T3>() 
            where T1 : SubjectComponent where T2 : SubjectComponent where T3: SubjectComponent
            => When<T1>().When<T2>().When<T3>()
        ;

        public MechanicRegistration ExceptWhen<T>() 
            where T: SubjectComponent 
            => new MechanicRegistration(Included, Excluded.Add(typeof(T)))
        ;

        public void Do(Action a) => SManager.Register(Reflect.CallingMethodName(), a, Included, Excluded);
        public void Do<T1>(Action<T1> a) 
            where T1: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a, Included, Excluded)
        ;
        
        public void Do<T1, T2>(Action<T1, T2> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a, Included, Excluded)
        ;
        
        public void Do<T1, T2, T3>(Action<T1, T2, T3> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            where T3: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a, Included, Excluded)
        ;
        
        public void Do<T1, T2, T3, T4>(Action<T1, T2, T3, T4> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            where T3: SubjectComponent 
            where T4: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a, Included, Excluded)
        ;
        
        readonly ImmutableList<Type> _included;
        readonly ImmutableList<Type> _excluded;
    }
}
