using System;
using System.Linq;
using CommonMark.Syntax;
using Yaclops.Markdown.ConsoleMarkup;

namespace Yaclops.Markdown
{
    internal class ConsolePrinter
    {

        public static void WriteAst(Block ast)
        {
            var markup = new AstConverter().Convert(ast).ToList();

            // TODO - dump out the raw content to help with debugging
            //foreach (var item in markup)
            //{
            //    item.Dump(Console.Out);
            //}

            // TODO - print out the pretty stuff!
            bool startOfLine = true;
            foreach (var item in markup)
            {
                item.Write(ref startOfLine);
            }

            if (!startOfLine)
            {
                Console.WriteLine();
            }

        }

    }
}
