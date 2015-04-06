

namespace Yaclops.Parsing.Configuration
{
    internal class ParserPositionalParameter : ParserParameter
    {
        public ParserPositionalParameter(string key, bool isCollection)
            : base(key)
        {
            IsCollection = isCollection;
        }


        public bool IsCollection { get; private set; }
        public bool IsRequired { get; set; }
    }
}
