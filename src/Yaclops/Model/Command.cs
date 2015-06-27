using System;

namespace Yaclops.Model
{
    internal abstract class Command : CommandNode
    {
        protected Command(string verb) : base(verb)
        {
        }

    }


    internal class Command<T> : Command
    {
        public Command(string verb, Func<T> factory)
            : base(verb)
        {
            Factory = factory;
        }


        public Func<T> Factory { get; private set; }
    }
}
