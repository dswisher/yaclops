using Yaclops.Model;

namespace Yaclops.Parsing
{
    internal class ParserNamedParameterResult
    {
        public ParserNamedParameterResult(CommandNamedParameter parameter, string value)
        {
            Parameter = parameter;
            Value = value;
        }


        public CommandNamedParameter Parameter { get; private set; }
        public string Value { get; private set; }
    }
}
