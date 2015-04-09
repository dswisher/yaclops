using Yaclops.Help;

namespace Yaclops.Formatting
{
    internal class ConsoleFormatter
    {
        private readonly IConsole _console;

        public ConsoleFormatter(IConsole console)
        {
            _console = console;
        }



        public void Format(Document doc)
        {
            foreach (var block in doc.Children)
            {
                // TODO - handle indent
                // TODO - handle wrapping

                bool first = true;
                foreach (var item in block.Children)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        _console.Write(" ");
                    }

                    _console.Write(item.Text);
                }

                _console.WriteLine();
            }
        }
    }
}
