using System.Reflection;
using Yaclops.Attributes;

namespace Yaclops.Models
{
    internal class NamedParameterEntry
    {
        public PropertyInfo Property { get; set; }
        public NamedParameterAttribute Attribute { get; set; }
        public string Description { get; set; }
    }
}
