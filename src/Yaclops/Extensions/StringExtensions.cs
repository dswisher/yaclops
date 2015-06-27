using System.Collections.Generic;
using System.Text;

namespace Yaclops.Extensions
{
    internal static class StringExtensions
    {
        public static IEnumerable<string> Decamel(this string camel)
        {
            StringBuilder builder = new StringBuilder();

            foreach (char c in camel)
            {
                if (char.IsUpper(c))
                {
                    if (builder.Length > 0)
                    {
                        yield return builder.ToString();
                        builder.Clear();
                    }

                    builder.Append(c);
                }
                else
                {
                    builder.Append(c);
                }
            }

            if (builder.Length > 0)
            {
                yield return builder.ToString();
            }
        }
    }
}
