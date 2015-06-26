using System;
using System.Collections.Generic;
using System.Linq;


namespace Yaclops
{
    /// <summary>
    /// Parse a command line
    /// </summary>
    public class CommandLineParser<T>
    {
        private readonly CommandLineParserSettings _settings;
        private readonly List<TypeEntry> _types = new List<TypeEntry>();
        private bool _initialized;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// Commands can be added using the AddCommand or AddType methods.
        /// </remarks>
        /// <param name="settings">Additional settings for the parser</param>
        public CommandLineParser(CommandLineParserSettings settings = null)
        {
            _settings = settings ?? new CommandLineParserSettings();
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commands">List of command objects to reflect</param>
        /// /// <param name="settings">Additional settings for the parser</param>
        public CommandLineParser(IEnumerable<T> commands, CommandLineParserSettings settings = null)
            : this(settings)
        {
            foreach (var command in commands)
            {
                AddCommand(command);
            }
        }



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandTypes">List of command types on which to reflect</param>
        /// <param name="factory">A factory that can be used to create objects of the various command types</param>
        /// <param name="settings">Additional settings for the parser</param>
        public CommandLineParser(IEnumerable<Type> commandTypes, Func<Type, T> factory, CommandLineParserSettings settings = null)
            : this(settings)
        {
            foreach (var commandType in commandTypes)
            {
                var type = commandType;     // Avoid "access to foreach in closure" warning
                AddType(commandType, () => factory(type));
            }
        }



        /// <summary>
        /// Add a command object to the list of commands.
        /// </summary>
        /// <remarks>
        /// This must be called before the first call to Parse().
        /// </remarks>
        /// <param name="command">The command to add.</param>
        public void AddCommand(T command)
        {
            AddType(command.GetType(), () => command);
        }



        public void AddCommands(IEnumerable<T> commands)
        {
            foreach (var command in commands)
            {
                AddCommand(command);
            }
        }



        public void AddType(Type commandType, Func<T> factory)
        {
            if (_initialized)
            {
                throw new InvalidOperationException("Cannot add a command after Parse has been called.");
            }

            _types.Add(new TypeEntry { Type = commandType, Factory = factory });
        }



        public void AddTypes(IEnumerable<Type> commandTypes, Func<Type, T> factory)
        {
            foreach (var commandType in commandTypes)
            {
                var type = commandType;     // Avoid "access to foreach in closure" warning
                AddType(commandType, () => factory(type));
            }
        }



        /// <summary>
        /// Parse the given command line.
        /// </summary>
        /// <param name="args">The list of arguments passed into the program.</param>
        /// <returns>The parsed command</returns>
        public T Parse(IEnumerable<string> args)
        {
            return Parse(string.Join(" ", args.Select(Quote)));
        }



        /// <summary>
        /// Parse the given command line.
        /// </summary>
        /// <param name="input">The list of arguments passed into the program.</param>
        /// <returns>The parsed command</returns>
        public T Parse(string input)
        {
            if (!_initialized)
            {
                Initialize();
                _initialized = true;
            }

            // TODO
            return default(T);
        }



        private string Quote(string s)
        {
            // TODO - check for other whitespace chars
            if (s.Contains(' '))
            {
                return '"' + s + '"';
            }

            return s;
        }



        private void Initialize()
        {
            // Insanity check: must have at least one command
            if (!_types.Any())
            {
                throw new CommandLineParserException("At least one command or type must be added to the parser.");
            }

            // TODO
        }



        private class TypeEntry
        {
            public Type Type { get; set; }
            public Func<T> Factory { get; set; }
        }
    }



    public class CommandLineParser : CommandLineParser<ISubCommand>
    {
        // Doesn't seem like these constructors would be needed, but they help out Autofac

        public CommandLineParser(CommandLineParserSettings settings = null)
            : base(settings)
        {
        }


        public CommandLineParser(IEnumerable<ISubCommand> commands, CommandLineParserSettings settings = null)
            : base(commands, settings)
        {
        }
    }
}
