using System;
using System.Collections.Generic;

namespace Yaclops.Parsing
{
    internal class ParserCommand
    {
        public void AddAlias(string text)
        {
            // TODO
        }
    }


    internal class ParserConfiguration
    {
        private readonly HashSet<string> _commands = new HashSet<string>();
        private string _defaultCommand;


        // TODO - this should return an object so we can add stuff to it
        public ParserCommand AddCommand(string command)
        {
            if (_commands.Contains(command))
            {
                throw new ParserConfigurationException("Cannot add duplicate command '{0}'.", command);
            }

            // TODO - hack to get first unit test to pass
            _commands.Add(command);

            return new ParserCommand();
        }


        public string DefaultCommand
        {
            get { return _defaultCommand; }
            set
            {
                if (!_commands.Contains(value))
                {
                    throw new ParserConfigurationException("Cannot set default command to non-existant command '{0}'.", value);
                }

                _defaultCommand = value;
            }
        }


        [Obsolete]  // TODO - remove this!
        public bool IsCommand(string text)
        {
            return _commands.Contains(text);
        }
    }
}
