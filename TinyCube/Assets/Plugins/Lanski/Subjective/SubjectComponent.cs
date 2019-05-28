using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    [RequireComponent(typeof(Subject))]
    public abstract class SubjectComponent: MonoBehaviour
    {
        public void Start() {}
        
        protected Subject subject => _subject != null ? _subject : _subject = GetComponent<Subject>();
        
        Subject _subject;
    }
}
