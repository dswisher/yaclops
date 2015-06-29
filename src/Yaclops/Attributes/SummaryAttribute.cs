using System;

namespace Yaclops.Attributes
{
    /// <summary>
    /// Used to decorate a subcommand with a summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SummaryAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="summary">The summary of the command.</param>
        public SummaryAttribute(string summary)
        {
            Summary = summary;
        }


        /// <summary>
        /// The summary.
        /// </summary>
        internal string Summary { get; private set; }
    }
}
