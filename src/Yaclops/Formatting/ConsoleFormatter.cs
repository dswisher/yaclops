using System;
using Yaclops.Help;

namespace Yaclops.Formatting
{
    internal class ConsoleFormatter
    {
        private readonly IConsole _console;
        private int _position;
        private bool _needSeparator;



        public ConsoleFormatter(IConsole console)
        {
            _console = console;
        }



        public void Format(Document doc)
        {
            foreach (var block in doc.Children)
            {
                foreach (var item in block.Children)
                {
                    if (string.IsNullOrEmpty(item.Text))
                    {
                        continue;
                    }

                    var words = item.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var word in words)
                    {
                        if (_position + word.Length + (_needSeparator ? 1 : 0) > _console.Width)
                        {
                            NewLine();
                            // TODO - indent; need to handle edge case of indent + first word > width
                        }
                        else if (_needSeparator)
                        {
                            Whitespace(1);
                        }

                        WriteText(word);
                    }
                }

                NewLine();
            }
        }



        private void Whitespace(int length)
        {
            _console.Write(new string(' ', length));
            _position += length;
        }



        private void WriteText(string text)
        {
            _console.Write(text);
            _needSeparator = true;
            _position += text.Length;
        }



        private void NewLine()
        {
            _console.WriteLine();
            _position = 0;
            _needSeparator = false;
        }
    }
}
