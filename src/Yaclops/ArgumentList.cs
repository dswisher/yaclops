using System;
using System.Collections.Generic;
using System.Linq;

namespace Yaclops
{
    internal class ArgumentList
    {
        private readonly string[] _args;
        private int _position;


        public ArgumentList(IEnumerable<string> args)
        {
            if (args == null)
            {
                IsEmpty = true;
                _args = new string[0];
                return;
            }

            var strings = args.ToArray();

            if (!strings.Any())
            {
                IsEmpty = true;
                _args = new string[0];
                return;
            }

            _args = strings.ToArray();
        }


        public bool IsEmpty { get; private set; }


        public bool HasRemaining(int count = 1)
        {
            return _position + count <= _args.Length;
        }


        public string Peek(int index = 0)
        {
            return _args[_position + index];
        }


        public string Pop()
        {
            string value = Peek();

            _position += 1;

            return value;
        }


        public int Matches(ICommand command)
        {
            string[] words = command.AsWords();

            if (words.Length > _args.Length)
            {
                return 0;
            }

            for (int i = 0; i < words.Length; i++)
            {
                if (!words[i].Equals(_args[i], StringComparison.InvariantCultureIgnoreCase))
                {
                    return 0;
                }
            }

            return words.Length;
        }


        public void Accept(ICommand command)
        {
            _position = command.AsWords().Length;
        }
    }
}
