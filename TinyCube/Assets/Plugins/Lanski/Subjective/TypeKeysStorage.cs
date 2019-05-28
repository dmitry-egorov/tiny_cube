using System;
using System.Collections.Generic;

namespace Plugins.Lanski.Subjective
{
    internal static class TypeKeysStorage<T>
    {
        public static TypeKey Key => _key ?? (_key = TypeKeysStorage.GetKey(typeof(T))).Value;
        static TypeKey? _key;
    }

    internal static class TypeKeysStorage
    {
        public static TypeKey GetKey(Type t) => _map.TryGetValue(t, out var key) ? key : (_map[t] = Next);
        
        static TypeKey Next => _lastKey = _lastKey.next();
        
        static TypeKey _lastKey;
        static readonly Dictionary<Type, TypeKey> _map = new Dictionary<Type, TypeKey>();
    }
}