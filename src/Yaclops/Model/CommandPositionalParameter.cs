using System;
using System.Text;
using Yaclops.Extensions;

namespace Yaclops.Model
{
    internal class CommandPositionalParameter
    {
        public CommandPositionalParameter(string propertyName, bool isList, bool isMandatory, Func<object, object> propertyTarget)
        {
            PropertyName = propertyName;
            IsList = isList;
            IsMandatory = isMandatory;
            PropertyTarget = propertyTarget;
        }

        public string PropertyName { get; private set; }
        public bool IsList { get; private set; }
        public bool IsMandatory { get; private set; }
        public Func<object, object> PropertyTarget { get; private set; }
        public string Description { get; set; }

        public string Usage
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                if (!IsMandatory)
                {
                    builder.Append("[");
                }

                builder.Append("<");
                builder.Append(string.Join("-", PropertyName.Decamel()));
                builder.Append(">");

                if (!IsMandatory)
                {
                    builder.Append("]");
                }

                return builder.ToString();
            }
        }
    }
}
