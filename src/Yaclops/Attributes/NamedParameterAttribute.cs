using System;

namespace Yaclops.Attributes
{
    /// <summary>
    /// Mark a property as a named command-line parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NamedParameterAttribute : Attribute
    {
        /// <summary>
        /// The short name for the parameter.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// The long name for the parameter. The default is the decamel of the property name.
        /// </summary>
        [Obsolete("Use the LongName attribute instead.")]
        public string LongName { get; set; }
    }
}
