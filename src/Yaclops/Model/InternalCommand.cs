using System;

namespace Yaclops.Model
{
    internal class InternalCommand : Command
    {
        public InternalCommand(CommandNode parent, string verb, Action<CommandRoot, CommandNode> worker)
            : base(parent, verb)
        {
            Worker = worker;
        }


        public Action<CommandRoot, CommandNode> Worker { get; private set; }
    }
}
