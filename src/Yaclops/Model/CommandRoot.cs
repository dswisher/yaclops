

namespace Yaclops.Model
{
    internal class CommandRoot : CommandGroup
    {
        public CommandRoot()
            : base(null, null)
        {
        }


        public override string HelpVerb
        {
            // TODO - get the name of the command!
            get { return "COMMAND"; }
        }
    }
}
