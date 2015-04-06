using System.Collections.Generic;
using System.Linq;
using Yaclops.Parsing.Configuration;
using Yaclops.Parsing.States;

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
            ParserContext context = new ParserContext(_configuration, text);

            AbstractState state = new InitialState(context);
            while (!state.IsTerminal)
            {
                state = state.Advance();
            }

            return context.Result;
        }


        public ParseResult Parse(IEnumerable<string> args)
        {
            return Parse(string.Join(" ", args.Select(Quote)));
        }


        private string Quote(string s)
        {
            // TODO - check for other whitespace chars
            if (s.Contains(' '))
            {
                return '"' + s + '"';
            }

            return s;
        }
    }
}
