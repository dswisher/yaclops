using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Parsing.Configuration;
using Yaclops.Parsing.States;

namespace Yaclops.Parsing
{
    internal class Parser
    {
        private readonly ParserConfiguration _configuration;


        public Parser(ParserConfiguration configuration)
        {
            _configuration = configuration;
        }


        public ParseResult Parse(string text)
        {
            ParserContext context = new ParserContext(_configuration, text);

            AbstractState state = new InitialState(context);
            while (!state.IsTerminal)
            {
                state = state.Advance();
            }

            CheckForMissingRequiredParameters(context);

            return context.Result;
        }


        public ParseResult Parse(IEnumerable<string> args)
        {
            return Parse(string.Join(" ", args.Select(Quote)));
        }


        private void CheckForMissingRequiredParameters(ParserContext context)
        {
            if (context.Command != null)
            {
                var missing = context.Command.RemainingPositionalParameters.Where(x => x.IsRequired);

                foreach (var p in missing)
                {
                    bool ok = false;

                    if (p.IsCollection)
                    {
                        ok = context.Result.CommandListValues.Any(x => x.Name.Equals(p.Name, StringComparison.InvariantCultureIgnoreCase));
                    }

                    if (!ok)
                    {
                        context.Result.AddError("Required parameter '{0}' was not specified.", p.Name);
                    }
                }
            }
        }


        private string Quote(string s)
        {
            // TODO - check for other whitespace chars
            if (s.Contains(' '))
            {
                return '"' + s + '"';
            }

            return s;
        }
    }
}
