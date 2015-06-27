
namespace Yaclops.Parsing
{
    internal enum TokenKind
    {
        ShortName,
        LongName,
        Value,
        EndOfInput
    }


    internal class Token
    {
        public TokenKind Kind { get; set; }
        public string Text { get; set; }
        public string RawInput { get; set; }
    }
}
