using System.Collections.Generic;
using System.Linq;


namespace Yaclops.Help
{
    internal static class HelpBuilder
    {
        public static void WriteCommandList(IEnumerable<ISubCommand> commands, IConsole console)
        {
            // TODO - need to get bisect into this list, but only once!
            var topCommands = commands
                .Where(x => !x.Name().Contains(" "))
                .OrderBy(x => x.Name())
                .ToList();

            int maxLength = FindMaxCommandLength(topCommands);

            foreach (var command in topCommands)
            {
                console.WriteLine("  {0} {1}", command.Name().PadRight(maxLength), command.Summary());
            }
        }



        private static int FindMaxCommandLength(IEnumerable<ISubCommand> commands)
        {
            return commands.Select(x => x.Name().Length).Max();
        }
    }
}
