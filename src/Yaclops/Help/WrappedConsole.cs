using System;


namespace Yaclops.Help
{
    /// <summary>
    /// Interface for the bits of the console we use, so we can mock it easily in unit tests
    /// </summary>
    internal interface IConsole
    {
        void Write(string content);
        void Write(string format, params object[] args);

        void WriteLine();
        void WriteLine(string content);
        void WriteLine(string format, params object[] args);

        void StartWrap(string format, params object[] args);
        void EndWrap();

        void WriteTitle(string format, params object[] args);

        void StartIndent();
        void EndIndent();
    }


    /// <summary>
    /// IConsole implementation that writes to the console
    /// </summary>
    internal class WrappedConsole : IConsole
    {
        private bool _wrapping;
        private int _pos;
        private int _indent;


        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string content)
        {
            Console.WriteLine(content);
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }


        public void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }


        public void Write(string content)
        {
            // TODO - make right-margin configurable
            const int rightMargin = 80;

            if (_wrapping)
            {
                // TODO - handle multi-line content
                if ((_pos + content.Length > rightMargin) && (content.Length < rightMargin))
                {
                    Console.WriteLine();
                    Console.Write(new string(' ', _indent));
                    _pos = _indent;
                }

                Console.Write(content);
                _pos += content.Length;
            }
            else
            {
                Console.Write(content);
            }
        }


        public void StartWrap(string format, params object[] args)
        {
            string content = string.Format(format, args);

            Console.Write(content);

            _indent = content.Length;
            _pos = content.Length;
            _wrapping = true;
        }


        public void EndWrap()
        {
            if (_pos > 0)
            {
                WriteLine();
            }

            _wrapping = false;
        }


        public void WriteTitle(string format, params object[] args)
        {
            // TODO - make this stand out - change color or something, rather than this hacky way...
            string msg = string.Format(format, args).ToUpper();

            Console.WriteLine(msg);
            Console.WriteLine(new string('-', msg.Length));
        }


        public void StartIndent()
        {
            _wrapping = true;
            _indent = 3;
            _pos = _indent;
            Console.Write(new string(' ', _indent));
        }


        public void EndIndent()
        {
            EndWrap();
        }
    }
}
