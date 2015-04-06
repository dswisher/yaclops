using System;

namespace Yaclops.Parsing
{
    /// <summary>
    /// An error occured during command-line parsing.
    /// </summary>
    public class ParserConfigurationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParserConfigurationException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
