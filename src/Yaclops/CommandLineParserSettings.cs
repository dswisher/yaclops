using System.Collections.Generic;


namespace Yaclops
{
    public class CommandLineParserSettings
    {
        public CommandLineParserSettings()
        {
            HelpVerb = "help";
            HelpFlags = new[] { "-h", "--help" };
        }


        public string HelpVerb { get; set; }
        public IEnumerable<string> HelpFlags { get; set; }
    }
}
