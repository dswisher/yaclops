using System;

namespace Yaclops.Model
{
    internal class CommandPositionalParameter
    {
        public CommandPositionalParameter(string propertyName, bool isList, bool isRequired, Func<object, object> propertyTarget)
        {
            PropertyName = propertyName;
            IsList = isList;
            IsRequired = isRequired;
            PropertyTarget = propertyTarget;
        }

        public string PropertyName { get; private set; }
        public bool IsList { get; private set; }
        public bool IsRequired { get; private set; }
        public Func<object, object> PropertyTarget { get; private set; }
    }
}
