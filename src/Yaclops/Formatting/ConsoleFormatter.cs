using System;
using Yaclops.DocumentModel;

namespace Yaclops.Formatting
{
    internal class ConsoleFormatter
    {

        public void Format(Document doc)
        {
            foreach (var para in doc.Paragraphs)
            {
                foreach (var span in para.Spans)
                {
                    Console.Write(span.Text);
                    // TODO - whitespace?
                }

                Console.WriteLine();
            }
        }
    }
}
