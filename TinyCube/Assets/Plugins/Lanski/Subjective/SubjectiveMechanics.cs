using System;
using Plugins.Lanski.Reflections;
using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    public abstract class SubjectiveMechanics
    {
        protected static MechanicRegistration when<T>() 
            where T: SubjectComponent 
            => new MechanicRegistration().when<T>();

        protected static MechanicRegistration when<T1, T2>() 
            where T1 : SubjectComponent where T2 : SubjectComponent 
            => new MechanicRegistration().when<T1, T2>();
        
        protected static MechanicRegistration when<T1, T2, T3>() 
            where T1 : SubjectComponent where T2 : SubjectComponent where T3: SubjectComponent
            => new MechanicRegistration().when<T1, T2, T3>();

        protected static MechanicRegistration except_when<T>() 
            where T: SubjectComponent 
            => new MechanicRegistration().except_when<T>();

        protected static void act(Action a) 
            => SubjectiveManager.Register(Reflect.CallingMethodName(), a, new TypeMask(), new TypeMask());

        protected static void act<T1>(Action<T1> a) 
            where T1: SubjectComponent 
            => SubjectiveManager.Register(Reflect.CallingMethodName(), a, new TypeMask(), new TypeMask());

        protected static void act<T1, T2>(Action<T1, T2> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            => SubjectiveManager.Register(Reflect.CallingMethodName(), a, new TypeMask(), new TypeMask());

        protected static void act<T1, T2, T3>(Action<T1, T2, T3> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            where T3: SubjectComponent 
            => SubjectiveManager.Register(Reflect.CallingMethodName(), a, new TypeMask(), new TypeMask());

        protected static void act<T1, T2, T3, T4>(Action<T1, T2, T3, T4> a) 
            where T1: SubjectComponent 
            where T2: SubjectComponent 
            where T3: SubjectComponent 
            where T4: SubjectComponent 
            => SubjectiveManager.Register(Reflect.CallingMethodName(), a, new TypeMask(), new TypeMask());
        
        protected static Subject subject => SubjectiveManager.subject;
        protected static Transform transform => subject.transform;
        protected static float delta_time => SubjectiveManager.delta_time;
        protected static float presentation_time_ratio => SubjectiveManager.presentation_time_ratio;

        protected static bool has<TC>() where TC : SubjectComponent => subject.has<TC>();
        protected static bool has(Type tc) => subject.has(tc);
        protected static bool try_get<TC>(out TC c) where TC : SubjectComponent => subject.try_get(out c);
        protected static TC add<TC>() where TC : SubjectComponent => subject.add<TC>();
        protected static TC get_or_add<TC>() where TC : SubjectComponent => subject.get_or_add<TC>();
        protected static TC require<TC>() where TC : SubjectComponent => subject.require<TC>();
        protected static void remove<TC>() where TC : SubjectComponent => subject.remove<TC>();

        protected static bool key_is_pressed(KeyCode i) => SubjectiveInput.GetKey(i);
        protected static bool key_press_started(KeyCode i) => SubjectiveInput.GetKeyDown(i);
    }
}