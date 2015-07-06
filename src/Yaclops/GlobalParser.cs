using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Extensions;
using Yaclops.Injecting;
using Yaclops.Model;
using Yaclops.Parsing;

namespace Yaclops
{
    /// <summary>
    /// Command line parser that just parses some global options - not subcommands.
    /// </summary>
    public class GlobalParser
    {
        private readonly GlobalParserSettings _settings;
        private readonly List<GroupOptionsEntry> _rootOptions = new List<GroupOptionsEntry>();
        private Parser _parser;
        private bool _initialized;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Settings that alter the behavior of the parser</param>
        public GlobalParser(GlobalParserSettings settings = null)
        {
            _settings = settings ?? new GlobalParserSettings();
        }



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
            _rootOptions.Add(new GroupOptionsEntry<TO>(rootOptions));
        }



        /// <summary>
        /// Parse the given command line.
        /// </summary>
        /// <param name="args">The list of arguments passed into the program.</param>
        /// <returns>The parsed command</returns>
        public bool Parse(IEnumerable<string> args)
        {
            return Parse(string.Join(" ", args.Select(x => x.Quote())));
        }



        /// <summary>
        /// Parse the given command line.
        /// </summary>
        /// <param name="input">The arguments passed into the program.</param>
        public bool Parse(string input)
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

            Console.WriteLine("Parser, result.Kind={0}", result.Kind);

            switch (result.Kind)
            {
                case ParseResultKind.DefaultCommand:
                    PropertyInjector injector = new PropertyInjector(result);
                    injector.Populate(null);
                    return true;

                case ParseResultKind.Help:
                    // TODO - implement help
                    Console.WriteLine("Help!");
                    return false;

                case ParseResultKind.InternalCommand:
                case ParseResultKind.ExternalCommand:
                    throw new NotImplementedException("Internal parser error! result state " + result.Kind + " should not occur in GlobalParser.");

                default:
                    throw new NotImplementedException("Internal parser error! Unhandled result state: " + result.Kind);
            }
        }



        private void Initialize()
        {
            // Insanity check: must have at least one command
            if (!_rootOptions.Any())
            {
                throw new CommandLineParserException("At least one set of global options must be added to the parser.");
            }

            // Create the command graph
            ModelBuilder builder = new ModelBuilder(_settings.ProgramName);

            if (_settings.EnableYaclopsCommands)
            {
                builder.AddInternalCommands();
            }

            builder.AddGroupOptions(_rootOptions);

            _parser = new Parser(builder.Root, _settings.HelpFlags, _settings.HelpVerb);
        }
    }
}
