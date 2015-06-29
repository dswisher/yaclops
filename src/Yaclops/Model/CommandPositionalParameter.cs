

namespace Yaclops.Model
{
    internal class CommandPositionalParameter
    {
        public CommandPositionalParameter(string propertyName, bool isList)
        {
            PropertyName = propertyName;
            IsList = isList;
        }

        public string PropertyName { get; private set; }
        public bool IsList { get; private set; }
    }
}
