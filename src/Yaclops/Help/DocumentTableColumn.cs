

namespace Yaclops.Help
{
    internal class DocumentTableColumn : AbstractDocumentItem
    {
        private readonly string _content;

        public DocumentTableColumn(string content)
        {
            _content = content;
        }


        public override void RenderTo(IConsole target)
        {
            target.Write(_content);
        }
    }
}
