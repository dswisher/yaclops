using System;
using System.IO;

namespace Yaclops.Markdown.ConsoleMarkup
{
    internal class TextItem : MarkupItem
    {
        private readonly string _content;

        public TextItem(string content)
        {
            _content = content;
        }

        public override void Dump(TextWriter writer)
        {
            writer.WriteLine("[Text]: '{0}'", _content);
        }

        public override void Write(ref bool startOfLine)
        {
            if (!startOfLine)
            {
                Console.Write(" ");
            }

            // TODO - swap colors

            Console.Write(_content);

            // TODO - swap colors back

            startOfLine = false;
        }
    }
}
