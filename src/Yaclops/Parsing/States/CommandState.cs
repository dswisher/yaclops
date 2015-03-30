using System.Linq;


namespace Yaclops.Parsing.States
{
    internal class CommandState : AbstractState
    {
        public CommandState(ParserContext context) : base(context)
        {
        }


        public override AbstractState Advance()
        {
            Token token = Context.Lexer.Pop();

            switch (token.Kind)
            {
                case TokenKind.EndOfInput:
                    return new SuccessState(Context);

                case TokenKind.LongName:
                    var longParam = Context.Command.NamedParameters.FirstOrDefault(x => x.HasLongName(token.Text));
                    if (longParam == null)
                    {
                        // TODO - check for global command, too
                        Context.Result.AddError("Named parameter '{0}' is not known.", token.Text);
                        return new FailureState(Context);
                    }
                    // TODO - what about a bool, that does not have a value (or at least, may not have a value)?
                    return new CommandValueState(Context, longParam);

                default:
                    return new FailureState(Context);
            }
        }
    }
}
