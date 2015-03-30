using System;
using System.Collections.Generic;
using System.Linq;


namespace Yaclops
{
    /// <summary>
    /// Parse a command line
    /// </summary>
    /// <remarks>
    /// Once this is fully functional, drop the old one, make this one public and drop the Ex suffix
    /// </remarks>
    public class CommandLineParserEx
    {
        private readonly List<ISubCommand> _commands = new List<ISubCommand>();
        private bool _initialized;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commands">List of command objects to reflect</param>
        public CommandLineParserEx(IEnumerable<ISubCommand> commands)
        {
            _commands.AddRange(commands);
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// Commands can be added using the AddCommand method.
        /// </remarks>
        public CommandLineParserEx()
        {
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

            // TODO - HACK!
            return _commands.First();
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
            // Add the help command so it will be treated like any other command, even though it is internal
            // TODO - add help command

            // TODO - reflect on the given commands and build the parser configuration
        }
    }
}
