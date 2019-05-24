using System.Diagnostics;

namespace Plugins.Lanski.Reflections
{
    public static class Reflect
    {
        public static string CallingMethodName()
        {
            var m = new StackFrame(2).GetMethod();
            var t = m.DeclaringType;
            return $"{t.Name}.{m.Name}";
        }
    }
}