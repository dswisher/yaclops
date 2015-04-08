using System;
using System.Collections.Generic;
using Yaclops.Help;

namespace Yaclops.Tests.Formatting
{
    internal class MockConsole : IConsole
    {
        private readonly List<string> _lines = new List<string>();

        public IList<string> Result { get { return _lines; } }


        public void Write(string content)
        {
            throw new NotImplementedException();
        }

        public void Write(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WriteLine()
        {
            throw new NotImplementedException();
        }

        public void WriteLine(string content)
        {
            _lines.Add(content);
        }

        public void WriteLine(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

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
