using Yaclops.Model;

namespace Yaclops.Parsing
{
    internal enum ParseResultKind
    {
        Unknown,
        Help,
        Command
    }


    // TODO - should this be split into multiple classes with an 'exec' method or some such?
    internal class ParseResult
    {
        public ParseResultKind Kind { get; set; }
        public CommandNode FinalNode { get; set; }
    }
}
