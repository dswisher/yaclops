

namespace Yaclops.Parsing.States
{
    /// <summary>
    /// The initial state of the parser.
    /// </summary>
    internal class InitialState : AbstractState
    {
        public InitialState(ParserContext context) : base(context)
        {
        }


        public override AbstractState Advance(Token token)
        {
            switch (token.Kind)
            {
                case TokenKind.EndOfInput:
                    Context.Result.Command = Context.Configuration.DefaultCommand;
                    return new SuccessState(Context);

                case TokenKind.Value:
                    // TODO - need to switch to a new state, as there may be more input!
                    if (Context.Configuration.IsCommand(token.Text))
                    {
                        Context.Result.Command = token.Text;
                    }
                    return new SuccessState(Context);

                default:
                    // TODO - hack, for the moment - just fail right away on anything else
                    return new FailureState(Context);
            }
        }
    }
}
