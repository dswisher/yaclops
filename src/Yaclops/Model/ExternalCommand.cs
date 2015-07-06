using System;


namespace Yaclops.Model
{
    internal abstract class ExternalCommand : Command
    {
        protected ExternalCommand(CommandNode parent, string verb)
            : base(parent, verb)
        {
        }
    }



    internal class ExternalCommand<T> : ExternalCommand
    {
        public ExternalCommand(CommandNode parent, string verb, Func<T> factory)
            : base(parent, verb)
        {
            Factory = factory;
        }


        public Func<T> Factory { get; private set; }
    }
}
