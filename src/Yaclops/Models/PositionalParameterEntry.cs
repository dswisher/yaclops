using System.Reflection;

namespace Yaclops.Models
{
    internal class PositionalParameterEntry
    {
        public PropertyInfo Property { get; set; }
        public CommandLineParameterAttribute Attribute { get; set; }
        public bool Required { get; set; }
    }
}
