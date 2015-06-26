

namespace Yaclops.Parsing.Configuration
{
    internal class ParserNamedParameter : ParserParameter
    {
        public ParserNamedParameter(string key, bool isBool)
            : base(key)
        {
            IsBool = isBool;
        }


        public bool IsBool { get; private set; }
    }
}
