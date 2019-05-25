using System;
using UnityEngine;

namespace Plugins.UnityExtensions
{
    public static class ExtensionMonoBehaviourGetOrAddComponent
    {
        public static TC GetOrAddComponent<TC>(this GameObject go) where TC : Component
        {
            if (!go.TryGetComponent<TC>(out var r))
                r = go.AddComponent<TC>();
            
            return r;
        }
        
        public static TC GetOrAddComponent<TC>(this Component c) where TC : Component
        {
            if (!c.TryGetComponent<TC>(out var r))
                r = c.gameObject.AddComponent<TC>();
            
            return r;
        }
    }
    
    public static class ExtensionMonoBehaviourTryGetComponent
    {
        public static bool TryGetComponent<TC>(this Component c, out TC co) where TC : Component => c.gameObject.TryGetComponent(out co);
        public static bool TryGetComponent<TC>(this GameObject go, out TC c) where TC : Component
        {
            c = go.GetComponent<TC>();
            return c != null;
        }

        public static bool TryGetComponent(this Component c, Type t, out Component co) => c.gameObject.TryGetComponent(t, out co);
        public static bool TryGetComponent(this GameObject go, Type t, out Component c)
        {
            c = go.GetComponent(t);
            return c != null;
        }
    }
}