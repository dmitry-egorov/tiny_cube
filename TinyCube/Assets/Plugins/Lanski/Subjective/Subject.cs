using System;
using Plugins.Lanski.ExtraCollections;
using Plugins.UnityExtensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Plugins.Lanski.Subjective
{
    public class Subject: MonoBehaviour
    {
        public bool has<TC>() where TC : SubjectComponent => this.TryGetComponent<TC>(out var c) && c.isActiveAndEnabled;
        public bool has(Type tc) => this.TryGetComponent(tc, out var c) && ((Behaviour)c).isActiveAndEnabled;

        public bool try_get<TC>(out TC c) where TC : SubjectComponent => this.TryGetComponent(out c) && c.isActiveAndEnabled;
        public TC get<TC>() where TC : SubjectComponent => GetComponent<TC>();
        
        public TC require<TC>() where TC : SubjectComponent
        {
            var c = GetComponent<TC>();
            Assert.IsNotNull(c);
            return c;
        }
    
        public TC add<TC>() where TC: SubjectComponent
        {
            if (!this.TryGetComponent<TC>(out var c))
            {
                components_mask.add<TC>();
                return gameObject.AddComponent<TC>();
            }
        
            if (c.isActiveAndEnabled)
                throw new InvalidOperationException("Component already exists");
            
            components_mask.add<TC>();
            c.enabled = true;
            return c;
        }

        public TC get_or_add<TC>() where TC : SubjectComponent
        {
            if (!this.TryGetComponent<TC>(out var c))
            {
                components_mask.add<TC>();
                return gameObject.AddComponent<TC>();
            }

            if (!c.isActiveAndEnabled)
            {
                components_mask.add<TC>();
                c.enabled = true;
            }

            return c;
        }
        
        public void remove<TC>() where TC: SubjectComponent
        {
            if (!this.TryGetComponent<TC>(out var c))
                return;

            components_mask.remove<TC>();
            c.enabled = false;
        }


        internal bool contains_all(TypeMask included) => components_mask.contains_all(included);
        internal bool contains_any(TypeMask excluded) => components_mask.contains_any(excluded);

        void OnEnable() => _token = SubjectiveManager.Register(this);
        void OnDisable() => SubjectiveManager.Unregister(_token);

        TypeMask components_mask => _components_mask ?? (_components_mask = gather_component_mask());

        TypeMask gather_component_mask()
        {
            var cm = new TypeMask();
            var cs = GetComponents<SubjectComponent>();
            foreach (var c in cs)
            {
                cm.add(c.GetType());
            }

            return cm;
        }

        ShuffleList<Subject>.Token _token;
        TypeMask _components_mask;

    }
}
