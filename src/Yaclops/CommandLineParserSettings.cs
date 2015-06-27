using System;
using System.Collections.Generic;
using Yaclops.Commands;


namespace Yaclops
{
    public class CommandLineParserSettings<T>
    {
        public CommandLineParserSettings()
        {
            HelpVerb = "help";
            HelpFlags = new[] { "-h", "--help" };
            NullCommand = () => default(T);
        }


        public string HelpVerb { get; set; }
        public IEnumerable<string> HelpFlags { get; set; }

        public Func<T> NullCommand { get; set; }
    }



    public class DefaultSubCommandSettings : CommandLineParserSettings<ISubCommand>
    {
        public DefaultSubCommandSettings()
        {
            NullCommand = () => new NullSubCommand();
        }
    }
}
