﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Yaclops.Attributes;


namespace Yaclops.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static bool IsBool(this PropertyInfo info)
        {
            return (info.PropertyType == typeof(bool)) || (info.PropertyType == typeof(bool?));
        }


        public static bool IsList(this PropertyInfo info)
        {
            return info.PropertyType.IsGenericType && (info.PropertyType.GetGenericTypeDefinition() == typeof(List<>));
        }


        public static bool IsHashSet(this PropertyInfo info)
        {
            return info.PropertyType.IsGenericType && (info.PropertyType.GetGenericTypeDefinition() == typeof(HashSet<>));
        }


        public static bool IsMandatory(this PropertyInfo info)
        {
            return FindAttribute<MandatoryAttribute>(info).Any();
        }


        public static IList<T> FindAttribute<T>(this PropertyInfo prop)
        {
            var atts = prop.GetCustomAttributes(typeof(T), true);

            return atts.Cast<T>().ToList();
        }
    }
}
