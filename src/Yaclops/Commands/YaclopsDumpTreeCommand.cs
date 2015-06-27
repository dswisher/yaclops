using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Model;

namespace Yaclops.Commands
{
    internal class YaclopsDumpTreeCommand
    {
        private readonly List<TypeEntry> _entries = new List<TypeEntry>();
        private readonly CommandNode _start;

        public YaclopsDumpTreeCommand(CommandNode start)
        {
            _start = start;

            _entries.Add(new TypeEntry { Type = typeof(CommandRoot), Code = "R", HasChildren = true });
            _entries.Add(new TypeEntry { Type = typeof(CommandGroup), Code = "G", HasChildren = true });
            _entries.Add(new TypeEntry { Type = typeof(Command), Code = "C", HasChildren = false });
        }


        public void Execute()
        {
            DumpNode(_start, 0);
        }


        private void DumpNode(CommandNode node, int indent)
        {
            TypeEntry entry = _entries.FirstOrDefault(x => x.Type.IsInstanceOfType(node));
            if (entry == null)
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
            public Type Type { get; set; }
            public bool HasChildren { get; set; }
            public string Code { get; set; }
        }
    }
}
