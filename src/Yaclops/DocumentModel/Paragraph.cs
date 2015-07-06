using System.Collections.Generic;


namespace Yaclops.DocumentModel
{
    internal class Paragraph
    {
        private readonly List<Span> _spans = new List<Span>();


        public Paragraph(ParagraphStyle style)
        {
            Style = style;
        }


        public ParagraphStyle Style { get; set; }
        public IEnumerable<Span> Spans { get { return _spans; } }


        public Span AddSpan(Span span)
        {
            _spans.Add(span);
            return span;
        }


        public void AddTab()
        {
            // TODO - should this something other than a span?
            AddSpan(new Span("\t"));
        }
    }
}
