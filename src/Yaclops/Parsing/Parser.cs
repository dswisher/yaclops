using Yaclops.Model;

namespace Yaclops.Parsing
{
    internal class Parser
    {
        public Parser(CommandRoot commandRoot)
        {
            CommandRoot = commandRoot;
        }


        public CommandRoot CommandRoot { get; private set; }


        public ParseResult Parse(string input)
        {
            var result = new ParseResult();
            var lexer = new Lexer(input);
            var state = new ParserState(CommandRoot, lexer);

            // Special case
            if (state.CurrentToken.Kind == TokenKind.EndOfInput)
            {
                result.Kind = ParseResultKind.Help;
                result.FinalNode = state.CurrentNode;
            }
            else
            {
                while (state.Advance())
                {
                }

                if (state.CurrentNode is Command)
                {
                    // TODO - determine if the command is internal or external...
                    result.Kind = ParseResultKind.Command;
                    result.FinalNode = state.CurrentNode;
                    result.NamedParameters = state.NamedParameters;
                    result.PositionalParameters = state.PositionalParameters;
                }
                else
                {
                    // TODO - scan forward to see if this is unique
                    // TODO - build up a pretty message
                    throw new CommandLineParserException("Ambiguous/incomplete command!");
                }

                // TODO - build the parse result from the final state
            }

            return result;
        }

    }
}
