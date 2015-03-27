using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaclops.Parsing
{
    internal class Lexer
    {
        private readonly Queue<string> _queue = new Queue<string>();

        public Lexer(string text)
        {
            // TODO - this version is too simplistic!
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

            if (item.StartsWith("-"))
            {
                return new Token { Kind = TokenKind.Name };
            }

            return new Token { Kind = TokenKind.Value };
        }

    }
}
