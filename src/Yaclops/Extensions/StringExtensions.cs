using System.Collections.Generic;
using System.Linq;
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

                    builder.Append(char.ToLower(c));
                }
                else
                {
                    builder.Append(char.ToLower(c));
                }
            }

            if (builder.Length > 0)
            {
                yield return builder.ToString();
            }
        }



        public static bool IsBool(this string text)
        {
            bool scratch;
            return bool.TryParse(text, out scratch);
        }



        public static string Quote(this string text)
        {
            // TODO - check for other whitespace chars
            if (text.Contains(' '))
            {
                return '"' + text + '"';
            }

            return text;
        }
    }
}
