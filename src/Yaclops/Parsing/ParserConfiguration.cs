using System;
using System.Collections.Generic;
using System.Linq;

namespace Yaclops.Parsing
{
    internal class ParserCommand
    {
        private readonly List<string> _aliases = new List<string>();
        private readonly List<ParserNamedParameter> _namedParameters = new List<ParserNamedParameter>();


        public ParserCommand(string text)
        {
            Text = text;
        }


        public ParserCommand AddAlias(string text)
        {
            _aliases.Add(text);
            return this;
        }


        public ParserNamedParameter AddNamedParameter(string key, Type type)
        {
            var param = new ParserNamedParameter(key, type);

            // TODO - check for duplicates

            _namedParameters.Add(param);

            return param;
        }


        public override string ToString()
        {
            return Text;
        }


        public string Text { get; private set; }
        public IEnumerable<string> Aliases { get { return _aliases; } }
        public IEnumerable<ParserNamedParameter> NamedParameters { get {  return _namedParameters; } }
    }



    internal abstract class ParserParameter
    {
        private readonly HashSet<string> _shortNames = new HashSet<string>();
        private readonly HashSet<string> _longNames = new HashSet<string>();
        private bool _longNameIsDefault;

        protected ParserParameter(string key)
        {
            Key = key;

            // Set default long name
            _longNames.Add(key.Decamel('-').ToLower());
            _longNameIsDefault = true;
        }

        public string Key { get; private set; }

        public bool HasShortName(string text)
        {
            return _shortNames.Contains(text);
        }

        public bool HasLongName(string text)
        {
            return _longNames.Contains(text);
        }

        public ParserParameter ShortName(string name)
        {
            _shortNames.Add(name);
            return this;
        }

        public ParserParameter LongName(string name)
        {
            if (_longNameIsDefault)
            {
                _longNameIsDefault = false;
                _longNames.Clear();
            }

            _longNames.Add(name);

            return this;
        }
    }


    internal class ParserNamedParameter : ParserParameter
    {
        public ParserNamedParameter(string key, Type type)
            : base(key)
        {
            IsBool = type == typeof(bool);
        }


        public bool IsBool { get; private set; }
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


        public ParserNamedParameter AddNamedParameter(string key, Type type)
        {
            var param = new ParserNamedParameter(key, type);

            // TODO - check for duplicates

            _namedParameters.Add(param);

            return param;
        }
    }
}
