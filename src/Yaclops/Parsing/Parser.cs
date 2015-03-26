using System.Collections.Generic;

namespace Yaclops.Parsing
{
    internal class Parser
    {
        private readonly ParserConfiguration _configuration;


        public Parser(ParserConfiguration configuration)
        {
            _configuration = configuration;
        }


        public ParseResult Parse(string text)
        {
            ParseResult result = new ParseResult();

            // Quick hacks to get first unit test to pass
            if (string.IsNullOrEmpty(text))
            {
                result.Command = _configuration.DefaultCommand;
            }
            else if (_configuration.IsCommand(text))
            {
                result.Command = text;
            }

            // TODO
            
            return result;
        }


        public ParseResult Parse(IEnumerable<string> args)
        {
            return Parse(string.Join(" ", args));
        }

    }
}
