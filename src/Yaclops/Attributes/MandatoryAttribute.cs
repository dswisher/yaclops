using System;

namespace Yaclops.Attributes
{
    /// <summary>
    /// Mark a parameter as mandatory (aka required)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MandatoryAttribute : Attribute
    {
    }
}
