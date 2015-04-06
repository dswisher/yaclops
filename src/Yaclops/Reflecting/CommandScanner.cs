using System.Collections.Generic;
using System.Linq;
using Yaclops.Attributes;
using Yaclops.Parsing.Configuration;

namespace Yaclops.Reflecting
{
    /// <summary>
    /// Scan ISubCommand instance and add its info tot he parser configuration.
    /// </summary>
    /// <remarks>
    /// This is the bridge between the attributes decorating an ISubCommand instance and
    /// the ParserConfiguration.
    /// </remarks>
    internal class CommandScanner
    {
        private readonly ParserConfiguration _configuration;

        public CommandScanner(ParserConfiguration configuration)
        {
            _configuration = configuration;
        }



        public ParserCommand Scan(ISubCommand subCommand)
        {
            var type = subCommand.GetType();

            // Build the command itself
            var name = type.Name.Replace("Command", string.Empty).Decamel();
            var command = _configuration.AddCommand(name);

            // Add any long-name overrides to the command
            foreach (LongNameAttribute att in type.GetCustomAttributes(typeof(LongNameAttribute), true))
            {
                command.AddLongName(att.Name);
            }

            // Pick out the named parameters
            var namedProps = type.GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(NamedParameterAttribute), true).Any());

            foreach (var prop in namedProps)
            {
                var namedProp = (NamedParameterAttribute)prop.GetCustomAttributes(typeof(NamedParameterAttribute), true).First();
                var param = command.AddNamedParameter(prop.Name, prop.PropertyType);

                // TODO - add any explicitly specified long names

                param.ShortName(namedProp.ShortName);
            }

            // Pick out the positional parameters
            var positionalProps = type.GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(PositionalParameterAttribute), true).Any());

            foreach (var prop in positionalProps)
            {
                var propType = prop.PropertyType;

                // TODO - pick out more collection types that just List<string>. (But CommandPusher needs to handle 'em!)
                bool isCollection = propType == typeof (List<string>);

                // var positionalProp = (PositionalParameterAttribute)prop.GetCustomAttributes(typeof(PositionalParameterAttribute), true).First();
                /* var param = */ command.AddPositionalParameter(prop.Name, isCollection);

                // TODO - add any explicitly specified long names
            }

            return command;
        }
    }
}
