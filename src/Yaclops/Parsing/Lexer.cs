using System;
using System.Collections.Generic;


namespace Yaclops.Parsing
{
    internal class Lexer
    {
        private readonly Queue<string> _queue = new Queue<string>();
        private Token _unpushed;
        private Token _lastToken;
        private bool _endSent;


        public Lexer(string text)
        {
            // TODO - this version is almost certainly too simplistic!

            if (!string.IsNullOrEmpty(text))
            {
                string[] bits = text.Split(' ');

                foreach (var item in bits)
                {
                    _queue.Enqueue(item);
                }
            }
        }



        public Token Pop()
        {
            if (_unpushed != null)
            {
                var token = _unpushed;
                _unpushed = null;
                _lastToken = token;

                return token;
            }

            if (_queue.Count == 0)
            {
                if (_endSent)
                {
                    throw new InvalidOperationException("Attempt to pop token after EndOfInput.");
                }

                _endSent = true;
                return Return(TokenKind.EndOfInput, "<end>", null);
            }

            string item = _queue.Dequeue();

            if (item.StartsWith("--"))
            {
                return Return(TokenKind.LongName, item, item.Substring(2));
            }

            if (item.StartsWith("-"))
            {
                return Return(TokenKind.ShortName, item, item.Substring(1));
            }

            return Return(TokenKind.Value, item, item);
        }



        public void Unpush()
        {
            if (_lastToken == null)
            {
                throw new CommandLineParserException("Attempt to unpush from lexer before pop.");
            }

            _unpushed = _lastToken;
        }



        private Token Return(TokenKind kind, string rawInput, string text)
        {
            var token = new Token { Kind = kind, RawInput = rawInput, Text = text };

            _lastToken = token;

            return token;
        }
    }
}
