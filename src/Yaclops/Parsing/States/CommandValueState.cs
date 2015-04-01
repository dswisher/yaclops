

namespace Yaclops.Parsing.States
{
    internal class CommandValueState : AbstractState
    {
        private readonly ParserNamedParameter _param;

        public CommandValueState(ParserContext context, ParserNamedParameter param)
            : base(context)
        {
            _param = param;
        }


        // TODO - merge this with GlobalValueState, perhaps by having a ReturnState to go back to GlobalState/CommandState?
        public override AbstractState Advance()
        {
            Token token = Context.Lexer.Pop();

            switch (token.Kind)
            {
                case TokenKind.Value:
                    Context.Result.AddCommandValue(_param, token.Text);
                    return new CommandState(Context);

                default:
                    Context.Result.AddError("Expected value for global parameter '{0}'.", _param.Key);
                    return new FailureState(Context);
            }
        }
    }
}
