using System;

namespace Yaclops
{
    /// <summary>
    /// Mark a property as a positional command-line parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineParameterAttribute : Attribute
    {
        // TODO - rename this to PositionalParameter?
    }
}
