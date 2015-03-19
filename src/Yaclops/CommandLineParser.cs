using System;
using System.Collections.Generic;
using Yaclops.Help;

namespace Yaclops
{
    public class CommandLineParser
    {
        private readonly List<ISubCommand> _commands = new List<ISubCommand>();


        public CommandLineParser(IEnumerable<ISubCommand> commands)
        {
            _commands.AddRange(commands);

            // TODO - do a sanity check for conficting commands?
        }


        internal IList<ISubCommand> Commands { get { return _commands; } }


        public ISubCommand Parse(IEnumerable<string> raw)
        {
            return ParseMinion(raw);
        }


        public ISubCommand ParseMinion(IEnumerable<string> raw)
        {
            var args = new ArgumentList(raw);

            ISubCommand matchingCommand = null;

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
