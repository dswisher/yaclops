

namespace Yaclops.Reflecting
{
    internal class ReflectedPositionalParameter
    {
        private readonly string _propertyName;
        private readonly bool _isList;

        public ReflectedPositionalParameter(string propertyName, bool isList)
        {
            _propertyName = propertyName;
            _isList = isList;
        }
    }
}
