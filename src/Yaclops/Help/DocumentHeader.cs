

namespace Yaclops.Help
{
    internal class DocumentHeader : AbstractDocumentItem
    {
        private readonly string _title;

        public DocumentHeader(string title)
        {
            _title = title;
        }


        public override void RenderTo(IConsole target)
        {
            target.WriteLine(_title);
        }
    }
}
