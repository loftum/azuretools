using System;
using System.Collections.Generic;
using System.Linq;

namespace Kudu.Extensions
{
    public static class TypeExtensions
    {
        private static readonly IDictionary<Type, string> ValueNames = new Dictionary<Type, string>
        {
            {typeof(int), "int"},
            {typeof(short), "short"},
            {typeof(byte), "byte"},
            {typeof(bool), "bool"},
            {typeof(long), "long"},
            {typeof(float), "float"},
            {typeof(double), "double"},
            {typeof(decimal), "decimal"},
            {typeof(string), "string"}
        };

        public static string GetFriendlyName(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (ValueNames.ContainsKey(type))
            {
                return ValueNames[type];
            }
            if (type.IsGenericType)
            {
                return $"{type.Name.Split('`')[0]}<{string.Join(", ", type.GetGenericArguments().Select(GetFriendlyName))}>";
            }
            return type.Name;
        }
    }
}