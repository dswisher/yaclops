using System;
using System.Collections.Generic;

namespace Yaclops
{
    public class CommandLineParser
    {
        private readonly List<ICommand> _commands = new List<ICommand>();


        public CommandLineParser(IEnumerable<ICommand> commands)
        {
            _commands.AddRange(commands);

            // TODO - do a sanity check for conficting commands?
        }


        internal IList<ICommand> Commands { get { return _commands; } }


        public ICommand Parse(IEnumerable<string> raw)
        {
            return ParseMinion(raw);
        }


        public ICommand ParseMinion(IEnumerable<string> raw)
        {
            var args = new ArgumentList(raw);

            ICommand matchingCommand = null;

            if (args.IsEmpty || (args.Peek().Equals("help", StringComparison.CurrentCultureIgnoreCase)))
            {
                if (!args.IsEmpty)
                {
                    args.Pop();
                }

                matchingCommand = new HelpCommand(this);
            }
            else
            {
                // Find the best match
                int maxMatches = 0;
                foreach (var command in _commands)
                {
                    int matches = args.Matches(command);
                    if (matches > maxMatches)
                    {
                        maxMatches = matches;
                        matchingCommand = command;
                    }
                }

                // If we didn't find match, we're toast
                if (matchingCommand == null)
                {
                    throw new CommandLineParserException("Unknown command.");
                }

                // Drop the command part and parse any remaining args
                args.Accept(matchingCommand);
            }

            CommandLineArgumentParser.Parse(matchingCommand, args);

            // Return what we've got
            return matchingCommand;
        }
    }
}
