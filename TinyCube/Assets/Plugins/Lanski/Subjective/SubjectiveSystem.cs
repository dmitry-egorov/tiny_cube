using System.Linq;

namespace Plugins.Lanski.Subjective
{
    public abstract class SubjectiveSystem: SubjectiveMechanics, ISystem
    {
        public abstract void Register();
        
        protected static void presentation() => SubjectiveManager.SetToPresentationRegistration();
        protected static void gameplay() => SubjectiveManager.SetToGameplayRegistration();

        
    }
}