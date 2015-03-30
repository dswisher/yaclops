using System;

namespace Yaclops.Attributes
{
    /// <summary>
    /// Mark a property as a positional command-line parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PositionalParameterAttribute : Attribute
    {
    }
}
