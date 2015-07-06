using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Yaclops
{
    /// <summary>
    /// Fluenty interface to assist with setting up parsers.
    /// </summary>
    /// <typeparam name="T">The base type/interface for all the commands.</typeparam>
    public class ParserBuilder<T>
    {
        private readonly List<Type> _commandTypes = new List<Type>();
        private readonly List<object> _globals = new List<object>();
        private readonly Type _type = typeof(T);
        private Func<Type, T> _factory;
        private CommandLineParserSettings<T> _settings;


        private ParserBuilder()
        {
            _factory = DefaultFactory;
        }


        /// <summary>
        /// Start building a parser by scanning one or more assemblies for command objects.
        /// </summary>
        /// <param name="assemblies">The assemblies to scan.</param>
        /// <returns></returns>
        public static ParserBuilder<T> FromCommands(params Assembly[] assemblies)
        {
            var builder = new ParserBuilder<T>();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(x => builder._type.IsAssignableFrom(x) && !x.IsInterface)
                    .ToList();

                if (types.Any())
                {
                    builder._commandTypes.AddRange(types);
                }
            }

            return builder;
        }



        /// <summary>
        /// Set the factory to use to construct instances.
        /// </summary>
        /// <param name="factory">The factory</param>
        /// <returns>The builder, to keep the fluentness alive</returns>
        public ParserBuilder<T> WithFactory(Func<Type, T> factory)
        {
            _factory = factory;
            return this;
        }



        /// <summary>
        /// Add global parameters to the parser
        /// </summary>
        /// <param name="globals">An object contain global parameters</param>
        /// <returns>The builder, to keep the fluentness alive</returns>
        public ParserBuilder<T> WithGlobals(object globals)
        {
            _globals.Add(globals);
            return this;
        }



        /// <summary>
        /// Set the settings that are used to alter the default behavior of the parser.
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <returns>The builder, to keep the fluentness alive</returns>
        public ParserBuilder<T> WithSettings(CommandLineParserSettings<T> settings)
        {
            _settings = settings;
            return this;
        }



        /// <summary>
        /// Get the resulting parser
        /// </summary>
        public CommandLineParser<T> Parser
        {
            get
            {
                var parser = new CommandLineParser<T>(_settings);

                parser.AddTypes(_commandTypes, _factory);

                foreach (var g in _globals)
                {
                    parser.AddGlobalOptions(g);
                }

                return parser;
            }
        }



        private T DefaultFactory(Type type)
        {
            return (T)Activator.CreateInstance(type);
        }
    }
}
