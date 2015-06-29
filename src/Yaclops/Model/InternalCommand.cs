using System;


namespace Yaclops.Model
{
    internal class InternalCommand : Command
    {
        public InternalCommand(string verb, Action<CommandRoot, CommandNode> worker)
            : base(verb)
        {
            Worker = worker;
        }


        public Action<CommandRoot, CommandNode> Worker { get; private set; }
    }
}
