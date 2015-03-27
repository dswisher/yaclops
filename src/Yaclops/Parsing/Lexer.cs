using System.Collections.Generic;


namespace Yaclops.Parsing
{
    internal class Lexer
    {
        private readonly Queue<string> _queue = new Queue<string>();

        public Lexer(string text)
        {
            // TODO - this version is almost certainly too simplistic!
            string[] bits = text.Split(' ');

            foreach (var item in bits)
            {
                _queue.Enqueue(item);
            }
        }



        public Token Pop()
        {
            if (_queue.Count == 0)
            {
                return null;
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
