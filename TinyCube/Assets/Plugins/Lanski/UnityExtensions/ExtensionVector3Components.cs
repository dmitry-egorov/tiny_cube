using UnityEngine;

namespace Plugins.UnityExtensions
{
    public static class ExtensionVector3Components
    {
        public static Vector2 xz(this Vector3 v) => new Vector2(v.x, v.z);
        public static Vector2 xy(this Vector3 v) => new Vector2(v.x, v.y);
    }
}