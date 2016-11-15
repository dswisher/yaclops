

namespace Yaclops.Reflecting
{
    internal class ReflectedPositionalParameter
    {
        public ReflectedPositionalParameter(string propertyName, bool isList, bool isMandatory)
        {
            PropertyName = propertyName;
            IsList = isList;
            IsMandatory = isMandatory;
        }

        public string PropertyName { get; private set; }
        public string Description { get; set; }
        public bool IsList { get; private set; }
        public bool IsMandatory { get; private set; }
    }
}
