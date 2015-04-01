using System;

namespace Yaclops.Help
{
    /// <summary>
    /// Newfangled help command. This will replace the existing HelpCommand once the new parser is done.
    /// </summary>
    internal class HelpCommandEx : ISubCommand
    {
        public void Execute()
        {
            // TODO - implement new help command
            Console.WriteLine("HelpCommandEx.Execute is not yet implemented.");
        }
    }
}
