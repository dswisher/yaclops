using System;


namespace Yaclops.Parsing.States
{
    internal class AbstractTerminalState : AbstractState
    {
        protected AbstractTerminalState(ParserContext context) : base(context)
        {
        }


        public override bool IsTerminal
        {
            get { return true; }
        }


        public override AbstractState Advance()
        {
            throw new InvalidOperationException("Should not attempt to advance out of a terminal state.");
        }
    }
}
