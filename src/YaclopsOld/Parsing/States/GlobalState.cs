

using System;
using System.Linq;
using Yaclops.Parsing.Configuration;

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
                    if (Context.Mapper.State == MapperState.Pending)
                    {
                        Context.Result.AddError("Command '{0}' is ambiguous.", Context.Mapper.CommandText);
                        return new FailureState(Context);
                    }
                    return new SuccessState(Context);

                case TokenKind.Value:
                    Context.Mapper.Advance(token.Text);
                    switch (Context.Mapper.State)
                    {
                        case MapperState.Accepted:
                            Context.Command = Context.Mapper.Command;
                            Context.Result.Command = Context.Mapper.Command;
                            return new CommandState(Context);

                        case MapperState.Rejected:
                            Context.Result.AddError("Command '{0}' is not known.", Context.Mapper.CommandText);
                            return new FailureState(Context);

                        case MapperState.Pending:
                            return this;
                    }
                    return this;

                case TokenKind.LongName:
                    return HandleNamedParameter(token, (t, p) => p.HasLongName(t.Text));

                case TokenKind.ShortName:
                    return HandleNamedParameter(token, (t, p) => p.HasShortName(t.Text));

                default:
                    return new FailureState(Context);
            }
        }



        private AbstractState HandleNamedParameter(Token token, Func<Token, ParserNamedParameter, bool> hasName)
        {
            if (Context.Mapper.CanAccept(token.RawInput))
            {
                Context.Mapper.Advance(token.RawInput);
                // TODO - assert/throw if state is not Accepted?  Short/Long-name command shouldn't be multiple words...
                Context.Command = Context.Mapper.Command;
                Context.Result.Command = Context.Mapper.Command;
                return new CommandState(Context);
            }

            var param = Context.Configuration.GlobalNamedParameters.FirstOrDefault(x => hasName(token, x));
            if (param == null)
            {
                Context.Result.AddError("Named parameter '{0}' is not known.", token.RawInput);
                return new FailureState(Context);
            }

            return new ValueState(Context, param, true, this);
        }
    }
}
