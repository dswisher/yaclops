

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


        public override AbstractState Advance()
        {
            Token token = Context.Lexer.Pop();

            if (token.Kind == TokenKind.EndOfInput)
            {
                Context.Result.Command = Context.Configuration.DefaultCommand;
                return new SuccessState(Context);
            }

            Context.Lexer.Unpush();
            return new GlobalState(Context);
        }
    }
}
