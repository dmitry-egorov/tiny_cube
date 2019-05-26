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
        
        protected static Subject Subject => SManager.Subject;
        protected static Transform Transform => Subject.transform;
        protected static float DeltaTime => SManager.DeltaTime;
        protected static float PresentationTimeRatio => SManager.PresentationTimeRatio;

        public static bool Has<TC>() where TC : SubjectComponent => Subject.Has<TC>();
        public static bool Has(Type tc) => Subject.Has(tc);
        public static bool TryGet<TC>(out TC c) where TC : SubjectComponent => Subject.TryGet(out c);
        public static TC Add<TC>() where TC : SubjectComponent => Subject.Add<TC>();
        public static TC GetOrAdd<TC>() where TC : SubjectComponent => Subject.GetOrAdd<TC>();
        protected TC Require<TC>() where TC : SubjectComponent => Subject.Require<TC>();
        protected void Remove<TC>() where TC : SubjectComponent => Subject.Remove<TC>();

        protected bool GetKey(KeyCode i) => SManager.GetKey(i);
        protected bool GetKeyDown(KeyCode i) => SManager.GetKeyDown(i);
    }
}