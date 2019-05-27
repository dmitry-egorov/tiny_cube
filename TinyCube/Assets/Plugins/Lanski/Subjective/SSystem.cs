using System;
using System.Linq;
using Plugins.Lanski.Reflections;
using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    public abstract class SSystem: ISystem
    {
        public abstract void Register();
        
        protected static void Presentation() => SManager.SetToPresentationRegistration();
        protected static void Gameplay() => SManager.SetToGameplayRegistration();
        
        public MechanicRegistration When<T>() 
            where T: SubjectComponent 
            => new MechanicRegistration().When<T>();

        public MechanicRegistration When<T1, T2>() 
            where T1 : SubjectComponent where T2 : SubjectComponent 
            => new MechanicRegistration().When<T1, T2>();
        
        public MechanicRegistration When<T1, T2, T3>() 
            where T1 : SubjectComponent where T2 : SubjectComponent where T3: SubjectComponent
            => new MechanicRegistration().When<T1, T2, T3>();
        
        public MechanicRegistration ExceptWhen<T>() 
            where T: SubjectComponent 
            => new MechanicRegistration().ExceptWhen<T>();
        
        public void Do(Action a) 
            => SManager.Register(Reflect.CallingMethodName(), a);

        public void Do<T1>(Action<T1> a) 
            where T1: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a);
        
        public void Do<T1, T2>(Action<T1, T2> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a);

        public void Do<T1, T2, T3>(Action<T1, T2, T3> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            where T3: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a);

        public void Do<T1, T2, T3, T4>(Action<T1, T2, T3, T4> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            where T3: SubjectComponent 
            where T4: SubjectComponent 
            => SManager.Register(Reflect.CallingMethodName(), a);
        
        protected static Subject subject => SManager.Subject;
        protected static Transform transform => subject.transform;
        protected static float delta_time => SManager.DeltaTime;
        protected static float presentation_time_ratio => SManager.PresentationTimeRatio;

        public static bool has<TC>() where TC : SubjectComponent => subject.Has<TC>();
        public static bool has(Type tc) => subject.Has(tc);
        public static bool try_get<TC>(out TC c) where TC : SubjectComponent => subject.TryGet(out c);
        public static TC add<TC>() where TC : SubjectComponent => subject.Add<TC>();
        public static TC get_or_add<TC>() where TC : SubjectComponent => subject.GetOrAdd<TC>();
        protected TC require<TC>() where TC : SubjectComponent => subject.Require<TC>();
        protected void remove<TC>() where TC : SubjectComponent => subject.Remove<TC>();

        protected bool get_input_key(KeyCode i) => SManager.GetKey(i);
        protected bool get_input_key_down(KeyCode i) => SManager.GetKeyDown(i);
    }
}