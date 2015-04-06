using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Help;
using Yaclops.Parsing;
using Yaclops.Parsing.Configuration;
using Yaclops.Reflecting;


namespace Yaclops
{
    /// <summary>
    /// Parse a command line
    /// </summary>
    /// <remarks>
    /// Once this is fully functional, drop the old one, make this one public and drop the Ex suffix
    /// </remarks>
    public class CommandLineParser
    {
        private readonly ParserConfiguration _configuration = new ParserConfiguration();
        private readonly List<ISubCommand> _commands = new List<ISubCommand>();
        private readonly Dictionary<string, ISubCommand> _commandMap = new Dictionary<string, ISubCommand>();
        private readonly HelpCommand _helpCommand;
        private bool _initialized;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commands">List of command objects to reflect</param>
        public CommandLineParser(IEnumerable<ISubCommand> commands) : this()
        {
            _commands.AddRange(commands);
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// Commands can be added using the AddCommand method.
        /// </remarks>
        public CommandLineParser()
        {
            // Seed the command list with the help command
            _helpCommand = new HelpCommand(_configuration, new WrappedConsole());
            _commands.Add(_helpCommand);
        }



        /// <summary>
        /// Parse the given command line.
        /// </summary>
        /// <param name="raw">The raw list of arguments passed into the program.</param>
        /// <returns>The parsed command</returns>
        public ISubCommand Parse(IEnumerable<string> raw)
        {
            if (!_initialized)
            {
                Initialize();
                _initialized = true;
            }

            Parser parser = new Parser(_configuration);

            var result = parser.Parse(raw);

            if (result.Errors.Any())
            {
                // TODO - build better error text if there are multiple errors
                throw new CommandLineParserException(result.Errors.First());
            }

            if (result.GlobalValues.Any(x => x.Name == "Help"))
            {
                // TODO - help!
                Console.WriteLine("**** HELP!");
            }

            // Find the command...
            var command = _commandMap[result.Command.Text];

            // Populate all the parameters on the command based on the parse result...
            var pusher = new CommandPusher(result);

            pusher.Push(command);

            // TODO - what about the global values?

            // If this is the help command, give it a little assist by looking up the help target (if any)...
            if (command == _helpCommand)
            {
                if ((_helpCommand.Commands != null) && (_helpCommand.Commands.Any()))
                {
                    string text = string.Join(" ", _helpCommand.Commands);

                    var helpTarget = _commandMap[text];

                    if (helpTarget == null)
                    {
                        // TODO - better error handling
                        throw new CommandLineParserException("No help for command '{0}', as it is not known.", text);
                    }

                    _helpCommand.Target = _configuration.Commands.FirstOrDefault(x => x.Text == text);
                }
            }

            // Return the populated, ready-to-roll command...
            return command;
        }



        /// <summary>
        /// Add a command to the list of commands.
        /// </summary>
        /// <remarks>
        /// This must be called before the first call to Parse().
        /// </remarks>
        /// <param name="command">The command to add.</param>
        public void AddCommand(ISubCommand command)
        {
            if (_initialized)
            {
                throw new InvalidOperationException("Cannot add a command after Parse has been called.");
            }

            _commands.Add(command);
        }



        private void Initialize()
        {
            // TODO - allow help options to be configured via settings
            // _configuration.AddNamedParameter("Help", true).AddShortName("h").AddShortName("?");

            // Scan all the commands for their options...
            CommandScanner scanner = new CommandScanner(_configuration);

            foreach (var c in _commands)
            {
                // Add the command to the configuration based on attribute decorations...
                var parserCommand = scanner.Scan(c);

                // Add the command to the map so we can find it after parsing...
                _commandMap.Add(parserCommand.Text, c);

                // Set help as the default command
                if (c == _helpCommand)
                {
                    _configuration.DefaultCommand = parserCommand;
                }
            }
        }
    }
}
