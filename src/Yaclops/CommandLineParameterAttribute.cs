using System;

namespace Yaclops
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineParameterAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
