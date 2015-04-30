using System.Collections.Generic;


namespace Yaclops.Formatting
{
    internal abstract class AbstractDocumentItem
    {
        private readonly List<AbstractDocumentItem> _children = new List<AbstractDocumentItem>();

        public IEnumerable<AbstractDocumentItem> Children { get { return _children; } }


        protected AbstractDocumentItem()
        {
            Style = new Style();
        }


        public void Add(AbstractDocumentItem child)
        {
            _children.Add(child);
        }

        public virtual string Text { get { return string.Empty; } }
        public abstract string Tag { get; }
        public Style Style { get; set; }


        public TextBlock AddBlock(string text)
        {
            var block = new TextBlock(text);
            Add(block);
            return block;
        }
    }
}
