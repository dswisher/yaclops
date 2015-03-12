using System;

namespace Yaclops
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineOptionAttribute : Attribute
    {
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
