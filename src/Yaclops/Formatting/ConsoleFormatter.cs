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
                foreach (var item in block.Children)
                {
                    // TODO - way too simplistic, but want to get the first unit test to pass!
                    _console.WriteLine(item.Text);
                }
            }
        }
    }
}
