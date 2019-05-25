namespace Plugins.Lanski.FunctionalExtensions
{
    public static class ExtensionsObjectTryGet
    {
        public static bool TryGet<T>(this T value, out T variable) where T: class
        {
            variable = value;
            return value != null;
        }
    }
}