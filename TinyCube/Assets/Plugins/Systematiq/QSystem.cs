using UnityEngine;

namespace Plugins.Systematiq
{
    public abstract class QSystem: ISystem
    {
        public abstract void Register();

        protected QRegistrationContext Gameplay() => Q.Gameplay();
        protected QRegistrationContext Presentation() => Q.Presentation();
        protected Subject Subject => Q.Subject;
        protected Transform Transform => Subject.transform;
        protected float DeltaTime => Q.DeltaTime;
        protected float PresentationTimeRatio => Q.PresentationTimeRatio;
    }
}