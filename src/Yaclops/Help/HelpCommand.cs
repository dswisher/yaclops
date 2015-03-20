using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


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
                    command = _parser.Parse(Commands);
                }
                catch (CommandLineParserException)
                {
                    console.WriteLine("Information on that command is not available, as it does not exist.");
                    return;
                }

                // TODO - print out command detail!

                console.WriteLine("Help on {0} (or any command) is not yet implemented.", command.Name());
            }
            else
            {
                // TODO - allow the name to be overridden in settings
                string command = Assembly.GetEntryAssembly().GetName().Name.ToLower();

                console.StartWrap("usage: {0}", command);

                // TODO - write global flags

                console.Write(" <command> [<args>]");

                console.EndWrap();
                console.WriteLine();

                HelpBuilder.WriteCommandList(_parser.Commands, console);
            }
        }
    }
}
