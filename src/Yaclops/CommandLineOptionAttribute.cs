using System;

namespace Yaclops
{
    /// <summary>
    /// Mark a property as a command-line option (named parameter).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineOptionAttribute : Attribute
    {
        // TODO - rename this to NamedParameter?

        /// <summary>
        /// The short name for the option.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// The long name for the option. The default is the decamel of the property name.
        /// </summary>
        public string LongName { get; set; }
    }
}
