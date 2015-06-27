using System;
using System.Collections.Generic;
using System.Linq;

namespace Yaclops.Model
{
    internal class CommandGroup : CommandNode
    {
        private readonly List<CommandNode> _nodes = new List<CommandNode>();


        public CommandGroup(string verb)
            : base(verb)
        {
        }



        public IList<CommandNode> Nodes { get { return _nodes; } }


        public CommandGroup GetOrAddGroup(string verb)
        {
            var group = _nodes.OfType<CommandGroup>().FirstOrDefault(x => x.Verb == verb);
            if (group == null)
            {
                group = new CommandGroup(verb);
                _nodes.Add(group);
            }

            return group;
        }



        public Command<T> AddCommand<T>(string verb, Func<T> factory)
        {
            if (_nodes.Any(x => x.Verb == verb))
            {
                // TODO - better error message - need context - add links to parent and walk up the stack?
                throw new CommandLineParserException("Duplicate command: " + verb);
            }

            var command = new Command<T>(verb, factory);
            _nodes.Add(command);
            return command;
        }
    }
}
