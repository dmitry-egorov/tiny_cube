using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflections
{
    public static class ExtensionAppDomainFindAllDerivedTypes
    {
        public static IEnumerable<Type> FindAllDerivedTypes<T>(this AppDomain domain) =>
            domain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(T).IsAssignableFrom(t) && t != typeof(T))
        ;

        public static IEnumerable<T> InstantiateAllDerivedTypes<T>(this AppDomain domain) =>
            domain
            .FindAllDerivedTypes<T>()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Select(t => (T) Activator.CreateInstance(t))
        ;
    }
}