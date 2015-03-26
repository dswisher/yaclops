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

        // TODO - this should return an object so we can add stuff to it
        public ParserCommand AddCommand(string command)
        {
            // TODO - hack to get first unit test to pass
            _commands.Add(command);

            return new ParserCommand();
        }


        [Obsolete]  // TODO - remove this!
        public bool IsCommand(string text)
        {
            return _commands.Contains(text);
        }
    }
}
