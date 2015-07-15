using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Commands;
using Yaclops.Extensions;
using Yaclops.Injecting;
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
        private readonly List<GroupOptionsEntry> _groupOptions = new List<GroupOptionsEntry>();
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
        /// Add a single command object to the parser's list of known commands.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public void AddCommand(T command)
        {
            AddType(command.GetType(), () => command);
        }



        /// <summary>
        /// Add multiple command objects to the parser's list of known commands.
        /// </summary>
        /// <param name="commands"></param>
        public void AddCommands(IEnumerable<T> commands)
        {
            foreach (var command in commands)
            {
                AddCommand(command);
            }
        }



        /// <summary>
        /// Add a type to the parser's list of known commands.
        /// </summary>
        /// <param name="commandType">The type to add.</param>
        /// <param name="factory">A factory to create the command object, if parsed.</param>
        public void AddType(Type commandType, Func<T> factory)
        {
            _initialized = false;
            _types.Add(new TypeEntry<T> { Type = commandType, Factory = factory });
        }



        /// <summary>
        /// Add a set of types to the parser's list of known commands.
        /// </summary>
        /// <param name="commandTypes">The list of types to add.</param>
        /// <param name="factory">A factory to create the command objects, if parsed.</param>
        public void AddTypes(IEnumerable<Type> commandTypes, Func<Type, T> factory)
        {
            foreach (var commandType in commandTypes)
            {
                var type = commandType;     // Avoid "access to foreach in closure" warning
                AddType(commandType, () => factory(type));
            }
        }


        // TODO - add a method to add details about a group, including group-specific options


        /// <summary>
        /// Add global options
        /// </summary>
        /// <remarks>
        /// This can be called multiple times with multiple objects, and their options are combined.
        /// </remarks>
        /// <param name="rootOptions"></param>
        public void AddGlobalOptions<TO>(TO rootOptions)
        {
            _initialized = false;
            _groupOptions.Add(new GroupOptionsEntry<TO>(rootOptions));
        }



        /// <summary>
        /// Parse the given command line.
        /// </summary>
        /// <param name="args">The list of arguments passed into the program.</param>
        /// <returns>The parsed command</returns>
        public T Parse(IEnumerable<string> args)
        {
            return Parse(string.Join(" ", args.Select(x => x.Quote())));
        }



        /// <summary>
        /// Parse the given command line.
        /// </summary>
        /// <param name="input">The arguments passed into the program.</param>
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

            var result = _parser.Parse(input);

            switch (result.Kind)
            {
                case ParseResultKind.Help:
                case ParseResultKind.DefaultCommand:    // TODO - make default command configurable
                    HelpCommand.Make(result.FinalNode, _settings).Execute();
                    return _settings.NullCommand();

                case ParseResultKind.ExternalCommand:
                    return HandleExternalCommand(result);

                case ParseResultKind.InternalCommand:
                    return HandleInternalCommand(result);

                default:
                    throw new NotImplementedException("Internal parser error! Unhandled result state: " + result.Kind);
            }
        }



        private T HandleInternalCommand(ParseResult result)
        {
            var node = (InternalCommand)result.FinalNode;

            node.Worker(_parser.CommandRoot, result.FinalNode);

            return _settings.NullCommand();
        }



        private T HandleExternalCommand(ParseResult result)
        {
            var node = (ExternalCommand<T>)result.FinalNode;

            T command = node.Factory();

            PropertyInjector injector = new PropertyInjector(result);
            injector.Populate(command);

            return command;
        }



        private void Initialize()
        {
            // Insanity check: must have at least one command
            if (!_types.Any())
            {
                throw new CommandLineParserException("At least one command or type must be added to the parser.");
            }

            // Create the command graph
            ModelBuilder builder = new ModelBuilder(_settings.ProgramName);

            builder.AddTypes(_types);

            if (_settings.EnableYaclopsCommands)
            {
                builder.AddInternalCommands();
            }

            builder.AddGroupOptions(_groupOptions);

            _parser = new Parser(builder.Root, _settings.HelpFlags, _settings.HelpVerb);
        }
    }



    /// <summary>
    /// A command line parser using the built-in ISubCommand.
    /// </summary>
    public class CommandLineParser : CommandLineParser<ISubCommand>
    {
        // Doesn't seem like these constructors would be needed, but they help out Autofac

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Optional settings for the parser.</param>
        public CommandLineParser(CommandLineParserSettings<ISubCommand> settings = null)
            : base(settings ?? new DefaultSubCommandSettings())
        {
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="commands">A list of command objects to add to the parser.</param>
        /// <param name="settings">Optional settings for the parser.</param>
        public CommandLineParser(IEnumerable<ISubCommand> commands, CommandLineParserSettings<ISubCommand> settings = null)
            : base(commands, settings ?? new DefaultSubCommandSettings())
        {
        }
    }
}
