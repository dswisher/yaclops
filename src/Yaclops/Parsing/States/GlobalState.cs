

namespace Yaclops.Parsing.States
{
    /// <summary>
    /// Looking for the next token, haven't found a command, yet
    /// </summary>
    internal class GlobalState : AbstractState
    {
        public GlobalState(ParserContext context) : base(context)
        {
        }


        public override AbstractState Advance()
        {
            Token token = Context.Lexer.Pop();

            switch (token.Kind)
            {
                case TokenKind.EndOfInput:
                    if (Context.Command == null)
                    {
                        return new SuccessState(Context);
                    }
                    // TODO - add message, indicating unknown command
                    return new FailureState(Context);

                case TokenKind.Value:
                    Context.Mapper.Advance(token.Text);
                    switch (Context.Mapper.State)
                    {
                        case MapperState.Accepted:
                            // TODO - advance to command state
                            Context.Command = Context.Mapper.Command;
                            Context.Result.Command = Context.Command;
                            return new SuccessState(Context);

                        case MapperState.Rejected:
                            // TODO - add error message
                            return new FailureState(Context);

                        case MapperState.Pending:
                            return this;
                    }
                    return this;

                default:
                    // TODO - hack, for the moment - just fail right away on anything else
                    return new FailureState(Context);
            }
        }
    }
}
