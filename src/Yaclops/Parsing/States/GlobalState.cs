

using System;
using System.Linq;

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
                    if (Context.Mapper.CanAccept(token.RawInput))
                    {
                        Context.Mapper.Advance(token.RawInput);
                        // TODO - assert/throw if state is not Accepted?  Longname command shouldn't be multiple words...
                        Context.Command = Context.Mapper.Command;
                        Context.Result.Command = Context.Mapper.Command;
                        return new CommandState(Context);
                    }
                    var longParam = Context.Configuration.GlobalNamedParameters.FirstOrDefault(x => x.HasLongName(token.Text));
                    if (longParam == null)
                    {
                        Context.Result.AddError("Named parameter '{0}' is not known.", token.Text);
                        return new FailureState(Context);
                    }
                    // TODO - what about a bool, that does not have a value (or at least, may not have a value)?
                    return new GlobalValueState(Context, longParam);

                case TokenKind.ShortName:
                    Console.WriteLine("---> ShortName, token.Text='{0}', RawInput='{1}'", token.Text, token.RawInput);
                    if (Context.Mapper.CanAccept(token.RawInput))
                    {
                        Context.Mapper.Advance(token.RawInput);
                        // TODO - assert/throw if state is not Accepted?  Shortname command shouldn't be multiple words...
                        Context.Command = Context.Mapper.Command;
                        Context.Result.Command = Context.Mapper.Command;
                        return new CommandState(Context);
                    }
                    var shortParam = Context.Configuration.GlobalNamedParameters.FirstOrDefault(x => x.HasShortName(token.Text));
                    if (shortParam == null)
                    {
                        Context.Result.AddError("Named parameter '{0}' is not known.", token.Text);
                        return new FailureState(Context);
                    }
                    // TODO - what about a bool, that does not have a value (or at least, may not have a value)?
                    return new GlobalValueState(Context, shortParam);

                default:
                    // TODO - hack, for the moment - just fail right away on anything else
                    return new FailureState(Context);
            }
        }
    }
}
