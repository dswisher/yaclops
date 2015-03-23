using System.Collections.Generic;
using System.Linq;


namespace Yaclops.Help
{
    internal class HelpCommand : ISubCommand
    {
        private readonly CommandLineParser _parser;


        public HelpCommand(CommandLineParser parser)
        {
            _parser = parser;
            Commands = new List<string>();
        }


        [CommandLineParameter]
        public List<string> Commands { get; private set; }


        public void Execute()
        {
            IConsole console = new WrappedConsole();

            if (Commands.Any())
            {
                ISubCommand command;
                try
                {
                    command = _parser.Parse(Commands, false);
                }
                catch (CommandLineParserException)
                {
                    console.WriteLine("Information on that command is not available, as it does not exist.");
                    return;
                }

                // TODO - how to get help on multi-word commands, like bisect? ('sample help bisect' should show *something*)

                // Start off with a blank line...seems cleaner to me...
                console.WriteLine();

                // Print the name
                console.WriteTitle("Name");
                console.WriteLine("{0} {1} - {2}", HelpBuilder.ExeName(), command.Name(), command.Summary());

                // Print the synopsis
                console.WriteLine();
                console.WriteTitle("Synopsis");
                HelpBuilder.WriteSynopsis(command, console);

                // Long description
                var desc = command.Description();
                if (!string.IsNullOrEmpty(desc))
                {
                    console.WriteLine();
                    console.WriteTitle("Description");
                    console.WriteLine(command.Description());
                }

                // Option details
                console.WriteLine();
                console.WriteTitle("Options");
                HelpBuilder.WriteOptionDetails(command, console);

                // TODO - examples
                // TODO - see also?
            }
            else
            {
                console.StartWrap("usage: {0}", HelpBuilder.ExeName());

                // TODO - write global flags

                console.Write(" <command> [<args>]");

                console.EndWrap();
                console.WriteLine();

                HelpBuilder.WriteCommandList(_parser.Commands, console);
            }
        }
    }
}
