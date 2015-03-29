using System.Collections.Generic;

namespace Yaclops.Parsing
{
    internal class ParseResult
    {
        public ParseResult()
        {
            GlobalValues = new List<ParsedValue>();
        }


        // The command that was parsed
        public ParserCommand Command { get; set; }

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
