

namespace Yaclops.Parsing
{
    internal enum TokenKind
    {
        ShortName,
        LongName,
        Value
    }


    internal class Token
    {
        public TokenKind Kind { get; set; }
        public string Text { get; set; }
    }
}
