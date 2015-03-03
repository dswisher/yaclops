using System;
using System.IO;

namespace Yaclops.Markdown.ConsoleMarkup
{
    internal class NewlineItem : MarkupItem
    {
        public override void Dump(TextWriter writer)
        {
            writer.WriteLine("[Newline]");
        }

        public override void Write(ref bool startOfLine)
        {
            // TODO - allow multiple newlines in a row?
            if (!startOfLine)
            {
                Console.WriteLine();
                startOfLine = true;
            }
        }
    }
}
