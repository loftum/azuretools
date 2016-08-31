using System;
using System.Linq;

namespace Kudu.Extensions
{
    public static class StringExtensions
    {
        public static bool In(this string value, StringComparison comparison, params string[] values)
        {
            return values.Any(v => string.Equals(v, value, comparison));
        }
    }
}