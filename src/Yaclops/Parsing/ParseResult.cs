using System.Collections.Generic;

namespace Yaclops.Parsing
{
    internal class ParseResult
    {
        private readonly List<string> _errors = new List<string>();
        private readonly List<ParsedValue> _globalValues = new List<ParsedValue>();
        private readonly List<ParsedValue> _values = new List<ParsedValue>();



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
    }


    // TODO - move this to a separate file? Or inside ParseResult?
    internal class ParsedValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        // TODO - how to represent the value, given we want to support collections?
    }
}
