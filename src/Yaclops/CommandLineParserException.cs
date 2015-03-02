using System;

namespace Yaclops
{
    public class CommandLineParserException : Exception
    {
        public CommandLineParserException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
