

using System.Collections.Generic;
using System.Linq;

namespace Yaclops.Formatting
{
    internal class StyleEx
	{
        private readonly List<Entry<int>> _indents = new List<Entry<int>>();


        public int GetIndent(AbstractDocumentItem item)
        {
            var entry = _indents.FirstOrDefault(x => x.Selector == item.Tag);

            return (entry == null) ? 0 : entry.Value;
        }


        public void SetIndent(string selector, int value)
        {
            _indents.Add(new Entry<int>(selector, value));
        }


        // TODO - first line indent
        // TODO - tab stops



        private class Entry<T>
        {
            public Entry(string selector, T value)
            {
                Selector = selector;
                Value = value;
            }

            public string Selector { get; private set; }
            public T Value { get; private set; }
        }
	}
}

