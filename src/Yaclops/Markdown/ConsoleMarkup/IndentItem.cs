using System;
using System.IO;


namespace Yaclops.Markdown.ConsoleMarkup
{
    internal class IndentItem : MarkupItem
    {
        private readonly int _spaces;

        public IndentItem(int spaces)
        {
            _spaces = spaces;
        }


        public override void Dump(TextWriter writer)
        {
            writer.WriteLine("[indent]: {0} spaces", _spaces);
        }


        public override void Write(ref bool startOfLine)
        {
            Console.Write(new string(' ', _spaces));
        }
    }
}
