using System;

namespace DefaultNamespace
{
    public static class ExtensionArrayAny
    {
        public static bool QAny<T>(this T[] a, Func<T, bool> predicate)
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