using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Attributes;
using Yaclops.Parsing.Configuration;

namespace Yaclops.Help
{
    /// <summary>
    /// Newfangled help command. This will replace the existing HelpCommand once the new parser is done.
    /// </summary>
    [LongName("help")]
    internal class HelpCommandEx : ISubCommand
    {
        private readonly ParserConfiguration _configuration;
        private readonly IConsole _console;

        public HelpCommandEx(ParserConfiguration configuration, IConsole console)
        {
            _configuration = configuration;
            _console = console;
        }


        [PositionalParameter]
        public List<string> Commands { get; set; }


        public ParserCommand Target { get; set; }


        public void Execute()
        {
            if (Target == null)
            {
                ListCommands();
            }
            else
            {
                OneCommand();
            }
        }


        private void ListCommands()
        {
            _console.StartWrap("usage: {0}", HelpBuilder.ExeName());

            // TODO - write global flags

            _console.Write(" <command> [<args>]");

            _console.EndWrap();
            _console.WriteLine();

            int maxLength = _configuration.Commands.Select(x => x.Text.Length).Max();
            foreach (var command in _configuration.Commands.OrderBy(x => x.Text))
            {
                _console.WriteLine("   {0}   {1}", command.Text.PadRight(maxLength), command.Summary);
            }

            _console.WriteLine();
            _console.WriteLine("See '{0} help <command>' to read about a specific subcommand.", HelpBuilder.ExeName());
        }



        private void OneCommand()
        {
            Console.WriteLine("HelpCommandEx.Execute is not yet implemented - OneCommand: '{0}'.", Target.Text);
            // TODO - implement new help command
        }
    }
}
