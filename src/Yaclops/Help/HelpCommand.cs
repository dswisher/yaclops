using System;
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
            if (Commands.Any())
            {
                ISubCommand command;
                try
                {
                    command = _parser.Parse(Commands);
                }
                catch (CommandLineParserException)
                {
                    Console.WriteLine("Information on that command is not available, as it does not exist.");
                    return;
                }

                IConsole console = new WrappedConsole();

                // TODO - print out command detail!

                console.WriteLine("Help on {0} (or any command) is not yet implemented.", command.Name());
            }
            else
            {
                IConsole console = new WrappedConsole();

                HelpBuilder.WriteCommandList(_parser.Commands, console);
            }
        }
    }
}
