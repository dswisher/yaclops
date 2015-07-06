

using Yaclops.Helpers;

namespace Yaclops.Model
{
    internal class CommandRoot : CommandGroup
    {
        private readonly string _helpVerb;

        public CommandRoot(string programName)
            : base(null, null)
        {
            if (string.IsNullOrEmpty(programName))
            {
                _helpVerb = ExeHelpers.Name;
            }
            else
            {
                _helpVerb = programName;
            }
        }


        public override string HelpVerb
        {
            get { return _helpVerb; }
        }
    }
}
