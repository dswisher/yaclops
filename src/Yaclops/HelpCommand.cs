using System;
using System.Collections.Generic;
using System.Linq;


namespace Yaclops
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

                Console.WriteLine("Detailed help on '{0}' is not yet available...", command.Name());
            }
            else
            {
                Console.WriteLine("Commands:");

                foreach (var command in _parser.Commands.OrderBy(x => x.Name()))
                {
                    Console.WriteLine("   {0}", command.Name());
                }
            }
        }
    }
}
