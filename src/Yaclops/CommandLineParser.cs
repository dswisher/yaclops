using System;
using System.Collections.Generic;
using Yaclops.Help;

namespace Yaclops
{
    /// <summary>
    /// Parse a command line
    /// </summary>
    public class CommandLineParser
    {
        private readonly List<ISubCommand> _commands = new List<ISubCommand>();


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commands">The list of commands to expose to the parser</param>
        public CommandLineParser(IEnumerable<ISubCommand> commands)
        {
            _commands.AddRange(commands);

            // TODO - do a sanity check for conficting commands?
        }


        /// <summary>
        /// Parse the given command line.
        /// </summary>
        /// <param name="raw">The raw list of arguments passed into the program.</param>
        /// <param name="checkRequired">If true, an exception will be thrown if a required parameter is omitted.</param>
        /// <returns></returns>
        public ISubCommand Parse(IEnumerable<string> raw, bool checkRequired = true)
        {
            return ParseMinion(raw, checkRequired);
        }


        internal IList<ISubCommand> Commands { get { return _commands; } }


        private ISubCommand ParseMinion(IEnumerable<string> raw, bool checkRequired)
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

            CommandLineArgumentParser.Parse(matchingCommand, args, checkRequired);

            // Return what we've got
            return matchingCommand;
        }
    }
}
