using System;

namespace Yaclops
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineOptionAttribute : Attribute
    {
        public string ShortName { get; set; }
    }
}
