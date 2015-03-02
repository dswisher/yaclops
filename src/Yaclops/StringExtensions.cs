using System.Text;

namespace Yaclops
{
    internal static class StringExtensions
    {
        public static string Decamel(this string camel, char separator = ' ')
        {
            StringBuilder builder = new StringBuilder();

            foreach (char c in camel.Replace("Command", string.Empty))
            {
                if (char.IsUpper(c))
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(separator);
                    }

                    builder.Append(char.ToLower(c));
                }
                else
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }
    }
}
