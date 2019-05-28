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
        
        public MechanicRegistration when<T>() 
            where T: SubjectComponent
        {
            _included.add<T>();
            return this;
        }

        public MechanicRegistration when<T1, T2>() 
            where T1 : SubjectComponent where T2 : SubjectComponent 
            => when<T1>().when<T2>()
        ;
        
        public MechanicRegistration when<T1, T2, T3>() 
            where T1 : SubjectComponent where T2 : SubjectComponent where T3: SubjectComponent
            => when<T1>().when<T2>().when<T3>()
        ;

        public MechanicRegistration except_when<T>() 
            where T: SubjectComponent
        {
            _excluded.add<T>();
            return this;
        }

        public void act(Action a) => SubjectiveManager.Register(Reflect.CallingMethodName(), a, _included, _excluded);
        public void act<T1>(Action<T1> a) 
            where T1: SubjectComponent 
            => SubjectiveManager.Register(Reflect.CallingMethodName(), a, _included, _excluded)
        ;
        
        public void act<T1, T2>(Action<T1, T2> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            => SubjectiveManager.Register(Reflect.CallingMethodName(), a, _included, _excluded)
        ;
        
        public void act<T1, T2, T3>(Action<T1, T2, T3> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            where T3: SubjectComponent 
            => SubjectiveManager.Register(Reflect.CallingMethodName(), a, _included, _excluded)
        ;
        
        public void act<T1, T2, T3, T4>(Action<T1, T2, T3, T4> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            where T3: SubjectComponent 
            where T4: SubjectComponent 
            => SubjectiveManager.Register(Reflect.CallingMethodName(), a, _included, _excluded)
        ;
        
        readonly TypeMask _included;
        readonly TypeMask _excluded;
    }
}
