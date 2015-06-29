using System;


namespace Yaclops.Model
{
    internal abstract class ExternalCommand : Command
    {
        protected ExternalCommand(string verb) : base(verb)
        {
        }
    }



    internal class ExternalCommand<T> : ExternalCommand
    {
        public ExternalCommand(string verb, Func<T> factory)
            : base(verb)
        {
            Factory = factory;
        }


        public Func<T> Factory { get; private set; }
    }
}
