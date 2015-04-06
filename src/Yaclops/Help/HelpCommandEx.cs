using System;
using Yaclops.Attributes;

namespace Yaclops.Help
{
    /// <summary>
    /// Newfangled help command. This will replace the existing HelpCommand once the new parser is done.
    /// </summary>
    [LongName("help")]
    internal class HelpCommandEx : ISubCommand
    {
        public void Execute()
        {
            // TODO - implement new help command
            Console.WriteLine("HelpCommandEx.Execute is not yet implemented.");
        }
    }
}
