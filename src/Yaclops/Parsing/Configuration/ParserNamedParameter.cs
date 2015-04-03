using System;

namespace Yaclops.Parsing.Configuration
{
    internal class ParserNamedParameter : ParserParameter
    {
        public ParserNamedParameter(string key, Type type)
            : base(key)
        {
            IsBool = type == typeof(bool);
        }


        public bool IsBool { get; private set; }
    }
}
