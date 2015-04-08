
namespace Yaclops.Formatting
{
    internal class Document : AbstractDocumentItem
    {

        // Convenience methods
        public Paragraph AddParagraph()
        {
            Paragraph para = new Paragraph();
            Add(para);
            return para;
        }
    }
}
