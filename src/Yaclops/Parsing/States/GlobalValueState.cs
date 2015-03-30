

namespace Yaclops.Parsing.States
{
    /// <summary>
    /// We've seen a global parameter, and now just need the value to go with it.
    /// </summary>
    internal class GlobalValueState : AbstractState
    {
        private readonly ParserNamedParameter _param;


        public GlobalValueState(ParserContext context, ParserNamedParameter param) : base(context)
        {
            _param = param;
        }


        public override AbstractState Advance()
        {
            Token token = Context.Lexer.Pop();

            switch (token.Kind)
            {
                case TokenKind.Value:
                    Context.Result.AddValue(_param, token.Text);
                    return new GlobalState(Context);

                default:
                    Context.Result.AddError("Expected value for global parameter '{0}'.", _param.Key);
                    return new FailureState(Context);
            }
        }
    }
}
