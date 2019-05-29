using System;

namespace Plugins.Lanski.ArrayExtensions
{
    public static class ExtensionArrayAny
    {
        public static bool any<T>(this T[] a, Func<T, bool> predicate)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (predicate(a[i]))
                    return true;
            }

            return false;
        }
    }
}