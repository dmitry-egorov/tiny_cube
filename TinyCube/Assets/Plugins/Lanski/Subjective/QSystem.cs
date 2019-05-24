using System;
using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    public abstract class QSystem: ISystem
    {
        public abstract void Register();

        protected static QRegistrationContext OnGameplay() => Q.Gameplay();
        protected static QRegistrationContext OnPresentation() => Q.Presentation();
        protected static Subject Subject => Q.Subject;
        protected static Transform Transform => Subject.transform;
        protected static float DeltaTime => Q.DeltaTime;
        protected static float PresentationTimeRatio => Q.PresentationTimeRatio;

        public static bool Has<TC>() where TC : QComponent => Subject.Has<TC>();
        public static bool Has(Type tc) => Subject.Has(tc);
        public static bool TryGet<TC>(out TC c) where TC : QComponent => Subject.TryGet(out c);
        public static TC Add<TC>() where TC : QComponent => Subject.Add<TC>();
        public static TC GetOrAdd<TC>() where TC : QComponent => Subject.GetOrAdd<TC>();
    }
}