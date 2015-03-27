

namespace Yaclops.Parsing.States
{
    internal abstract class AbstractState
    {
        protected AbstractState(ParserContext context)
        {
            Context = context;
        }

        protected ParserContext Context { get; private set; }

        public virtual bool IsTerminal { get { return false; } }

        public abstract AbstractState Advance(Token token);
    }
}
