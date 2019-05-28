using System;
using ExtraCollections;
using Plugins.Lanski.Reflections;

namespace Plugins.Lanski.Subjective
{
    public class MechanicRegistration
    {

        public MechanicRegistration()
        {
            _included = new TypeMask();
            _excluded = new TypeMask();
        }
        
        public MechanicRegistration When<T>() 
            where T: SubjectComponent
        {
            _included.add<T>();
            return this;
        }

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
        {
            _excluded.add<T>();
            return this;
        }

        public void Do(Action a) => SManager.Register(Reflect.CallingMethodName(), a, _included, _excluded);
        public void Do<T1>(Action<T1> a) 
            where T1: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a, _included, _excluded)
        ;
        
        public void Do<T1, T2>(Action<T1, T2> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a, _included, _excluded)
        ;
        
        public void Do<T1, T2, T3>(Action<T1, T2, T3> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            where T3: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a, _included, _excluded)
        ;
        
        public void Do<T1, T2, T3, T4>(Action<T1, T2, T3, T4> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            where T3: SubjectComponent 
            where T4: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a, _included, _excluded)
        ;
        
        readonly TypeMask _included = new TypeMask();
        readonly TypeMask _excluded = new TypeMask();
    }
}
