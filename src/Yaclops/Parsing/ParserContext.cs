

namespace Yaclops.Parsing
{
    internal class ParserContext
    {
        public ParserContext(ParserConfiguration configuration, string text)
        {
            Configuration = configuration;
            Result = new ParseResult();
            Lexer = new Lexer(text);
        }

        public ParserConfiguration Configuration { get; private set; }
        public ParseResult Result { get; private set; }
        public Lexer Lexer { get; private set; }

        // TODO - this needs to be a class on its own right, that knows how to pull the command object
        public string Command { get; set; }
    }
}
