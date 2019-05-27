namespace Plugins.Lanski.NullableExtensions
{
    public static class NullableIsEmptyExtension
    {
        public static bool is_empty<T>(this T? n) where T : struct => !n.HasValue;
    }
}