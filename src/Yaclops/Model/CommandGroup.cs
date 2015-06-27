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



        public Command AddCommand(string verb)
        {
            if (_nodes.Any(x => x.Verb == verb))
            {
                // TODO - better error message - need context - add links to parent and walk up the stack?
                throw new CommandLineParserException("Duplicate command: " + verb);
            }

            var command = new Command(verb);
            _nodes.Add(command);
            return command;
        }
    }
}
