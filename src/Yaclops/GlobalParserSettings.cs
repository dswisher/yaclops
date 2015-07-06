using System.Collections.Generic;


namespace Yaclops
{
    /// <summary>
    /// Settings that alter the default behavior of the parser, but do not depend on the command type.
    /// </summary>
    public class GlobalParserSettings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GlobalParserSettings()
        {
            HelpVerb = "help";
            HelpFlags = new[] { "-h", "--help", "-?" };
        }

        /// <summary>
        /// The verb that indicates help is desired. Defaults to "help".
        /// </summary>
        public string HelpVerb { get; set; }

        /// <summary>
        /// List of strings that indicate help is desired. Defaults to -h, -? and --help.
        /// </summary>
        public IEnumerable<string> HelpFlags { get; set; }

        /// <summary>
        /// Enable (hidden) internal Yaclops command used for debugging.
        /// </summary>
        public bool EnableYaclopsCommands { get; set; }
    }
}
