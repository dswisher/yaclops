
namespace Yaclops.DocumentModel
{
    internal class Span
    {
        public Span(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
        public SpanStyle Style { get; set; }
    }
}
