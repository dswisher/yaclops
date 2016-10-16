using System;
using System.Linq;
using Yaclops.DocumentModel;
using Yaclops.Extensions;

namespace Yaclops.Formatting
{
    internal class ConsoleFormatter
    {

        public void Format(Document doc)
        {
            int width = int.MaxValue;
            try
            {
                width = Console.WindowWidth;
            }
            catch
            {
                // Probably in Windows Application mode or some other process where stdout is unsized 
            }

            foreach (var para in doc.Paragraphs)
            {
                // Add any extra lines
                for (int i = 0; i < para.Style.LinesBefore; i++)
                {
                    Console.WriteLine();
                }

                int pos = 0;
                int line = 1;

                foreach (var span in para.Spans)
                {
                    if (string.IsNullOrEmpty(span.Text))
                    {
                        continue;
                    }

                    var words = span.Text.SplitText();

                    foreach (var word in words)
                    {
                        // Handling tabs is fun
                        if ((word == "\t") && (para.Style.Tabs != null))
                        {
                            var spaces = para.Style.Tabs.FirstOrDefault(x => x > pos) - pos - 1;
                            if (spaces > 0)
                            {
                                // TODO - the >0 check above seems like a hack - off by one error?
                                WriteText(ref pos, new string(' ', spaces));
                            }
                            continue;
                        }

                        // If we'd extend beyond the edge, skip to a new line.
                        if ((pos > 0) && (pos + word.Length + 1 >= width))
                        {
                            pos = 0;
                            line += 1;
                            Console.WriteLine();
                        }

                        // Indent is good.
                        var indent = para.Style.Indent;
                        if (line == 1)
                        {
                            indent += para.Style.FirstLineIndent;
                        }
                        if (pos < indent)
                        {
                            WriteText(ref pos, new string(' ', indent - pos));
                        }

                        // Whitespace between words is good, too
                        if (pos > para.Style.Indent)
                        {
                            WriteText(ref pos, " ");
                        }

                        // Write the word...
                        WriteText(ref pos, word);
                    }
                }

                // End the most recent line (if any)...
                if (pos > 0)
                {
                    Console.WriteLine();
                }

                // Add any extra lines
                for (int i = 0; i < para.Style.LinesAfter; i++)
                {
                    Console.WriteLine();
                }
            }
        }



        // TODO - make pos a state variable in the class?
        private void WriteText(ref int pos, string content)
        {
            Console.Write(content);

            pos += content.Length;
        }
    }
}
