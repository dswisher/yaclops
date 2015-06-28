using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Yaclops.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static bool IsBool(this PropertyInfo info)
        {
            return info.PropertyType == typeof(bool);
        }



        public static IList<T> FindAttribute<T>(this PropertyInfo prop)
        {
            var atts = prop.GetCustomAttributes(typeof(T), true);

            return atts.Cast<T>().ToList();
        }
    }
}
