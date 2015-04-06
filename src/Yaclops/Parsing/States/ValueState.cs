using Yaclops.Parsing.Configuration;


namespace Yaclops.Parsing.States
{
    internal class ValueState : AbstractState
    {
        private readonly ParserNamedParameter _param;
        private readonly bool _isGlobal;
        private readonly AbstractState _returnState;


        public ValueState(ParserContext context, ParserNamedParameter param, bool isGlobal, AbstractState returnState)
            : base(context)
        {
            _param = param;
            _isGlobal = isGlobal;
            _returnState = returnState;
        }



        public override AbstractState Advance()
        {
            if (_param.IsBool)
            {
                if (_isGlobal)
                {
                    Context.Result.AddGlobalValue(_param, "true");
                }
                else
                {
                    Context.Result.AddCommandValue(_param, "true");
                }
                return _returnState;
            }

            Token token = Context.Lexer.Pop();

            switch (token.Kind)
            {
                case TokenKind.Value:
                    if (_isGlobal)
                    {
                        Context.Result.AddGlobalValue(_param, token.Text);
                    }
                    else
                    {
                        Context.Result.AddCommandValue(_param, token.Text);
                    }
                    return _returnState;

                default:
                    Context.Result.AddError("Expected value for{1} parameter '{0}'.", _param.Key, _isGlobal ? " global" : string.Empty);
                    return new FailureState(Context);
            }
        }
    }
}
