

namespace Yaclops.Parsing
{
    internal class ParserContext
    {
        public ParserContext(ParserConfiguration configuration)
        {
            Configuration = configuration;
            Result = new ParseResult();
        }

        public ParserConfiguration Configuration { get; private set; }
        public ParseResult Result { get; private set; }
    }
}
