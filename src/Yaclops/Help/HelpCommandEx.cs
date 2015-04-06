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


        public void Execute()
        {
            // TODO - implement new help command
            Console.WriteLine("HelpCommandEx.Execute is not yet implemented.");
        }
    }
}
