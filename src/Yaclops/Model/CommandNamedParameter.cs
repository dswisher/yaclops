using System;
using System.Collections.Generic;
using System.Text;


namespace Yaclops.Model
{
    internal class CommandNamedParameter
    {
        private readonly List<string> _longNames = new List<string>();
        private readonly List<string> _shortNames = new List<string>();

        public CommandNamedParameter(string propertyName, bool isBool, Func<object, object> propertyTarget)
        {
            PropertyName = propertyName;
            IsBool = isBool;
            PropertyTarget = propertyTarget;
        }

        public string PropertyName { get; private set; }
        public bool IsBool { get; private set; }
        public Func<object, object> PropertyTarget { get; private set; }
        public List<string> LongNames { get { return _longNames; } }
        public List<string> ShortNames { get { return _shortNames; } }
        public string Description { get; set; }

        public string Usage
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                foreach (var sn in ShortNames)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append("|");
                    }

                    builder.Append("-");
                    builder.Append(sn);
                }

                foreach (var ln in LongNames)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append("|");
                    }

                    builder.Append("--");
                    builder.Append(ln);
                }

                // TODO - check to see if param is mandatory
                return "[" + builder + "]";
            }
        }
    }
}
