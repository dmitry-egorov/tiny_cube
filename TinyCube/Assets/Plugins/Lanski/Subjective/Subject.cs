using System;
using Plugins.UnityExtensions;
using UnityEngine;

namespace Plugins.Lanski.Subjective
{
    public class Subject: MonoBehaviour
    {
        public bool Has<TC>() where TC : SubjectComponent => this.TryGetComponent<TC>(out var c) && c.isActiveAndEnabled;
        public bool Has(Type tc) => this.TryGetComponent(tc, out var c) && ((Behaviour)c).isActiveAndEnabled;

        public bool TryGet<TC>(out TC c) where TC : SubjectComponent => this.TryGetComponent(out c) && c.isActiveAndEnabled;
    
        public TC Add<TC>() where TC: SubjectComponent
        {
            if (!this.TryGetComponent<TC>(out var c)) 
                return gameObject.AddComponent<TC>();
        
            if (c.isActiveAndEnabled) 
                throw new InvalidOperationException("Component already exists");
            
            c.enabled = true;
            return c;
        }

        public TC GetOrAdd<TC>() where TC : SubjectComponent
        {
            if (!this.TryGetComponent<TC>(out var c)) 
                return gameObject.AddComponent<TC>();

            if (!c.isActiveAndEnabled) c.enabled = true;

            return c;
        }
        
        public void Remove(Type t)
        {
            if (!this.TryGetComponent(t, out var c))
                return;
            
            ((Behaviour)c).enabled = false;
        }
        
        public void Remove<TC>() where TC: SubjectComponent
        {
            if (!this.TryGetComponent<TC>(out var c))
                return;
            
            c.enabled = false;
        }

        public void OnEnable() => _index = SManager.Register(this);
        public void OnDisable() => SManager.Unregister(_index);

        [NonSerialized] int _index;

        
    }
}
