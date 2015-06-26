using System.Collections.Generic;
using System.Linq;

namespace Yaclops.Parsing.Configuration
{
    internal class ParserConfiguration
    {
        private readonly List<ParserCommand> _commands = new List<ParserCommand>();
        private readonly List<ParserNamedParameter> _namedParameters = new List<ParserNamedParameter>();


        public ParserCommand AddCommand(string commandText)
        {
            if (_commands.Any(x => x.Text == commandText))
            {
                throw new ParserConfigurationException("Cannot add duplicate command '{0}'.", commandText);
            }

            var command = new ParserCommand(commandText);

            _commands.Add(command);

            return command;
        }


        public ParserCommand DefaultCommand { get; set; }
        public IEnumerable<ParserCommand> Commands { get { return _commands; } }
        public IEnumerable<ParserNamedParameter> GlobalNamedParameters { get { return _namedParameters; } }


        public ParserNamedParameter AddNamedParameter(string key, bool isBool = false)
        {
            var param = new ParserNamedParameter(key, isBool);

            // TODO - check for duplicates

            _namedParameters.Add(param);

            return param;
        }
    }
}
