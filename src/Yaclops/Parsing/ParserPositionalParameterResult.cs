using System.Collections.Generic;
using Yaclops.Model;

namespace Yaclops.Parsing
{
    internal class ParserPositionalParameterResult
    {
        public CommandPositionalParameter Parameter { get; private set; }
        public List<string> Values { get; private set; }

        public ParserPositionalParameterResult(CommandPositionalParameter parameter, string firstValue)
        {
            Values = new List<string> { firstValue };
            Parameter = parameter;
        }
    }
}
