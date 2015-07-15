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
            EnableYaclopsCommands = true;
            ChildThreshold = 3;
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

        /// <summary>
        /// The name of the program. The default is the executable name.
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// When printing the command summary, if a group has fewer child commands
        /// than this threshold, the children are listed. If there are more children,
        /// only the group is listed. The default is 3.
        /// </summary>
        public int ChildThreshold { get; set; }
    }
}
