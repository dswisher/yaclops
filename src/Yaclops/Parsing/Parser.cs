using System.Collections.Generic;
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


        public ParseResult Parse(string input, IEnumerable<string> helpFlags, string helpVerb)
        {
            var result = new ParseResult();
            var lexer = new Lexer(input);
            var state = new ParserState(CommandRoot, lexer, helpFlags, helpVerb);

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

                result.FinalNode = state.CurrentNode;
                result.NamedParameters = state.NamedParameters;
                result.PositionalParameters = state.PositionalParameters;

                if (state.HelpWanted)
                {
                    result.Kind = ParseResultKind.Help;
                }
                else if (state.CurrentNode is Command)
                {
                    if (state.CurrentNode is ExternalCommand)
                    {
                        result.Kind = ParseResultKind.ExternalCommand;
                    }
                    else if (state.CurrentNode is InternalCommand)
                    {
                        result.Kind = ParseResultKind.InternalCommand;
                    }
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
