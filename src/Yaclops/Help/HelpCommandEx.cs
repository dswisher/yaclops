using System;
using System.Collections.Generic;
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

        public HelpCommandEx(ParserConfiguration configuration)
        {
            _configuration = configuration;
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
            Console.WriteLine("HelpCommandEx.Execute is not yet implemented - ListCommands.");
            // TODO - implement new help command
        }


        private void OneCommand()
        {
            Console.WriteLine("HelpCommandEx.Execute is not yet implemented - OneCommand: '{0}'.", Target.Text);
            // TODO - implement new help command
        }
    }
}
