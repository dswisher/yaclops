using System;
using System.Collections.Generic;
using System.Text;

namespace Yaclops.Extensions
{
    internal static class StringExtensions
    {
        public static string Decamel(this string camel, char separator = ' ')
        {
            StringBuilder builder = new StringBuilder();

            // TODO - should the "Command" substitution be done outside this class to make this more general purpose?
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



        public static IEnumerable<string> SplitText(this string s)
        {
            //int startIndex = -1;
            //bool inWord = false;
            //for (int i = 0; i < s.Length; i++)
            //{
            //    if (s[i] == ' ')
            //    {
            //        if (inWord)
            //        {
            //            yield return s.Substring(startIndex, i);
            //            inWord = false;
            //        }
            //    }
            //    else if (s[i] == '\t')
            //    {
            //        if (inWord)
            //    }
            //    else
            //    {
            //        if (!inWord)
            //        {
            //            inWord = true;
            //            startIndex = i;
            //        }
            //    }
            //}

            //if (inWord)
            //{
            //    yield return s.Substring(startIndex);
            //}

            // TODO! This is pretty inefficient...but it works...
            return s.Replace("\t", " \t ").Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
