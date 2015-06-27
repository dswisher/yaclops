using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Model;

namespace Yaclops.Commands
{
    internal class YaclopsDumpTreeCommand
    {
        private readonly Dictionary<Type, TypeEntry> _map = new Dictionary<Type, TypeEntry>();
        private readonly CommandNode _start;

        public YaclopsDumpTreeCommand(CommandNode start)
        {
            _start = start;

            _map.Add(typeof(CommandGroup), new TypeEntry { Code = "G", HasChildren = true });
            _map.Add(typeof(Command), new TypeEntry { Code = "C", HasChildren = false });
            _map.Add(typeof(CommandRoot), new TypeEntry { Code = "R", HasChildren = true });
        }


        public void Execute()
        {
            DumpNode(_start, 0);
        }


        private void DumpNode(CommandNode node, int indent)
        {
            TypeEntry entry;
            if (!_map.TryGetValue(node.GetType(), out entry))
            {
                throw new ArgumentException("Node is of an unknown type: " + node.GetType().Name, "node");
            }

            var chars = new string('.', indent);
            Console.WriteLine("{0}[{1}]: {2}", chars, entry.Code, node.Verb);

            if (entry.HasChildren)
            {
                var group = (CommandGroup) node;
                foreach (var child in group.Nodes.OrderBy(x => x.Verb))
                {
                    DumpNode(child, indent + 2);
                }
            }
        }



        private class TypeEntry
        {
            public bool HasChildren { get; set; }
            public string Code { get; set; }
        }
    }
}
