using System;
using System.Linq;
using Yaclops.Parsing.Configuration;


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

                case TokenKind.ShortName:
                    return HandleNamedParameter(token, (t, p) => p.HasShortName(t.Text));

                case TokenKind.LongName:
                    return HandleNamedParameter(token, (t, p) => p.HasLongName(t.Text));

                case TokenKind.Value:
                    return HandlePositionalParameter(token);

                default:
                    Context.Result.AddError("Unexpected input: '{0}'.", token.RawInput);
                    return new FailureState(Context);
            }
        }



        private AbstractState HandlePositionalParameter(Token token)
        {
            var param = Context.Command.PopPositionalParameter();
            if (param == null)
            {
                Context.Result.AddError("Unexpected input: '{0}'.", token.RawInput);
                return new FailureState(Context);
            }

            Context.Result.AddCommandValue(param, token.Text);
            return this;
        }



        private AbstractState HandleNamedParameter(Token token, Func<Token, ParserNamedParameter, bool> hasName)
        {
            var param = Context.Command.NamedParameters.FirstOrDefault(x => hasName(token, x));
            if (param == null)
            {
                // TODO - check for global named parameter, too
                Context.Result.AddError("Named parameter '{0}' is not known.", token.RawInput);
                return new FailureState(Context);
            }

            if (param.IsBool)
            {
                Context.Result.AddCommandValue(param, "true");
                return this;
            }

            return new CommandValueState(Context, param);
        }
    }
}
