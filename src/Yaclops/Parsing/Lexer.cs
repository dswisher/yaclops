using System;
using System.Collections.Generic;


namespace Yaclops.Parsing
{
    internal class Lexer
    {
        private readonly Queue<string> _queue = new Queue<string>();
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
            if (_queue.Count == 0)
            {
                if (_endSent)
                {
                    throw new InvalidOperationException("Attempt to pop token after EndOfInput.");
                }

                _endSent = true;
                return new Token { Kind = TokenKind.EndOfInput };
            }

            string item = _queue.Dequeue();

            if (item.StartsWith("--"))
            {
                return new Token { Kind = TokenKind.LongName, Text = item.Substring(2) };
            }

            if (item.StartsWith("-"))
            {
                return new Token { Kind = TokenKind.ShortName, Text = item.Substring(1) };
            }

            return new Token { Kind = TokenKind.Value, Text = item };
        }

    }
}
