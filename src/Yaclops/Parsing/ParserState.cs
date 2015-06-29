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
        private readonly Queue<CommandPositionalParameter> _positionalParameters = new Queue<CommandPositionalParameter>();
        private CommandNamedParameter _currentParameter;

        public ParserState(CommandNode startNode, Lexer lexer)
        {
            NamedParameters = new List<ParserNamedParameterResult>();
            PositionalParameters = new List<ParserPositionalParameterResult>();
            CurrentNode = startNode;
            ExtractParameters(CurrentNode);

            _lexer = lexer;
            CurrentToken = _lexer.Pop();
        }


        public CommandNode CurrentNode { get; private set; }
        public Token CurrentToken { get; private set; }
        public List<ParserNamedParameterResult> NamedParameters { get; private set; }
        public List<ParserPositionalParameterResult> PositionalParameters { get; private set; }


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
                    if (_shortNames.ContainsKey(CurrentToken.Text))
                    {
                        _currentParameter = _shortNames[CurrentToken.Text];
                        CurrentToken = _lexer.Pop();
                        return true;
                    }
                    // TODO - check for help flag
                    throw new CommandLineParserException("Unknown named parameter: " + CurrentToken.Text);

                case TokenKind.Value:
                    if (CurrentNode is Command)
                    {
                        if (_positionalParameters.Count > 0)
                        {
                            var commandParam = _positionalParameters.Peek();
                            if (commandParam.IsList)
                            {
                                // A list; create a new param or add to an existing one
                                if ((PositionalParameters.Count == 0) || (PositionalParameters.Last().Parameter != commandParam))
                                {
                                    PositionalParameters.Add(new ParserPositionalParameterResult(commandParam, CurrentToken.Text));
                                }
                                else
                                {
                                    PositionalParameters.Last().Values.Add(CurrentToken.Text);
                                }
                            }
                            else
                            {
                                // Not a list; always create a new param
                                PositionalParameters.Add(new ParserPositionalParameterResult(commandParam, CurrentToken.Text));
                                _positionalParameters.Dequeue();
                            }

                            CurrentToken = _lexer.Pop();
                            return true;
                        }

                        throw new CommandLineParserException("Unexpected positional parameter: " + CurrentToken.Text);
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

            foreach (var p in node.PositionalParameters)
            {
                _positionalParameters.Enqueue(p);
            }
        }
    }
}
