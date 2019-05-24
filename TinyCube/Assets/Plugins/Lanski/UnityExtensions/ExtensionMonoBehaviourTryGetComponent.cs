using System;
using UnityEngine;

namespace Plugins.UnityExtensions
{
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