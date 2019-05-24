using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    [RequireComponent(typeof(Subject))]
    public abstract class SubjectComponent: MonoBehaviour
    {
        void Start() => Subject = GetComponent<Subject>();

        protected void RemoveSelf() => Subject.Remove(GetType()); 

        protected Subject Subject;
    }
}
