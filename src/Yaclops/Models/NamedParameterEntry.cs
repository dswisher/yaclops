using System.Reflection;

namespace Yaclops.Models
{
    internal class NamedParameterEntry
    {
        public PropertyInfo Property { get; set; }
        public CommandLineOptionAttribute Attribute { get; set; }
        public string Description { get; set; }
    }
}
