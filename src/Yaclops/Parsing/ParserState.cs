using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Model;

namespace Yaclops.Parsing
{
    internal class ParserState
    {
        private readonly Lexer _lexer;
        private readonly Dictionary<string, CommandNamedParameter> _longNames = new Dictionary<string, CommandNamedParameter>();
        private readonly Dictionary<string, CommandNamedParameter> _shortNames = new Dictionary<string, CommandNamedParameter>();
        private CommandNamedParameter _currentParameter;

        public ParserState(CommandNode startNode, Lexer lexer)
        {
            NamedParameters = new List<ParserNamedParameterResult>();
            CurrentNode = startNode;
            ExtractParameters(CurrentNode);

            _lexer = lexer;
            CurrentToken = _lexer.Pop();
        }


        public CommandNode CurrentNode { get; private set; }
        public Token CurrentToken { get; private set; }
        public List<ParserNamedParameterResult> NamedParameters { get; private set; }


        public bool Advance()
        {
            if (_currentParameter != null)
            {
                switch (CurrentToken.Kind)
                {
                    case TokenKind.EndOfInput:
                        HandleMissingNamedParameterValue();
                        return false;

                    case TokenKind.LongName:
                    case TokenKind.ShortName:
                        HandleMissingNamedParameterValue();
                        break;

                    case TokenKind.Value:
                        NamedParameters.Add(new ParserNamedParameterResult(_currentParameter, CurrentToken.Text));
                        _currentParameter = null;
                        CurrentToken = _lexer.Pop();
                        return true;
                }
            }

            return HandleDefaultState();
        }



        private void HandleMissingNamedParameterValue()
        {
            if (!_currentParameter.IsBool)
            {
                throw new CommandLineParserException("No value specified for parameter: " + _currentParameter.PropertyName);
            }

            NamedParameters.Add(new ParserNamedParameterResult(_currentParameter, true.ToString()));
            _currentParameter = null;
        }



        private bool HandleDefaultState()
        {
            switch (CurrentToken.Kind)
            {
                case TokenKind.EndOfInput:
                    return false;

                case TokenKind.LongName:
                    // TODO - check for prefixes?
                    if (_longNames.ContainsKey(CurrentToken.Text))
                    {
                        _currentParameter = _longNames[CurrentToken.Text];
                        CurrentToken = _lexer.Pop();
                        return true;
                    }
                    // TODO - check for help flag
                    throw new CommandLineParserException("Unknown named parameter: " + CurrentToken.Text);

                case TokenKind.ShortName:
                    // TODO - named parameters
                    throw new NotImplementedException("Short-named parameter handling is TBD");

                case TokenKind.Value:
                    if (CurrentNode is Command)
                    {
                        // TODO - at a terminal - this must be a value we need to squirrel away
                        throw new NotImplementedException("Positional parameter handling is TBD");
                    }

                    if (CurrentNode is CommandGroup)
                    {
                        // TODO - handle substring matching ("comm" should match "commit", if unique)
                        var next = ((CommandGroup)CurrentNode).Nodes.FirstOrDefault(x => x.Verb.Equals(CurrentToken.Text, StringComparison.InvariantCultureIgnoreCase));
                        if (next == null)
                        {
                            // TODO - check for help verb and handle it
                        }
                        if (next == null)
                        {
                            // TODO - build up a pretty message
                            throw new CommandLineParserException("Unknown command!");
                        }

                        // Advance
                        CurrentNode = next;
                        ExtractParameters(CurrentNode);
                        CurrentToken = _lexer.Pop();
                        return true;
                    }

                    // Should never hit this
                    throw new CommandLineParserException("Invalid parser state!");
            }

            return false;
        }



        private void ExtractParameters(CommandNode node)
        {
            foreach (var p in node.NamedParameters)
            {
                foreach (var name in p.LongNames)
                {
                    _longNames[name] = p;
                }

                foreach (var name in p.ShortNames)
                {
                    _shortNames[name] = p;
                }
            }

            // TODO - extract any positional from the node
        }
    }
}
