using System.Collections.Generic;
using Yaclops.Parsing.Configuration;

namespace Yaclops.Parsing
{
    internal class ParseResult
    {
        private readonly List<string> _errors = new List<string>();
        private readonly List<ParsedValue> _globalValues = new List<ParsedValue>();
        private readonly List<ParsedValue> _values = new List<ParsedValue>();
        private readonly Dictionary<string, ParsedListValue> _listValues = new Dictionary<string, ParsedListValue>();



        public void AddError(string format, params object[] args)
        {
            _errors.Add(string.Format(format, args));
        }


        // The command that was parsed
        public ParserCommand Command { get; set; }

        public IEnumerable<string> Errors { get { return _errors; } }

        /// <summary>
        /// The list of parsed parameters that are global - not associated with a specific command
        /// </summary>
        public IEnumerable<ParsedValue> GlobalValues { get { return _globalValues; } }


        /// <summary>
        /// The list of parameters that are tied to the command
        /// </summary>
        public IEnumerable<ParsedValue> CommandValues { get { return _values; } }

        /// <summary>
        /// The list of list (collection) parameters that are tied to the command
        /// </summary>
        public IEnumerable<ParsedListValue> CommandListValues { get { return _listValues.Values; } }


        public ParsedValue AddGlobalValue(ParserParameter parameter, string value)
        {
            var item = new ParsedValue { Name = parameter.Key, Value = value };

            _globalValues.Add(item);

            return item;
        }



        public ParsedValue AddCommandValue(ParserParameter parameter, string value)
        {
            var item = new ParsedValue { Name = parameter.Key, Value = value };

            _values.Add(item);

            return item;
        }



        public ParsedListValue AddCommandListValue(ParserParameter parameter, string value)
        {
            ParsedListValue item;

            if (!_listValues.TryGetValue(parameter.Key, out item))
            {
                item = new ParsedListValue(parameter.Key);
                _listValues.Add(parameter.Key, item);
            }

            item.Values.Add(value);

            return item;
        }



        internal class ParsedListValue
        {
            private readonly List<string> _values = new List<string>();

            public ParsedListValue(string name)
            {
                Name = name;
            }

            public string Name { get; private set; }
            public List<string> Values { get { return _values; } }
        }
    }


    // TODO - move this inside ParseResult
    internal class ParsedValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
