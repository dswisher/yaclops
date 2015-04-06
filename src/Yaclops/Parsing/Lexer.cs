using System;
using System.Collections.Generic;
using System.Text;


namespace Yaclops.Parsing
{
    internal class Lexer
    {
        private readonly List<Token> _tokenList = new List<Token>();
        private int _index;



        public Lexer(string text)
        {
            Tokenizer tokenizer = new Tokenizer(text);

            _tokenList.AddRange(tokenizer.Tokenize());
            _tokenList.Add(new Token { Kind = TokenKind.EndOfInput, RawInput = "<end>" });
        }



        public Token Pop()
        {
            if (_index < _tokenList.Count)
            {
                _index += 1;
                return _tokenList[_index - 1];
            }

            throw new InvalidOperationException("Attempt to pop token after EndOfInput.");
        }


        public void Unpush()
        {
            if (_index == 0)
            {
                throw new CommandLineParserException("Attempt to unpush from lexer before pop.");
            }

            _index -= 1;
        }




        private class Tokenizer
        {
            private readonly Queue<char> _input = new Queue<char>();
            private readonly List<Token> _tokens = new List<Token>();


            public Tokenizer(string text)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    foreach (var c in text)
                    {
                        _input.Enqueue(c);
                    }
                }
            }


            public IEnumerable<Token> Tokenize()
            {
                while (_input.Count > 0)
                {
                    if (Char.IsWhiteSpace(Current))
                    {
                        Advance();
                    }
                    else if (Current == '-')
                    {
                        ConsumeParameter();
                    }
                    else if (Current == '"')
                    {
                        Advance();
                        ConsumeQuotedValue();
                    }
                    else
                    {
                        ConsumeValue();
                    }
                }

                return _tokens;
            }


            private void ConsumeParameter()
            {
                StringBuilder raw = new StringBuilder();
                StringBuilder text = new StringBuilder();
                TokenKind kind = TokenKind.ShortName;

                raw.Append(Current);
                Advance();
                while (!End && !Char.IsWhiteSpace(Current) && (Current != '='))
                {
                    if ((Current == '-') && (text.Length == 0))
                    {
                        kind = TokenKind.LongName;
                        raw.Append(Current);
                    }
                    else
                    {
                        raw.Append(Current);
                        text.Append(Current);
                    }

                    Advance();
                }

                _tokens.Add(new Token
                {
                    Kind = kind,
                    Text = text.ToString(),
                    RawInput = raw.ToString()
                });

                if (!End && (Current == '='))
                {
                    Advance();
                }
            }


            private void ConsumeValue()
            {
                StringBuilder builder = new StringBuilder();

                while (!End && !Char.IsWhiteSpace(Current))
                {
                    builder.Append(Current);
                    Advance();
                }

                _tokens.Add(new Token
                {
                    Kind = TokenKind.Value,
                    Text = builder.ToString(),
                    RawInput = builder.ToString()
                });
            }


            private void ConsumeQuotedValue()
            {
                StringBuilder builder = new StringBuilder();

                // TODO - handle unterminated values
                // TODO - handle escaped quotes
                while (!End && (Current != '"'))
                {
                    builder.Append(Current);
                    Advance();
                }

                _tokens.Add(new Token
                {
                    Kind = TokenKind.Value,
                    Text = builder.ToString(),
                    RawInput = builder.ToString()
                });

                if (!End && (Current == '"'))
                {
                    Advance();
                }
            }


            private char Current { get { return _input.Peek(); } }
            private bool End { get { return _input.Count == 0; } }


            private void Advance()
            {
                _input.Dequeue();
            }
        }
    }
}
