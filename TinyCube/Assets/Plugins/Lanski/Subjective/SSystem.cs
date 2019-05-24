using System;
using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    public abstract class SSystem: ISystem
    {
        public abstract void Register();

        protected static MechanicRegistration OnGameplay() => S.Gameplay();
        protected static MechanicRegistration OnPresentation() => S.Presentation();
        protected static Subject Subject => S.Subject;
        protected static Transform Transform => Subject.transform;
        protected static float DeltaTime => S.DeltaTime;
        protected static float PresentationTimeRatio => S.PresentationTimeRatio;

        public static bool Has<TC>() where TC : SubjectComponent => Subject.Has<TC>();
        public static bool Has(Type tc) => Subject.Has(tc);
        public static bool TryGet<TC>(out TC c) where TC : SubjectComponent => Subject.TryGet(out c);
        public static TC Add<TC>() where TC : SubjectComponent => Subject.Add<TC>();
        public static TC GetOrAdd<TC>() where TC : SubjectComponent => Subject.GetOrAdd<TC>();
    }
}