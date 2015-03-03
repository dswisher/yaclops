using System.IO;

namespace Yaclops.Markdown.ConsoleMarkup
{
    internal abstract class MarkupItem
    {
        public abstract void Dump(TextWriter writer);
        public abstract void Write(ref bool startOfLine);
    }
}
