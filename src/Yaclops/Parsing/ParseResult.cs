using System.Collections.Generic;

namespace Yaclops.Parsing
{
    internal class ParseResult
    {
        private readonly List<string> _errors = new List<string>();

        public ParseResult()
        {
            GlobalValues = new List<ParsedValue>();
        }


        public void AddError(string format, params object[] args)
        {
            _errors.Add(string.Format(format, args));
        }


        // The command that was parsed
        public ParserCommand Command { get; set; }

        public IEnumerable<string> Errors { get { return _errors; } }

        // The list of parsed parameters that are global - not associated with a specific command
        public IEnumerable<ParsedValue> GlobalValues { get; private set; }
    }


    // TODO - move this to a separate file?
    internal class ParsedValue
    {
        public string Name { get; set; }
        // TODO - how to represent the value, given we want to support collections?
    }
}
