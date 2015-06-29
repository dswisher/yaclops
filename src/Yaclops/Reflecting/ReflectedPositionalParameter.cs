

namespace Yaclops.Reflecting
{
    internal class ReflectedPositionalParameter
    {
        public ReflectedPositionalParameter(string propertyName, bool isList)
        {
            PropertyName = propertyName;
            IsList = isList;
        }

        public string PropertyName { get; private set; }
        public bool IsList { get; private set; }
    }
}
