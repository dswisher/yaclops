using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Commands;
using Yaclops.Model;
using Yaclops.Parsing;


namespace Yaclops
{
    /// <summary>
    /// Parse a command line
    /// </summary>
    public class CommandLineParser<T>
    {
        private readonly CommandLineParserSettings<T> _settings;
        private readonly List<TypeEntry<T>> _types = new List<TypeEntry<T>>();
        private Parser _parser;
        private bool _initialized;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// Commands can be added using the AddCommand or AddType methods.
        /// </remarks>
        /// <param name="settings">Additional settings for the parser</param>
        public CommandLineParser(CommandLineParserSettings<T> settings = null)
        {
            _settings = settings ?? new CommandLineParserSettings<T>();
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commands">List of command objects to reflect</param>
        /// /// <param name="settings">Additional settings for the parser</param>
        public CommandLineParser(IEnumerable<T> commands, CommandLineParserSettings<T> settings = null)
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
        public CommandLineParser(IEnumerable<Type> commandTypes, Func<Type, T> factory, CommandLineParserSettings<T> settings = null)
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
            _initialized = false;
            _types.Add(new TypeEntry<T> { Type = commandType, Factory = factory });
        }



        public void AddTypes(IEnumerable<Type> commandTypes, Func<Type, T> factory)
        {
            foreach (var commandType in commandTypes)
            {
                var type = commandType;     // Avoid "access to foreach in closure" warning
                AddType(commandType, () => factory(type));
            }
        }


        // TODO - BOTH of the following should also be on the settings object
        // TODO - add a method to add details about a group, including group-specific options
        // TODO - add a method to add global-option objects


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
            if (input == null)
            {
                throw new ArgumentException("Input cannot be null", "input");
            }

            if (!_initialized)
            {
                Initialize();
                _initialized = true;
            }

            // Special case easily handled here
            if (string.IsNullOrWhiteSpace(input))
            {
                // TODO - restore proper help command - dump internals is a hack!
                new YaclopsDumpTreeCommand(_parser.CommandRoot).Execute();
                // HelpCommand.Make(_commandRoot).Execute();
                return _settings.NullCommand();
            }

            var result = _parser.Parse(input);

            // TODO - examine the result and do the right thing!

            return _settings.NullCommand();
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

            // Create the command graph
            ModelBuilder builder = new ModelBuilder();

            builder.AddTypes(_types);
            // TODO - pull info from settings, too!

            _parser = new Parser(builder.Root);
        }
    }



    public class CommandLineParser : CommandLineParser<ISubCommand>
    {
        // Doesn't seem like these constructors would be needed, but they help out Autofac

        public CommandLineParser(CommandLineParserSettings<ISubCommand> settings = null)
            : base(settings ?? new DefaultSubCommandSettings())
        {
        }


        public CommandLineParser(IEnumerable<ISubCommand> commands, CommandLineParserSettings<ISubCommand> settings = null)
            : base(commands, settings ?? new DefaultSubCommandSettings())
        {
        }
    }
}
