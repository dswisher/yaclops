using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaclops.Model;

namespace Yaclops.Parsing
{
    internal class ParserState
    {
        private readonly Lexer _lexer;
        private CommandNode _currentNode;
        private Token _currentToken;

        public ParserState(CommandNode startNode, Lexer lexer)
        {
            _currentNode = startNode;
            ExtractParameters(_currentNode);

            _lexer = lexer;
            _currentToken = _lexer.Pop();
        }


        public bool Advance()
        {
            switch (_currentToken.Kind)
            {
                case TokenKind.EndOfInput:
                    return false;

                case TokenKind.LongName:
                case TokenKind.ShortName:
                    // TODO - for now, just skip
                    _currentToken = _lexer.Pop();
                    break;

                case TokenKind.Value:
                    if (_currentNode is Command)
                    {
                        // TODO - at a terminal - this must be a value we need to squirrel away
                        return false;   // TODO - for now, just stop
                    }

                    if (_currentNode is CommandGroup)
                    {
                        // TODO - handle substring matching ("comm" should match "commit", if unique)
                        var next = ((CommandGroup)_currentNode).Nodes.FirstOrDefault(x => x.Verb.Equals(_currentToken.Text, StringComparison.InvariantCultureIgnoreCase));
                        if (next == null)
                        {
                            // TODO - check for help verb and handle it
                        }
                        if (next == null)
                        {
                            // TODO - build up a pretty message
                            throw new CommandLineParserException("Ambiguous/incomplete command!");
                        }

                        // Advance
                        _currentNode = next;
                        ExtractParameters(_currentNode);
                        _currentToken = _lexer.Pop();
                        return true;
                    }

                    // Should never hit this
                    throw new CommandLineParserException("Invalid parser state!");
            }

            return false;
        }


        private void ExtractParameters(CommandNode node)
        {
            // TODO - extract any positional or named parameters from the current node
        }
    }
}
