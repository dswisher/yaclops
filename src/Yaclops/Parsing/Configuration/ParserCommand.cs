using System;
using System.Collections.Generic;

namespace Yaclops.Parsing.Configuration
{
    internal class ParserCommand
    {
        private readonly List<string> _aliases = new List<string>();
        private readonly List<ParserNamedParameter> _namedParameters = new List<ParserNamedParameter>();
        private readonly List<ParserPositionalParameter> _positionalParameters = new List<ParserPositionalParameter>();
        private readonly Queue<ParserPositionalParameter> _positionalQueue = new Queue<ParserPositionalParameter>();


        public ParserCommand(string text)
        {
            Text = text;
        }


        public ParserCommand AddAlias(string text)
        {
            _aliases.Add(text);
            return this;
        }


        public ParserNamedParameter AddNamedParameter(string key, Type type)
        {
            var param = new ParserNamedParameter(key, type);

            // TODO - check for duplicates

            _namedParameters.Add(param);

            return param;
        }


        public ParserPositionalParameter AddPositionalParameter(string key, bool isCollection)
        {
            var param = new ParserPositionalParameter(key, isCollection);

            _positionalParameters.Add(param);
            _positionalQueue.Enqueue(param);

            return param;
        }


        public void AddLongName(string name)
        {
            // TODO - this needs to keep a list of long names, and only override the default - not other long names
            Text = name;
        }


        public override string ToString()
        {
            return Text;
        }


        public string Text { get; private set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Aliases { get { return _aliases; } }
        public IEnumerable<ParserNamedParameter> NamedParameters { get { return _namedParameters; } }
        public IEnumerable<ParserPositionalParameter> PositionalParameters { get { return _positionalParameters; } }


        public ParserPositionalParameter PopPositionalParameter()
        {
            if (_positionalQueue.Count == 0)
            {
                return null;
            }

            var param = _positionalQueue.Peek();

            if (!param.IsCollection)
            {
                _positionalQueue.Dequeue();
            }

            return param;
        }
    }
}
