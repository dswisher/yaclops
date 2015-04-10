using System;
using System.Collections.Generic;
using System.Text;
using Yaclops.Help;

namespace Yaclops.Tests.Formatting
{
    internal class MockConsole : IConsole
    {
        private readonly List<string> _lines = new List<string>();
        private StringBuilder _currentLine = new StringBuilder();

        public IList<string> Result { get { return _lines; } }
        public int Width { get; private set; }


        public MockConsole(int width)
        {
            Width = width;
        }


        public void Write(string content)
        {
            _currentLine.Append(content);
        }

        public void Write(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WriteLine()
        {
            _lines.Add(_currentLine.ToString());
            _currentLine.Clear();
        }

        public void WriteLine(string content)
        {
            Write(content);
            WriteLine();
        }

        public void WriteLine(string format, params object[] args)
        {
            throw new NotImplementedException();
        }




        // TODO - get rid of these methods, once the new formatting code is done!
        public void StartWrap(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void EndWrap()
        {
            throw new NotImplementedException();
        }

        public void WriteTitle(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void StartIndent()
        {
            throw new NotImplementedException();
        }

        public void EndIndent()
        {
            throw new NotImplementedException();
        }
    }
}
