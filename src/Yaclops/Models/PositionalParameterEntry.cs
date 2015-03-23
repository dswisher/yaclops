using System.Reflection;

namespace Yaclops.Models
{
    public class PositionalParameterEntry
    {
        public PropertyInfo Property { get; set; }
        public CommandLineParameterAttribute Attribute { get; set; }
        public bool Required { get; set; }
    }
}
