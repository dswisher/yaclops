using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var lexer = new Lexer(input);
            var state = new ParserState(CommandRoot, lexer);

            while (state.Advance())
            {
            }

            // TODO - build the parse result from the final state

            return new ParseResult();
        }

    }
}
