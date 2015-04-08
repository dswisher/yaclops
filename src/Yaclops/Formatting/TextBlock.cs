
namespace Yaclops.Formatting
{
    internal class TextBlock : AbstractDocumentItem
    {
        private readonly string _text;

        public TextBlock(string text)
        {
            _text = text;
        }

        public override string Text
        {
            get { return _text; }
        }
    }
}
