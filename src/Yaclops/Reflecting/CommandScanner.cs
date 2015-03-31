using System.Linq;
using Yaclops.Attributes;
using Yaclops.Parsing;

namespace Yaclops.Reflecting
{
    internal class CommandScanner
    {
        private readonly ParserConfiguration _configuration;

        public CommandScanner(ParserConfiguration configuration)
        {
            _configuration = configuration;
        }



        public void Scan(ISubCommand subCommand)
        {
            var type = subCommand.GetType();

            // Build the command itself
            var name = type.Name.Replace("Command", string.Empty).Decamel();
            var command = _configuration.AddCommand(name);

            // Pick out the named parameters
            var namedProps = type.GetProperties()
                .Where(x => x.GetCustomAttributes(typeof (NamedParameterAttribute), true).Any());

            foreach (var prop in namedProps)
            {
                // var propName = "-" + prop.Name.Decamel('-').ToLower();
                /* var param = */ command.AddNamedParameter(prop.Name);

                // TODO - add any explicitly specified long names
                // TODO - add short any specified short names
            }

            // Pick out the positional parameters
            // TODO - positional parameters
        }
    }
}
