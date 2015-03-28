

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
                    // TODO - the context command needs to be smarter; this is just quick hack
                    if (Context.Command == null)
                    {
                        Context.Command = token.Text;
                    }
                    else
                    {
                        Context.Command += " " + token.Text;
                    }

                    // TODO - need to switch to a new state, as there may be more input!
                    if (Context.Configuration.IsCommand(Context.Command))
                    {
                        Context.Result.Command = Context.Command;
                        return new SuccessState(Context);
                    }
                    return this;

                default:
                    // TODO - hack, for the moment - just fail right away on anything else
                    return new FailureState(Context);
            }
        }
    }
}
