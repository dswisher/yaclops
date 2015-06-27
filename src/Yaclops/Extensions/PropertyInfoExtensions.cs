using System.Reflection;


namespace Yaclops.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static bool IsBool(this PropertyInfo info)
        {
            return info.PropertyType == typeof(bool);
        }
    }
}
