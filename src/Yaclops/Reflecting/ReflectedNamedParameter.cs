
namespace Yaclops.Reflecting
{
    internal class ReflectedNamedParameter
    {
        public ReflectedNamedParameter(string propertyName, bool isBool)
        {
            PropertyName = propertyName;
            IsBool = isBool;
        }


        public string PropertyName { get; private set; }
        public bool IsBool { get; set; }
    }
}
