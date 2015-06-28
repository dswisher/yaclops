
using Yaclops.Model;

namespace Yaclops.Parsing
{
    internal class ParserNamedParameterResult
    {
        public CommandNamedParameter Parameter { get; set; }
        public string Value { get; set; }

        public ParserNamedParameterResult(CommandNamedParameter parameter, string value)
        {
            Parameter = parameter;
            Value = value;
        }
    }
}
