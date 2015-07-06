using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Yaclops.Extensions
{
    internal static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> FindProperties<T>(this Type type)
        {
            return type.GetProperties()
                .Where(x => x.GetCustomAttributes(typeof (T), true).Any());
        }



        public static T FindAttribute<T>(this Type type)
        {
            var atts = type.GetCustomAttributes(typeof(T), true);

            if (atts.Any())
            {
                return (T) atts.First();
            }

            return default(T);
        }
    }
}
