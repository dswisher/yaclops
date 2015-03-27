using System.Collections.Generic;
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
            ParserContext context = new ParserContext(_configuration);

            AbstractState state = new InitialState(context);
            Lexer lexer = new Lexer(text);
            while (!state.IsTerminal)
            {
                state = state.Advance(lexer.Pop());
            }

            return context.Result;
        }


        public ParseResult Parse(IEnumerable<string> args)
        {
            // TODO - if any item contains a space, wrap it in quotes
            return Parse(string.Join(" ", args));
        }

    }
}
