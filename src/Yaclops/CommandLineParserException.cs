using System;

namespace Yaclops
{
    /// <summary>
    /// An error occurred during command line parsing.
    /// </summary>
    public class CommandLineParserException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommandLineParserException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
