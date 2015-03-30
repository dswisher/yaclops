using System.Collections.Generic;
using System.Linq;

namespace Yaclops.Parsing
{
    internal class ParserCommand
    {
        private readonly List<string> _aliases = new List<string>();

        public ParserCommand(string text)
        {
            Text = text;
        }


        public ParserCommand AddAlias(string text)
        {
            _aliases.Add(text);
            return this;
        }


        public override string ToString()
        {
            return Text;
        }


        public string Text { get; private set; }
        public IEnumerable<string> Aliases { get { return _aliases; } }
    }



    internal abstract class ParserParameter
    {
        private readonly HashSet<string> _shortNames = new HashSet<string>();

        protected ParserParameter(string key)
        {
            Key = key;
        }

        public string Key { get; private set; }

        public bool HasShortName(string name)
        {
            return _shortNames.Contains(name);
        }


        public ParserParameter ShortName(string name)
        {
            _shortNames.Add(name);
            return this;
        }
    }


    internal class ParserNamedParameter : ParserParameter
    {
        public ParserNamedParameter(string key)
            : base(key)
        {
        }
    }



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


        public ParserNamedParameter AddNamedParameter(string key)
        {
            var param = new ParserNamedParameter(key);

            // TODO - check for duplicates

            _namedParameters.Add(param);

            return param;
        }
    }
}
