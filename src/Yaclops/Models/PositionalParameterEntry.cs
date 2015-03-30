using System.Reflection;
using Yaclops.Attributes;

namespace Yaclops.Models
{
    internal class PositionalParameterEntry
    {
        public PropertyInfo Property { get; set; }
        public PositionalParameterAttribute Attribute { get; set; }
        public bool Required { get; set; }
    }
}
