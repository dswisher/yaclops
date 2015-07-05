using System.Collections.Generic;

namespace Yaclops.DocumentModel
{
    internal class Document
    {
        private readonly List<Paragraph> _paragraphs = new List<Paragraph>();

        public DocumentStyle Style { get; set; }
        public IEnumerable<Paragraph> Paragraphs { get { return _paragraphs; } }

        public Paragraph AddParagraph(Paragraph paragraph)
        {
            _paragraphs.Add(paragraph);
            return paragraph;
        }
    }
}
