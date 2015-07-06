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
            int width = Console.WindowWidth;    // TODO - WindowWidth or BufferWidth?

            foreach (var para in doc.Paragraphs)
            {
                int pos = 0;

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
                            WriteText(ref pos, new string(' ', spaces));
                            continue;
                        }

                        // If we'd extend beyond the edge, skip to a new line.
                        if ((pos > 0) && (pos + word.Length + 1 > width))
                        {
                            pos = 0;
                            Console.WriteLine();
                        }

                        // Indent is good.
                        if (pos < para.Style.Indent)
                        {
                            WriteText(ref pos, new string(' ', para.Style.Indent - pos));
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
