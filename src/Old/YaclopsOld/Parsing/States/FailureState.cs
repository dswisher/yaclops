

namespace Yaclops.Parsing.States
{
    /// <summary>
    /// Something went wrong.
    /// </summary>
    internal class FailureState : AbstractTerminalState
    {
        public FailureState(ParserContext context) : base(context)
        {
        }
    }
}
