using System;
using System.Collections.Generic;

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
        protected ParserParameter(string key)
        {
            Key = key;
        }

        public string Key { get; private set; }

        public ParserParameter ShortName(string name)
        {
            // TODO
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
        private readonly HashSet<string> _commandsOld = new HashSet<string>();


        // TODO - this should return an object so we can add stuff to it
        public ParserCommand AddCommand(string commandText)
        {
            if (_commandsOld.Contains(commandText))
            {
                throw new ParserConfigurationException("Cannot add duplicate command '{0}'.", commandText);
            }

            // TODO - hack to get first unit test to pass
            _commandsOld.Add(commandText);

            var command = new ParserCommand(commandText);

            _commands.Add(command);

            return command;
        }


        public ParserCommand DefaultCommand { get; set; }
        public IEnumerable<ParserCommand> Commands { get { return _commands; } }


        public ParserNamedParameter AddNamedParameter(string key)
        {
            // TODO
            return new ParserNamedParameter(key);
        }


        [Obsolete]  // TODO - remove this!
        public bool IsCommand(string text)
        {
            return _commandsOld.Contains(text);
        }
    }
}
